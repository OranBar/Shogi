using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	[Serializable]
	public enum PlayerId{
		Player1 = 1,
		Player2
	}

	public static class PlayerIdEx{
		public static PlayerId GetOtherPlayer(this PlayerId playerId){
			return playerId == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;
		} 
	}

	[Serializable]
	public class HumanPlayer : APlayer
	{
		[SerializeField] private string _playerName;
		public override string PlayerName { get => _playerName; set => _playerName = value; }

		public PlayerId _playerId;
		public override PlayerId PlayerId {
			get { return _playerId; }
			set { _playerId = value; }
		}

		public PlayerId OpponentId => _playerId == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;

		public RefAction<Piece> OnPiece_Selected = new RefAction<Piece>();
		public RefAction<Piece> OnCapturePiece_Selected = new RefAction<Piece>();
		public RefAction<Cell> OnMoveCell_Selected = new RefAction<Cell>();

		private Button undoButton;

		[ReadOnly] public Piece selectedPiece;
		protected IShogiAction currAction;
		[HideInInspector] public ShogiGame shogiGame;
		protected bool actionReady = false;

		protected virtual void Awake(){
			shogiGame = FindObjectOfType<ShogiGame>(true );
			undoButton = FindObjectOfType<UndoButton>(true).gameObject.GetComponent<Button>(true);
		}

		protected virtual void OnDisable() {
			UnregisterAllCallbacks();
		}

		private void UnregisterAllCallbacks() {
			Cell.OnAnyCellClicked -= Select_CellToMove;
			shogiGame.Get_OnPieceClickedEvent( _playerId ).Value -= Select_ActionPiece;
			shogiGame.Get_OnPieceClickedEvent( OpponentId ).Value -= Select_PieceToCapture;
			undoButton.onClick.RemoveListener( RequestUndo );
		}

		void Select_ActionPiece(Piece piece){
			if(selectedPiece != null){
				//We entered this method more then once in the same turn.
				shogiGame.Get_OnPieceClickedEvent( OpponentId ).Value -= Select_PieceToCapture;
				Cell.OnAnyCellClicked -= Select_CellToMove;
			}

			selectedPiece = piece;
			if (selectedPiece.IsCaptured == false) {
				currAction = new MovePieceAction( selectedPiece );
			} else {
				currAction = new DropPieceAction( selectedPiece );
			}
			Debug.Log($"<{PlayerName}> Piece Selected ({piece.X},{piece.Y})", piece.gameObject);
			OnPiece_Selected.Invoke( selectedPiece );

			shogiGame.Get_OnPieceClickedEvent(OpponentId).Value += Select_PieceToCapture;
			Cell.OnAnyCellClicked += Select_CellToMove;
		}

		

		private void Select_CellToMove( Cell cell ) {
			Debug.Log($"<{PlayerName}> Move action to cell ({cell.x},{cell.y})");
			currAction.DestinationX = cell.x;
			currAction.DestinationY = cell.y;

			actionReady = true;
			OnMoveCell_Selected.Invoke(cell );
		}

		private void Select_PieceToCapture( Piece toCapture) {
			Debug.Log($"<{PlayerName}> Capture action: Piece on ({toCapture.X},{toCapture.Y})");
			currAction.DestinationX = toCapture.X;
			currAction.DestinationY = toCapture.Y;

			actionReady = true;
			OnCapturePiece_Selected.Invoke( toCapture );
		}

		public async override UniTask<IShogiAction> RequestAction() {
			undoButton.onClick.AddListener( RequestUndo );
			shogiGame.Get_OnPieceClickedEvent( _playerId ).Value += Select_ActionPiece;

			actionReady = false;
			currAction = null;
			selectedPiece = null;
			while (actionReady == false ) {
				await UniTask.Yield();
			}
			UnregisterAllCallbacks();


			if (currAction is MovePieceAction) {
				MovePieceAction moveAction = (MovePieceAction)currAction;
				if(moveAction.IsMoveValid(shogiGame) && moveAction.CanChooseToPromote_MovedPiece(shogiGame)){
					moveAction.Request_PromotePiece = await GetComponent<IPromotionPromter>().GetPromotionChoice(moveAction);
				}
			}


			return currAction;
		}

		[ContextMenu("Request Undo")]
		public void RequestUndo(){
			currAction = new UndoLastAction();
			actionReady = true;
		}
	}
}
