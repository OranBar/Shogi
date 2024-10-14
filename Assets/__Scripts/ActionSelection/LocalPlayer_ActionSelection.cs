using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	// public class LocalPlayer_ActionSelection : MonoBehaviour, IActionSelectionStrategy
	// {
	// 	private AShogiAction selectedAction;
	// 	[Auto] private PhotonView photonView;
	// 	private bool actionReady;
	// 	private AShogiAction currAction;
	// 	[Auto] HumanPlayer player;

	// 	public async override UniTask<AShogiAction> RequestAction() {
	// 		undoButton.onClick.AddListener( RequestUndo );
	// 		shogiGame.Get_OnPieceClickedEvent( _playerId ).Value += Select_ActionPiece;

	// 		actionReady = false;
	// 		currAction = null;
	// 		player.selectedPiece = null;
	// 		while (actionReady == false) {
	// 			await UniTask.Yield();
	// 		}
	// 		UnregisterAllCallbacks();


	// 		if (currAction is MovePieceAction) {
	// 			MovePieceAction moveAction = (MovePieceAction)currAction;
	// 			if (moveAction.IsMoveValid( shogiGame ) && moveAction.CanChooseToPromote_MovedPiece( shogiGame )) {
	// 				moveAction.Request_PromotePiece = await GetComponent<IPromotionPromter>().GetPromotionChoice( moveAction );
	// 			}
	// 		}


	// 		return currAction;
	// 	}

	// }


	public class LocalPlayer_ActionSelection : AActionSelectionStrategy
	{
		protected Button undoButton;

		[Auto] HumanPlayer player;
		[ReadOnly] public Piece selectedPiece;
		protected AShogiAction currAction;
		[HideInInspector] public ShogiGame shogiGame;
		protected bool actionReady = false;

		protected virtual void Awake() {
			shogiGame = FindObjectOfType<ShogiGame>( true );
			undoButton = FindObjectOfType<UndoButton>( true ).gameObject.GetComponent<Button>( true );
		}

		protected virtual void OnDisable() {
			UnregisterAllCallbacks();
		}

		private void UnregisterAllCallbacks() {
			Cell.OnAnyCellClicked -= Select_CellToMove;
			shogiGame.Get_OnPieceClickedEvent( player._playerId ).Value -= Select_ActionPiece;
			shogiGame.Get_OnPieceClickedEvent( player.OpponentId ).Value -= Select_PieceToCapture;
			undoButton.onClick.RemoveListener( RequestUndo );
		}

		void Select_ActionPiece( Piece piece ) {
			if (selectedPiece != null) {
				//We entered this method more then once in the same turn.
				shogiGame.Get_OnPieceClickedEvent( player.OpponentId ).Value -= Select_PieceToCapture;
				Cell.OnAnyCellClicked -= Select_CellToMove;
			}

			selectedPiece = piece;
			if (selectedPiece.IsCaptured == false) {
				currAction = new MovePieceAction( selectedPiece );
			} else {
				currAction = new DropPieceAction( selectedPiece );
			}
			Logger.Log( $"[Player] <{player.PlayerName}> Piece Selected ({piece.X},{piece.Y})", piece.gameObject );
			//I don't like invoking events from external classes
			player.OnPiece_Selected.Invoke( selectedPiece );

			shogiGame.Get_OnPieceClickedEvent( player.OpponentId ).Value += Select_PieceToCapture;
			Cell.OnAnyCellClicked += Select_CellToMove;
		}



		private void Select_CellToMove( Cell cell ) {
			Logger.Log( $"[Player] <{player.PlayerName}> Move action to cell ({cell.x},{cell.y})" );
			currAction.DestinationX = cell.x;
			currAction.DestinationY = cell.y;

			actionReady = true;
			//I don't like invoking events from external classes
			player.OnMoveCell_Selected.Invoke( cell );
		}

		private void Select_PieceToCapture( Piece toCapture ) {
			Logger.Log( $"[Player] <{player.PlayerName}> Capture action: Piece on ({toCapture.X},{toCapture.Y})" );
			currAction.DestinationX = toCapture.X;
			currAction.DestinationY = toCapture.Y;

			actionReady = true;
			//I don't like invoking events from external classes
			player.OnCapturePiece_Selected.Invoke( toCapture );
		}

		public async override UniTask<AShogiAction> RequestAction() {
			undoButton.onClick.AddListener( RequestUndo );
			shogiGame.Get_OnPieceClickedEvent( player._playerId ).Value += Select_ActionPiece;

			actionReady = false;
			currAction = null;
			selectedPiece = null;
			while (actionReady == false) {
				await UniTask.Yield();
			}
			UnregisterAllCallbacks();


			if (currAction is MovePieceAction) {
				MovePieceAction moveAction = (MovePieceAction)currAction;
				if (moveAction.IsMoveValid( shogiGame ) && moveAction.CanChooseToPromote_MovedPiece( shogiGame )) {
					moveAction.Request_PromotePiece = await GetComponent<IPromotionPromter>().GetPromotionChoice( moveAction );
				}
			}

			if (currAction.IsMoveValid( shogiGame ) == false){
				//I don't like invoking events from external classes
				player.OnInvalidMove_Selected.Invoke(currAction);
				return await RequestAction();
			}


			return currAction;
		}

		[ContextMenu( "Request Undo" )]
		public void RequestUndo() {
			currAction = new UndoLastAction();
			actionReady = true;
		}

	}
}