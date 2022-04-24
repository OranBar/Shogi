using System;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	[Serializable]
	public class MovePieceAction : AShogiAction
	{
		public bool Request_PromotePiece { get => _promotePiece; set => _promotePiece = value; }
		private bool _promotePiece = false;
		private Piece capturedPiece;

		public MovePieceAction( Piece piece ) : base( piece ) {
		}

		public MovePieceAction( Piece piece, int destinationX, int destinationY ) : base( piece, destinationX, destinationY ) {
		}

		public override string ToString() {
			return "Move "+base.ToString();
		}

		public override async UniTask ExecuteAction( ShogiGame game ) {
			base.ExecuteAction( game ).Forget();
			Board board = game.board;
			var actingPiece = board[StartX, StartY];
			UnityEngine.Debug.Log( $"Moving piece {actingPiece} on ({DestinationX},{DestinationY})" );


			capturedPiece = board[DestinationX, DestinationY];
			bool wasCapturingMove = capturedPiece != null && capturedPiece.owner != actingPiece.owner;

			if (wasCapturingMove) {
				//A piece was killed. Such cruelty. 
				capturedPiece.CapturePiece();
			}
			await actingPiece.PieceMovementAnimation( DestinationX, DestinationY );

			//Update game data structures
			UpdateBoard( board );
			actingPiece.X = DestinationX;
			actingPiece.Y = DestinationY;

			HandlePromotion(game);
		}

		public void UpdateBoard( Board board ) {
			Piece piece = board [StartX, StartY];
			board [piece.X, piece.Y] = null;
			board [DestinationX, DestinationY] = piece;
		}

		public override bool IsMoveValid( ShogiGame game ) {
			var actingPiece = game.board[StartX, StartY];
			bool isValidPieceMovement = actingPiece.GetValidMoves().Any( m => m.x == DestinationX && m.y == DestinationY );

			return isValidPieceMovement;
		}

		private void HandlePromotion( ShogiGame game ) {
			if(ActingPiece.IsPromoted){
				return;
			}
			
			if (MustPromoteAfterMove( game )) {
				ActingPiece.Promote();
			}

			if (IsPromotionRequirementSatisfied( game ) && Request_PromotePiece) {
				ActingPiece.Promote();
			}
		}

		public bool CanChooseToPromote( ShogiGame game ) {
			return IsPromotionRequirementSatisfied(game) && MustPromoteAfterMove( game ) == false;
		}

		public bool MustPromoteAfterMove( ShogiGame game ) {
			if (IsPromotionRequirementSatisfied(game)) {
				bool canMoveAgain = ActingPiece.DefaultMovement.GetAvailableMoves( DestinationX, DestinationY ).Any();
				if (canMoveAgain == false) {
					return true;
				}
			}

			return false;
		}

		private bool IsPromotionRequirementSatisfied(ShogiGame game){
			bool canPromote = ActingPiece.HasPromotion();
			if (canPromote == false) {
				return false; 
			}
			if (ActingPiece.IsPromoted) {
				return false;
			}

			return game.board.IsPromotionZone( StartX, StartY, ActingPiece.OwnerId ) ||
				game.board.IsPromotionZone( DestinationX, DestinationY, ActingPiece.OwnerId );
		}
	}
}
