using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	public enum PlayerId{
		Player1 = 1,
		Player2
	}

	[Serializable]
	public class HumanPlayer : MonoBehaviour, IPlayer
	{
		private GameSettings settings;
		[SerializeField] private string _playerName;
		public string PlayerName { get => _playerName; set => _playerName = value; }

		[SerializeField] private PlayerId playerId;
		public PlayerId PlayerId => playerId;

		public PlayerId OpponentId => playerId == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;

		public Button undoButton;

		private Piece selectedPiece;
		private IShogiAction currAction;
		private ShogiGame shogiGame;
		private bool actionReady = false;

		void Awake(){
			shogiGame = FindObjectOfType<ShogiGame>();
			settings = FindObjectOfType<GameSettings>();
		}

		private void OnDisable() {
			UnregisterAllCallbacks();
		}

		private void UnregisterAllCallbacks() {
			Cell.OnAnyCellClicked -= Select_CellToMove;
			shogiGame.Get_OnPieceClickedEvent( playerId ).Value -= Select_ActionPiece;
			shogiGame.Get_OnPieceClickedEvent( OpponentId ).Value -= Select_PieceToCapture;
			undoButton.onClick.RemoveListener( RequestUndo );
		}

		void Select_ActionPiece(Piece piece){
			selectedPiece?.GetComponent<IPieceHighlight>().DisableHighlight();
			selectedPiece = piece;
			if (selectedPiece.IsCaptured == false) {
				currAction = new MovePieceAction( selectedPiece );
			} else {
				currAction = new DropPieceAction( selectedPiece );
			}

			piece.LogAvailableMoves();
			Debug.Log($"<{PlayerName}> Piece Selected ({piece.X},{piece.Y})", piece.gameObject);
			piece.GetComponent<IPieceHighlight>().EnableHighlight( settings.selectedPiece_color );

			shogiGame.Get_OnPieceClickedEvent(OpponentId).Value += Select_PieceToCapture;
			Cell.OnAnyCellClicked += Select_CellToMove;
		}

		private void Select_CellToMove( Cell obj ) {
			Debug.Log($"<{PlayerName}> Move action to cell ({obj.x},{obj.y})");
			currAction.DestinationX = obj.x;
			currAction.DestinationY = obj.y;

			actionReady = true;
			selectedPiece.GetComponent<IPieceHighlight>().DisableHighlight( );

			Cell.OnAnyCellClicked -= Select_CellToMove;
		}

		private void Select_PieceToCapture( Piece toCapture) {
			Debug.Log($"<{PlayerName}> Capture action: Piece on ({toCapture.X},{toCapture.Y})");
			currAction.DestinationX = toCapture.X;
			currAction.DestinationY = toCapture.Y;

			actionReady = true;
			selectedPiece?.GetComponent<IPieceHighlight>().DisableHighlight( );

			shogiGame.Get_OnPieceClickedEvent( OpponentId ).Value -= Select_PieceToCapture;
			Cell.OnAnyCellClicked -= Select_CellToMove;
		}

		public async UniTask<IShogiAction> RequestAction() {
			undoButton.onClick.AddListener( RequestUndo );
			shogiGame.Get_OnPieceClickedEvent( playerId ).Value += Select_ActionPiece;

			actionReady = false;
			currAction = null;
			selectedPiece = null;
			while (actionReady == false ) {
				await UniTask.Yield();
			}


			if (currAction is MovePieceAction) {
				MovePieceAction moveAction = (MovePieceAction)currAction;
				if(moveAction.IsMoveValid(shogiGame) && moveAction.CanChooseToPromote_MovedPiece(shogiGame)){
					moveAction.Request_PromotePiece = await GetComponent<IPromotionPromter>().GetPromotionChoice();
				}
			}

			UnregisterAllCallbacks();

			return currAction;
		}

		[ContextMenu("Request Undo")]
		public void RequestUndo(){
			currAction = new UndoLastAction();
			actionReady = true;
		}
	}
}
