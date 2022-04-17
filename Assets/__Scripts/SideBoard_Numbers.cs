using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Shogi
{
	[Serializable]
	public class SideBoard_Numbers : MonoBehaviour
	{

		public SideBoard sideBoard;
		public TMP_Text pawnText;
		public TMP_Text lancerText;
		public TMP_Text knightText;
		public TMP_Text silverText;
		public TMP_Text goldText;
		public TMP_Text rookText;
		public TMP_Text bishopText;

		void Start(){
			sideBoard = GetComponent<SideBoard>();
			sideBoard.OnNewPieceCaptured.Value += UpdateText;
		}

		private void UpdateText( Piece newCapturedPiece) {
			switch (newCapturedPiece.PieceType) {
				case PieceType.Pawn:
					var pawns = int.Parse( pawnText.text ) + 1;
					pawnText.text = pawns.ToString();
					break;
				case PieceType.Lancer:
					var lancers = int.Parse( lancerText.text ) + 1;
					lancerText.text = lancers.ToString();
					break;
				case PieceType.Knight:
					var knights = int.Parse( knightText.text ) + 1;
					knightText.text = knights.ToString();
					break;
				case PieceType.Silver:
					var silvers = int.Parse( silverText.text ) + 1;
					silverText.text = silvers.ToString();
					break;
				case PieceType.Gold:
					var golds = int.Parse( goldText.text ) + 1;
					goldText.text = golds.ToString();
					break;
				case PieceType.Rook:
					var rooks = int.Parse( rookText.text ) + 1;
					rookText.text = rooks.ToString();
					break;
				case PieceType.Bishop:
					var bishops = int.Parse( bishopText.text ) + 1;
					bishopText.text = bishops.ToString();
					break;
			}
		}
	}

}
