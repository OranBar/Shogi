using System;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	[Serializable]
	public class DropPieceAction : AShogiAction
	{
		public DropPieceAction( Piece piece ) : base( piece ) {
		}

		public DropPieceAction( Piece piece, int destinationX, int destinationY ) : base( piece, destinationX, destinationY ) {
		}

		public override string ToString() {
			return "Drop "+base.ToString();
		}

		public override void DisableFX() {
			var actingPiece = GetActingPiece();

			actingPiece.GetComponent<IHighlightFx>().DisableHighlight();
		}

		public override async UniTask ExecuteAction( ShogiGame game ) {
			base.ExecuteAction(game).Forget();
			var actingPiece = GetActingPiece();
			UnityEngine.Debug.Log($"Dropping piece {actingPiece} on ({DestinationX},{DestinationY})");

			//TODO: replace with animation
			actingPiece.PlacePieceOnCell_Immediate( DestinationX, DestinationY );
			await actingPiece.GetComponent<IPieceDropActionFX>().DoDropAnimation( DestinationX, DestinationY );
			await actingPiece.GetComponent<IHighlightFx>().EnableHighlight( game.settings.lastMovedPiece_color );

			//Update game data structures
			game.GetSideBoard( actingPiece.OwnerId ).RemoveCapturedPiece( actingPiece );
			UpdateBoard( game, actingPiece );
			actingPiece.X = DestinationX;
			actingPiece.Y = DestinationY;
			actingPiece.IsCaptured = false;
		}

		public void UpdateBoard( ShogiGame game, Piece actingPiece ) {
			game.board [DestinationX, DestinationY] = actingPiece;
		}

		//TODO: Maybe I should create a DropPawnAction class, and move this code there. 
		//I don't need to do pawn validation if I'm dropping other pieces. It might make sense
		//It's more Object Oriented, but maybe our if is good enough in this case. 
		public override bool IsMoveValid( ShogiGame game ) {
			var actingPiece = GetActingPiece( );

			bool isValidPieceMovement = actingPiece.GetValidMoves().Any( m => m.x == DestinationX && m.y == DestinationY );
			bool willBeAbleToMove_FromDestination = actingPiece.defaultMovement.GetAvailableMoves( DestinationX, DestinationY ).Any();

			if(actingPiece.PieceType == PieceType.Pawn){
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
