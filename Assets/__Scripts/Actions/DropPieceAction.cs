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

		public override string ToString( ActionStringFormat format ) {
			if (format == ActionStringFormat.Minimized) {
				return $"Drop => ({DestinationX}, {DestinationY})";
			} else if (format == ActionStringFormat.Full) {
				return ToString();
			}

			throw new NotImplementedException();
		}


		public override void ExecuteAction( ShogiGame game ) {
			base.ExecuteAction( game );
			Logger.Log( $"[Action] Dropping piece {ActingPiece} on ({DestinationX},{DestinationY})" );

			//TODO: replace with animation
			// game.board.PlacePieceOnCell_Immediate( DestinationX, DestinationY, ActingPiece );
			// await ActingPiece.pieceFx.DoDropAnimation( this );
			
			game.GetSideBoard( ActingPiece.OwnerId ).RemoveCapturedPiece( ActingPiece );
			UpdateBoard( game );
			UpdatePiece();


			#region Local Methods -----------------------------
				void UpdateBoard( ShogiGame game ) {
					game.board [DestinationX, DestinationY] = ActingPiece;
				}

				void UpdatePiece() {
					ActingPiece.X = DestinationX;
					ActingPiece.Y = DestinationY;
					ActingPiece.IsCaptured = false;
				}
			#endregion -----------------------------------------
		}

		public override async UniTask ExecuteAction_FX() {
			await ActingPiece.pieceFx.DoDropAnimation( this );
		}


		//TODO: Maybe I should create a DropPawnAction class, and move this code there. 
		//I don't need to do pawn validation if I'm dropping other pieces. It might make sense
		//It's more Object Oriented, but maybe our if is good enough in this case. 
		public override bool IsMoveValid( ShogiGame game ) {

			bool isValidPieceMovement = ActingPiece.GetValidMoves().Any( m => m.x == DestinationX && m.y == DestinationY );
			bool willBeAbleToMove_FromDestination = ActingPiece.defaultMovement.GetAvailableMoves( DestinationX, DestinationY ).Any();

			// if(ActingPiece.PieceType == PieceType.Pawn){
			// 	if(AnyUnpromotedPawns_OnColumn()){
			// 		isValidPieceMovement = false;
			// 	}
			// }
			return isValidPieceMovement && willBeAbleToMove_FromDestination;

		}

	}
}
