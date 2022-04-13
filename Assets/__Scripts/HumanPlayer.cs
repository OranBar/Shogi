using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

		private Piece selectedPiece;
		private IShogiAction currAction;

		void Start() {
		}

		void Select_ActionPiece(Piece piece){
			selectedPiece = piece;
			Debug.Log("Piece Selected "+piece, piece.gameObject);

			if (playerId == PlayerId.Player1) {
				ShogiGame.OnPlayer2_PieceClicked += Select_PieceToCapture;
			} else {
				ShogiGame.OnPlayer1_PieceClicked += Select_PieceToCapture;
			}

			ShogiGame.OnAnyCellClicked += Select_CellToMove;
		}

		private void Select_CellToMove( Cell obj ) {
			currAction = new MovePieceAction( selectedPiece, obj.x, obj.y );
		}

		private void Select_PieceToCapture( Piece toCapture) {
			currAction = new MovePieceAction( selectedPiece, toCapture.X, toCapture.Y );
		}

		public async Task<IShogiAction> RequestAction() {
			if (playerId == PlayerId.Player1) {
				ShogiGame.OnPlayer1_PieceClicked += Select_ActionPiece;
			} else {
				ShogiGame.OnPlayer2_PieceClicked += Select_ActionPiece;
			}

			currAction = null;
			while(currAction == null){
				await Task.Yield();
			}

			if (playerId == PlayerId.Player1) {
				ShogiGame.OnPlayer1_PieceClicked -= Select_ActionPiece;
			} else {
				ShogiGame.OnPlayer2_PieceClicked -= Select_ActionPiece;
			}
			return currAction;
		}
	}
}
