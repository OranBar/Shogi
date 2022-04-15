using System;
using System.Collections;
using System.Collections.Generic;
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
		public PlayerId playerId;
		public PlayerId OpponentId => playerId == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;

		private Piece selectedPiece;
		private IShogiAction currAction;

		void Start() {
		}

		void Select_ActionPiece(Piece piece){
			selectedPiece = piece;
			Debug.Log($"<{playerName}> Piece Selected ({piece.X},{piece.Y})", piece.gameObject);

			ShogiGame.Get_OnPieceClickedEvent(OpponentId).Value += Select_PieceToCapture;
			// if (playerId == PlayerId.Player1) {
			// 	ShogiGame.OnPlayer2_PieceClicked += Select_PieceToCapture;
			// } else {
			// 	ShogiGame.OnPlayer1_PieceClicked += Select_PieceToCapture;
			// }

			ShogiGame.OnAnyCellClicked += Select_CellToMove;
		}

		private void Select_CellToMove( Cell obj ) {
			Debug.Log($"<{playerName}> Move action to cell ({obj.x},{obj.y})");
			currAction = new MovePieceAction( selectedPiece, obj.x, obj.y );
		}

		private void Select_PieceToCapture( Piece toCapture) {
			Debug.Log($"<{playerName}> Capture action: Piece on ({toCapture.X},{toCapture.Y})");
			currAction = new MovePieceAction( selectedPiece, toCapture.X, toCapture.Y );
			ShogiGame.Get_OnPieceClickedEvent( OpponentId ).Value -= Select_PieceToCapture;
		}

		public async UniTask<IShogiAction> RequestAction() {
			ShogiGame.Get_OnPieceClickedEvent(playerId).Value += Select_ActionPiece;
			// if (playerId == PlayerId.Player1) {
			// 	ShogiGame.OnPlayer1_PieceClicked += Select_ActionPiece;
			// } else {
			// 	ShogiGame.OnPlayer2_PieceClicked += Select_ActionPiece;
			// }

			currAction = null;
			selectedPiece = null;
			while(currAction == null){
				await UniTask.Yield();
			}

			ShogiGame.Get_OnPieceClickedEvent( playerId ).Value -= Select_ActionPiece;
			ShogiGame.Get_OnPieceClickedEvent( OpponentId ).Value -= Select_PieceToCapture;
			// if (playerId == PlayerId.Player1) {
			// 	ShogiGame.OnPlayer1_PieceClicked -= Select_ActionPiece;
			// } else {
			// 	ShogiGame.OnPlayer2_PieceClicked -= Select_ActionPiece;
			// }
			return currAction;
		}
	}
}
