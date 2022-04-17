using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
			foreach(Transform pieceArt in this.transform.GetChild(0)){
				pieceArt.gameObject.SetActive( false );
			}

			pawnText.GetComponentInParent<Button>(true).onClick.AddListener(() => OnPieceButtonClicked(PieceType.Pawn));
		}

		private void OnPieceButtonClicked( PieceType pieceType ) {
			Debug.Log("Ho clickato");
			Piece piece = sideBoard.CapturedPieces.First( p => p.PieceType == pieceType );
			piece.OnPointerClick( new PointerEventData(null) );
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

			if (number > 0) {
				text.transform.parent.gameObject.SetActive( true );
			}
			text.text = "x"+number.ToString();
		}

		public void DecreaseNumberLabel( TMP_Text text ) {
			string number_raw = text.text.Skip( 1 ).Replace( "\n", "" );
			int number = int.Parse( number_raw ) - 1;

			if(number <= 0){
				number = 0;
				text.transform.parent.gameObject.SetActive( false );
			}
			text.text = "x" + number.ToString();
		}
	}

}
