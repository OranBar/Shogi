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

		public TMP_Text pawnText;
		public TMP_Text lancerText;
		public TMP_Text knightText;
		public TMP_Text silverText;
		public TMP_Text goldText;
		public TMP_Text rookText;
		public TMP_Text bishopText;
		[Auto] private SideBoard sideBoard;

		void Start(){
			sideBoard.OnNewPieceCaptured.Value += UpdateText;
		}

		private void UpdateText( Piece newCapturedPiece) {
			switch (newCapturedPiece.PieceType) {
				case PieceType.Pawn:
					IncreaseNumberLabel( pawnText );
					break;
				case PieceType.Lancer:
					IncreaseNumberLabel( lancerText );
					break;
				case PieceType.Knight:
					IncreaseNumberLabel( knightText );
					break;
				case PieceType.Silver:
					IncreaseNumberLabel( silverText );
					break;
				case PieceType.Gold:
					IncreaseNumberLabel( goldText );
					break;
				case PieceType.Rook:
					IncreaseNumberLabel( rookText );
					break;
				case PieceType.Bishop:
					IncreaseNumberLabel( bishopText );
					break;
			}
		}

		public void IncreaseNumberLabel(TMP_Text text){
			string number_raw = text.text.Skip( 1 ).Replace( "\n", "" );
			int number = int.Parse( number_raw ) + 1;
			text.text = "x"+number.ToString();
		}
	}

}
