using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	[Serializable]
	public class MovePieceAction : AShogiAction
	{
		public bool Request_PromotePiece { get => _promotePiece; set => _promotePiece = value; }
		private bool _promotePiece = false;

		public MovePieceAction() : base(){
		}

		public MovePieceAction( Piece piece ) : base( piece ) {
		}

		public MovePieceAction( Piece piece, int destinationX, int destinationY ) : base( piece, destinationX, destinationY ) {

		}

		public override string ToString() {
			return "Move "+base.ToString();
		}

		public bool IsCapturingMove( ShogiGame game ){
			var capturedPiece = game.board [DestinationX, DestinationY];
			return capturedPiece != null && capturedPiece.OwnerId != ActingPiece.OwnerId;
		}

		public override async UniTask ExecuteAction( ShogiGame game ) {
			base.ExecuteAction( game ).Forget();

			UnityEngine.Debug.Log( $"Moving piece {ActingPiece} to cell ({DestinationX},{DestinationY})" );

			var capturedPiece = game.board [DestinationX, DestinationY];

			await ActingPiece.pieceFx.DoMoveAnimation( this );
			if (IsCapturingMove( game )) {
				//A piece was killed. Such cruelty. 
				await capturedPiece.CapturePiece();
			}

			UpdateBoard( game.board );
			UpdatePiece();
			HandlePromotion( game, ActingPiece );


			#region Local Methods -----------------------------
				void UpdateBoard( ABoard board ) {
					board [StartX, StartY] = null;
					board [DestinationX, DestinationY] = ActingPiece;
				}

				void UpdatePiece() {
					ActingPiece.X = DestinationX;
					ActingPiece.Y = DestinationY;
				}
			#endregion -----------------------------------------
		}


		public override bool IsMoveValid( ShogiGame game ) {
			var actingPiece = ActingPiece;
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
			var actingPiece = ActingPiece;

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
