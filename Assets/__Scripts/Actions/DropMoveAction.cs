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

		public override async UniTask ExecuteAction( ShogiGame game ) {
			base.ExecuteAction(game).Forget();
			var actingPiece = GetActingPiece( game );
			UnityEngine.Debug.Log($"Dropping piece {actingPiece} on ({DestinationX},{DestinationY})");

			game.GetSideBoard( actingPiece.OwnerId ).RemoveCapturedPiece( actingPiece );
			await actingPiece.movementFx.DoMoveAnimation( DestinationX, DestinationY );

			//Update game data structures
			UpdateBoard( game, actingPiece );
			actingPiece.X = DestinationX;
			actingPiece.Y = DestinationY;
			actingPiece.IsCaptured = false;
		}

		public void UpdateBoard( ShogiGame game, Piece actingPiece ) {
			game.board [DestinationX, DestinationY] = actingPiece;
		}

		public override bool IsMoveValid( ShogiGame game ) {
			var actingPiece = GetActingPiece( game );

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
					bool isUnpromotedPawn = isPawn && game.board [StartX, y].IsPromoted == false;

					if (isUnpromotedPawn) {
						return true;
					}
				}
				return false;
			}
		}


	}
}
