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

		void OnEnable(){
			sideBoard.OnNewPieceAdded.Value += IncreaseText;
			sideBoard.OnNewPieceRemoved.Value += DecreaseText;
			RegisterButtons();

		}

		void OnDisable(){
			sideBoard.OnNewPieceAdded.Value -= IncreaseText;
			sideBoard.OnNewPieceRemoved.Value -= DecreaseText;
			UnregisterButtons();
		}

		void Start() {
			ResetAllTextsToZero();
		}

		private void RegisterButtons() {
			pawnText.GetComponentInParent<Button>( true ).onClick.AddListener( () => OnPieceButtonClicked( PieceType.Pawn ) );
			lancerText.GetComponentInParent<Button>( true ).onClick.AddListener( () => OnPieceButtonClicked( PieceType.Lancer ) );
			knightText.GetComponentInParent<Button>( true ).onClick.AddListener( () => OnPieceButtonClicked( PieceType.Knight ) );
			silverText.GetComponentInParent<Button>( true ).onClick.AddListener( () => OnPieceButtonClicked( PieceType.Silver ) );
			goldText.GetComponentInParent<Button>( true ).onClick.AddListener( () => OnPieceButtonClicked( PieceType.Gold ) );
			rookText.GetComponentInParent<Button>( true ).onClick.AddListener( () => OnPieceButtonClicked( PieceType.Rook ) );
			bishopText.GetComponentInParent<Button>( true ).onClick.AddListener( () => OnPieceButtonClicked( PieceType.Bishop ) );
		}

		private void UnregisterButtons() {
			pawnText.GetComponentInParent<Button>( true ).onClick.RemoveAllListeners();
			lancerText.GetComponentInParent<Button>( true ).onClick.RemoveAllListeners();
			knightText.GetComponentInParent<Button>( true ).onClick.RemoveAllListeners();
			silverText.GetComponentInParent<Button>( true ).onClick.RemoveAllListeners();
			goldText.GetComponentInParent<Button>( true ).onClick.RemoveAllListeners();
			rookText.GetComponentInParent<Button>( true ).onClick.RemoveAllListeners();
			bishopText.GetComponentInParent<Button>( true ).onClick.RemoveAllListeners();
		}

		private void ResetAllTextsToZero(){
			pawnText.text = "x0";
			lancerText.text = "x0";
			knightText.text = "x0";
			silverText.text = "x0";
			goldText.text = "x0";
			rookText.text = "x0";
			bishopText.text = "x0";

			foreach (Transform pieceArt in this.transform.GetChild( 0 )) {
				pieceArt.gameObject.SetActive( false );
			}
		}

		private void OnPieceButtonClicked( PieceType pieceType ) {
			Debug.Log("Ho clickato");
			Piece piece = sideBoard.CapturedPieces.First( p => p.PieceType == pieceType );
			piece.OnPointerClick( new PointerEventData(null) );
		}


		private void IncreaseText( Piece newCapturedPiece) {
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

		private void DecreaseText( Piece newCapturedPiece ) {
			switch (newCapturedPiece.PieceType) {
				case PieceType.Pawn:
					DecreaseNumberLabel( pawnText );
					break;
				case PieceType.Lancer:
					DecreaseNumberLabel( lancerText );
					break;
				case PieceType.Knight:
					DecreaseNumberLabel( knightText );
					break;
				case PieceType.Silver:
					DecreaseNumberLabel( silverText );
					break;
				case PieceType.Gold:
					DecreaseNumberLabel( goldText );
					break;
				case PieceType.Rook:
					DecreaseNumberLabel( rookText );
					break;
				case PieceType.Bishop:
					DecreaseNumberLabel( bishopText );
					break;
			}
		}

		private int GetNumberFromLabel(TMP_Text text){
			string number_raw = text.text.Skip( 1 ).Replace( "\n", "" );
			int number = int.Parse( number_raw );
			return number;
		}

		public void IncreaseNumberLabel(TMP_Text text){
			int number = GetNumberFromLabel( text ) + 1;

			if (number > 0) {
				text.transform.parent.gameObject.SetActive( true );
			}
			text.text = "x"+number.ToString();
		}

		public void DecreaseNumberLabel( TMP_Text text ) {
			int number = GetNumberFromLabel( text ) - 1;

			if (number <= 0) {
				text.transform.parent.gameObject.SetActive( false );
			}
			text.text = "x" + number.ToString();
		}

		//TODO: Prendere Sideboard, leggere tutti i pezzi, aggiornare la UI
		public void RefreshUI(){
			ResetAllTextsToZero();
			foreach(var capturedPiece in sideBoard.CapturedPieces){
				IncreaseText( capturedPiece );
			}
		}
	}

}
