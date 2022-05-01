using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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

		public TMP_Text GetText(PieceType pieceType){
			switch (pieceType) {
				case PieceType.Pawn:
					return pawnText;
				case PieceType.Lancer:
					return lancerText;
				case PieceType.Knight:
					return knightText;
				case PieceType.Silver:
					return silverText;
				case PieceType.Gold:
					return goldText;
				case PieceType.Rook:
					return rookText;
				case PieceType.Bishop:
					return bishopText;
				default:
					throw new Exception( "Not supported PieceType" );
			}
		}

		[Auto] private SideBoard sideBoard;

		void OnEnable(){
			sideBoard.OnNewPieceAdded += IncreaseText;
			sideBoard.OnNewPieceRemoved += DecreaseText;
			sideBoard.OnCleared += ResetAllTextsToZero;

			sideBoard.OnNewPieceAdded += PieceAddedToSideboardFx;
			RegisterButtons();
		}

		private void PieceAddedToSideboardFx( Piece obj ) {
			Debug.Log("Sideboard piece FX");
			Transform sideboardPiece = GetText( obj.PieceType ).transform.parent;

			sideboardPiece.transform.localScale = Vector3.one * 2;
			sideboardPiece.DOScale(Vector3.one, 1f).SetEase(Ease.OutCubic);
		}

		void OnDisable(){
			sideBoard.OnNewPieceAdded.Value -= IncreaseText;
			sideBoard.OnNewPieceRemoved.Value -= DecreaseText;
			sideBoard.OnCleared.Value -= ResetAllTextsToZero;
			UnregisterButtons();
		}

		void Start() {
			ResetAllTextsToZero();
		}

		private void OnPieceButtonClicked( PieceType pieceType ) {
			Debug.Log( "Ho clickato" );
			Piece piece = sideBoard.CapturedPieces.First( p => p.PieceType == pieceType );
			piece.OnPointerClick( new PointerEventData( null ) );
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

		private void IncreaseText( Piece newCapturedPiece) {
			TMP_Text targetText = GetText( newCapturedPiece.PieceType );
			IncreaseNumberLabel( targetText );
		}

		private void DecreaseText( Piece newCapturedPiece ) {
			TMP_Text targetText = GetText( newCapturedPiece.PieceType );
			DecreaseNumberLabel( targetText );
		}

		private void IncreaseNumberLabel(TMP_Text text){
			int number = GetNumberFromLabel( text ) + 1;

			if (number > 0) {
				text.transform.parent.gameObject.SetActive( true );
			}
			text.text = "x"+number.ToString();
		}

		private void DecreaseNumberLabel( TMP_Text text ) {
			int number = GetNumberFromLabel( text ) - 1;

			if (number <= 0) {
				text.transform.parent.gameObject.SetActive( false );
			}
			text.text = "x" + number.ToString();
		}

		private int GetNumberFromLabel( TMP_Text text ) {
			string number_raw = text.text.Skip( 1 ).Replace( "\n", "" );
			int number = int.Parse( number_raw );
			return number;
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
