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

		public override async UniTask EnableLastMoveFX(GameSettings settings){
			var startCell = Cell.GetCell( StartX, StartY );
			Color highlightColor = settings.GetLastMovedPiece_Color(playerId);

			await ActingPiece.GetComponent<IHighlightFx>().EnableHighlight( highlightColor );
			await startCell.GetComponent<IHighlightFx>().EnableHighlight( highlightColor.SetAlpha( 0.5f ) );
		}

		public override void DisableLastMoveFX(){
			//I'm surprised this line works, since we have moved the piece to a different spot, and it's index is now different
			//We probably cached the correct one in time. But this one was a risky one, I don't realy like
			// var actingPiece = GetActingPiece();
			var startCell = Cell.GetCell( StartX, StartY );

			ActingPiece.GetComponent<IHighlightFx>().DisableHighlight();
			startCell.GetComponent<IHighlightFx>().DisableHighlight();
		}

		public bool IsCapturingMove( ShogiGame game ){
			var capturedPiece = game.board [DestinationX, DestinationY];
			return capturedPiece != null && capturedPiece.owner != ActingPiece.owner;
		}

		public override async UniTask ExecuteAction( ShogiGame game ) {
			base.ExecuteAction( game ).Forget();

			UnityEngine.Debug.Log( $"Moving piece {ActingPiece} to cell ({DestinationX},{DestinationY})" );
			
			EnableLastMoveFX( game.settings );

			// var startCell = Cell.GetCell( StartX, StartY );
			var capturedPiece = game.board[DestinationX, DestinationY];

			await ActingPiece.Move( DestinationX, DestinationY );
			if (IsCapturingMove(game)) {
				//A piece was killed. Such cruelty. 
				await capturedPiece.CapturePiece();
			}


			UpdateBoard( game.board );
			
			HandlePromotion( game, ActingPiece );
		}

		public void UpdateBoard( Board board ) {
			board [StartX, StartY] = null;
			board [DestinationX, DestinationY] = ActingPiece;
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
