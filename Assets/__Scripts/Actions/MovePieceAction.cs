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
		[NonSerialized] private Piece capturedPiece;

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
			return capturedPiece != null && capturedPiece.OwnerId != ActingPiece.OwnerId;
		}

		public override void ExecuteAction( ShogiGame game ) {
			base.ExecuteAction( game );
			UnityEngine.Debug.Log( $"Moving piece {ActingPiece} to cell ({DestinationX},{DestinationY})" );

			capturedPiece = game.board [DestinationX, DestinationY];

			UpdateBoard( game.board );
			UpdatePiece();
			HandlePromotion( game, ActingPiece );

			bool isCapturingMove = capturedPiece != null;
			if (isCapturingMove) {
				//A piece was killed. Such cruelty. 
				capturedPiece.CapturePiece();
				var sideboard = GameObject.FindObjectsOfType<SideBoard>().First( s => s.ownerId == capturedPiece.OwnerId );
				sideboard.AddCapturedPiece( capturedPiece );
			}

			
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

		public override async UniTask ExecuteAction_FX( ) {
			DoHighlightLastMovedPiece();


			await ActingPiece.pieceFx.DoMoveAnimation( this );
			bool isCapturingMove = capturedPiece != null;
			if (isCapturingMove) {
				await capturedPiece.pieceFx.DoPieceDeathAnimation();
				
				var sideboard = GameObject.FindObjectsOfType<SideBoard>().First( s => s.ownerId == capturedPiece.OwnerId );
				await sideboard.GetComponent<ISideboardFX>().PieceAddedToSideboard_FX( capturedPiece );
			}


			void DoHighlightLastMovedPiece() {
				IHighlightFx pieceHighlight = ActingPiece.GetComponent<IHighlightFx>();
				pieceHighlight.EnableHighlight( GameObject.FindObjectOfType<ShogiGameSettings>().GetLastMovedPiece_Color( PlayerId ) );
			}
		}

		public override bool IsMoveValid( ShogiGame game ) {
			bool isValidPieceMovement = ActingPiece.GetValidMoves().Any( m => m.x == DestinationX && m.y == DestinationY );

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
