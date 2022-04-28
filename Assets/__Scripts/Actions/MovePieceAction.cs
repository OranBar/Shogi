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

		public MovePieceAction( Piece piece ) : base( piece ) {
		}

		public MovePieceAction( Piece piece, int destinationX, int destinationY ) : base( piece, destinationX, destinationY ) {
		}

		public override string ToString() {
			return "Move "+base.ToString();
		}

		public override async UniTask ExecuteAction( ShogiGame game ) {
			base.ExecuteAction( game ).Forget();
			var actingPiece = GetActingPiece( game );

			UnityEngine.Debug.Log( $"Moving piece {actingPiece} on ({DestinationX},{DestinationY})" );

			var capturedPiece = game.board[DestinationX, DestinationY];
			bool wasCapturingMove = capturedPiece != null && capturedPiece.owner != actingPiece.owner;

			if (wasCapturingMove) {
				//A piece was killed. Such cruelty. 
				capturedPiece.CapturePiece();
			}
			await actingPiece.PieceMovementAnimation( DestinationX, DestinationY );

			//Update game data structures
			UpdateBoard( game.board, actingPiece );
			actingPiece.X = DestinationX;
			actingPiece.Y = DestinationY;

			HandlePromotion( game, actingPiece );
		}

		public void UpdateBoard( Board board, Piece actingPiece ) {
			board [actingPiece.X, actingPiece.Y] = null;
			board [DestinationX, DestinationY] = actingPiece;
		}

		public override bool IsMoveValid( ShogiGame game ) {
			var actingPiece = GetActingPiece( game );
			bool isValidPieceMovement = actingPiece.GetValidMoves().Any( m => m.x == DestinationX && m.y == DestinationY );

			return isValidPieceMovement;
		}

		private void HandlePromotion( ShogiGame game, Piece actingPiece ) {
			if(actingPiece.IsPromoted){
				return;
			}
			
			if (MustPromoteAfterMove( game, actingPiece )) {
				actingPiece.IsPromoted = true;
			}

			if (IsPromotionRequirementSatisfied( game, actingPiece ) && Request_PromotePiece) {
				actingPiece.IsPromoted = true;
			}
		}

		public bool CanChooseToPromote_MovedPiece( ShogiGame game ) {
			var actingPiece = GetActingPiece( game );

			return IsPromotionRequirementSatisfied( game, actingPiece ) && (MustPromoteAfterMove( game, actingPiece ) == false);
		}

		public bool MustPromoteAfterMove( ShogiGame game, Piece actingPiece ) {
			if (IsPromotionRequirementSatisfied(game, actingPiece )) {
				bool canMoveAgain = actingPiece.defaultMovement.GetAvailableMoves( DestinationX, DestinationY ).Any();
				if (canMoveAgain == false) {
					return true;
				}
			}

			return false;
		}

		private bool IsPromotionRequirementSatisfied(ShogiGame game, Piece actingPiece ){
			bool canPromote = actingPiece.HasPromotion();
			if (canPromote == false) {
				return false; 
			}
			if (actingPiece.IsPromoted) {
				return false;
			}

			return game.board.IsPromotionZone( StartX, StartY, actingPiece.OwnerId ) ||
				game.board.IsPromotionZone( DestinationX, DestinationY, actingPiece.OwnerId );
		}
	}
}
