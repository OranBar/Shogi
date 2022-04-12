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
			if (playerId == PlayerId.Player1) {
				ShogiGame.OnPlayer1_PieceClicked += Select_ActionPiece;
			} else {
				ShogiGame.OnPlayer2_PieceClicked += Select_ActionPiece;
			}
		}

		void Select_ActionPiece(Piece piece){
			selectedPiece = piece;
			Debug.Log("Piece Selected "+piece, piece.gameObject);
		}

		public async Task<IShogiAction> RequestAction() {
			currAction = null;
			while(currAction == null){
				await Task.Yield();
			}
			return currAction;
		}
	}
}
