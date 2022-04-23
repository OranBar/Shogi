using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public enum PlayerId{
		Player1 = 1,
		Player2
	}

	[Serializable]
	public class HumanPlayer : MonoBehaviour, IPlayer
	{
		public string playerName;
		[SerializeField] private PlayerId playerId;
		public PlayerId PlayerId => playerId;

		public PlayerId OpponentId => playerId == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;

		private Piece selectedPiece;
		private IShogiAction currAction;
		private ShogiGame shogiGame;
		private bool actionReady = false;
		private bool cancelActionRequest;

		void Awake(){
			shogiGame = FindObjectOfType<ShogiGame>();
		}

		private void OnEnable(){

		}

		private void OnDisable() {
			ShogiGame.OnAnyCellClicked -= Select_CellToMove;
			ShogiGame.Get_OnPieceClickedEvent( playerId ).Value -= Select_ActionPiece;
			ShogiGame.Get_OnPieceClickedEvent( OpponentId ).Value -= Select_PieceToCapture;
			cancelActionRequest = true;
		}


		void Select_ActionPiece(Piece piece){
			selectedPiece = piece;
			if (selectedPiece.IsCaptured == false) {
				currAction = new MovePieceAction( selectedPiece );
			} else {
				currAction = new DropPieceAction( selectedPiece );
			}

			piece.LogAvailableMoves();
			Debug.Log($"<{playerName}> Piece Selected ({piece.X},{piece.Y})", piece.gameObject);

			ShogiGame.Get_OnPieceClickedEvent(OpponentId).Value += Select_PieceToCapture;
			ShogiGame.OnAnyCellClicked += Select_CellToMove;
		}

		private void Select_CellToMove( Cell obj ) {
			Debug.Log($"<{playerName}> Move action to cell ({obj.x},{obj.y})");
			currAction.DestinationX = obj.x;
			currAction.DestinationY = obj.y;

			actionReady = true;

			ShogiGame.OnAnyCellClicked -= Select_CellToMove;
		}

		private void Select_PieceToCapture( Piece toCapture) {
			Debug.Log($"<{playerName}> Capture action: Piece on ({toCapture.X},{toCapture.Y})");
			currAction.DestinationX = toCapture.X;
			currAction.DestinationY = toCapture.Y;

			actionReady = true;

			ShogiGame.Get_OnPieceClickedEvent( OpponentId ).Value -= Select_PieceToCapture;
			ShogiGame.OnAnyCellClicked -= Select_CellToMove;
		}

		public async UniTask<IShogiAction> RequestAction() {
			ShogiGame.Get_OnPieceClickedEvent( playerId ).Value += Select_ActionPiece;

			actionReady = false;
			currAction = null;
			selectedPiece = null;
			cancelActionRequest = false;
			while (actionReady == false || cancelActionRequest) {
				await UniTask.Yield();
			}

			if (currAction is MovePieceAction) {
				await HandlePromotion();
			}

			ShogiGame.OnAnyCellClicked -= Select_CellToMove;
			ShogiGame.Get_OnPieceClickedEvent( playerId ).Value -= Select_ActionPiece;
			ShogiGame.Get_OnPieceClickedEvent( OpponentId ).Value -= Select_PieceToCapture;

			return currAction;
		}

		private async UniTask HandlePromotion() {
			Piece actingPiece = currAction.ActingPiece;

			MovePieceAction currMoveAction = currAction as MovePieceAction;
			bool canPromote = currMoveAction != null && actingPiece.HasPromotion();
			bool promotionRequirementSatisfied = (
				shogiGame.board.IsPromotionZone( currAction.StartX, currAction.StartY, PlayerId ) ||
				shogiGame.board.IsPromotionZone( currAction.DestinationX, currAction.DestinationY, PlayerId )
			);

			if (canPromote && promotionRequirementSatisfied) {
				bool canMoveAgain = actingPiece.MovementStrategy.GetAvailableMoves( currAction.DestinationX, currAction.DestinationY ).Any();
				if (canMoveAgain == false) {
					//Force promotion
					currMoveAction.PromotePiece = true;
				} else {
					//Display Ui to decide if promotion needs to be done.
					//set actionReady after ui click
					currMoveAction.PromotePiece = await GetComponent<IPromotionPromter>().GetPromotionChoice();
				}
			}
		}
	}
}
