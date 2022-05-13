using System;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	[Serializable]
	public class DropPieceAction : AShogiAction
	{
		public DropPieceAction() : base(){}

		public DropPieceAction( Piece piece ) : base( piece ) {
		}

		public DropPieceAction( Piece piece, int destinationX, int destinationY ) : base( piece, destinationX, destinationY ) {
		}

		public override string ToString() {
			return "Drop "+base.ToString();
		}

		public override async UniTask EnableLastMoveFX(GameSettings settings){
			await ActingPiece.GetComponent<IHighlightFx>().EnableHighlight( settings.GetLastMovedPiece_Color( playerId ) );
		}

		public override void DisableLastMoveFX() {
			ActingPiece.GetComponent<IHighlightFx>().DisableHighlight();
		}

		public override async UniTask ExecuteAction( ShogiGame game ) {
			base.ExecuteAction(game).Forget();
			UnityEngine.Debug.Log($"Dropping piece {ActingPiece} on ({DestinationX},{DestinationY})");

			//TODO: replace with animation
			ActingPiece.PlacePieceOnCell_Immediate( DestinationX, DestinationY );
			await ActingPiece.GetComponent<IPieceDropActionFX>().DoDropAnimation( DestinationX, DestinationY );
			await EnableLastMoveFX( game.settings );

			//Update game data structures
			game.GetSideBoard( ActingPiece.OwnerId ).RemoveCapturedPiece( ActingPiece );
			UpdateBoard( game );
			ActingPiece.X = DestinationX;
			ActingPiece.Y = DestinationY;
			ActingPiece.IsCaptured = false;
		}

		public void UpdateBoard( ShogiGame game ) {
			game.board [DestinationX, DestinationY] = ActingPiece;
		}

		//TODO: Maybe I should create a DropPawnAction class, and move this code there. 
		//I don't need to do pawn validation if I'm dropping other pieces. It might make sense
		//It's more Object Oriented, but maybe our if is good enough in this case. 
		public override bool IsMoveValid( ShogiGame game ) {

			bool isValidPieceMovement = ActingPiece.GetValidMoves().Any( m => m.x == DestinationX && m.y == DestinationY );
			bool willBeAbleToMove_FromDestination = ActingPiece.defaultMovement.GetAvailableMoves( DestinationX, DestinationY ).Any();

			if(ActingPiece.PieceType == PieceType.Pawn){
				if(AnyUnpromotedPawns_OnColumn()){
					isValidPieceMovement = false;
				}
			}
			return isValidPieceMovement && willBeAbleToMove_FromDestination;

			//Local methods
			bool AnyUnpromotedPawns_OnColumn(){
				for (int y = 0 ; y < 9 ; y++) {
					Piece piece = game.board [DestinationX, y];
					bool isPawn = piece?.PieceType == PieceType.Pawn;
					bool isUnpromotedPawn = isPawn && piece.IsPromoted == false;

					if (isUnpromotedPawn) {
						return true;
					}
				}
				return false;
			}
		}


	}
}
