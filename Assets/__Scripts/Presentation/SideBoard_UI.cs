using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Shogi
{
	[Serializable]
	public class SideBoard_UI : MonoBehaviour, ISideboardPieceAdded
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
			sideBoard.OnNewPieceRemoved += DecreaseText;
			sideBoard.OnCleared += ResetAllTextsToZero;
		}

		public async UniTask OnNewPieceAdded( Piece newPiece ) {
			PieceAddedToSideboardFx( newPiece );
		}

		private void PieceAddedToSideboardFx( Piece newPiece ) {
			Transform sideboardPiece = GetText( newPiece.PieceType ).transform.parent;
			newPiece.transform.position = sideboardPiece.transform.position;
			// //Make the piece higher in z position based on the number of pieces of the same type in the sideboard, to ensure we always click the topmost
			// //Don't disable the gameobject. It breaks a lot of stuff. Disable the child
			// newPiece.transform.GetChild(0).gameObject.SetActive( false );

			// var sequence = DOTween.Sequence();
			// sequence
			// 	.PrependInterval( .3f )
			// 	.AppendCallback( () => newPiece.transform.GetChild( 0 ).gameObject.SetActive( true ) )
			// 	.AppendCallback( () => newPiece.transform.localScale = Vector3.one * 2 )
			// 	.AppendCallback( () => IncreaseText( newPiece ) )
			// 	.Append( newPiece.transform.DOScale( Vector3.one, 1.5f ).SetEase( Ease.OutCubic ) );
		}

		void OnDisable(){
			sideBoard.OnNewPieceAdded.Value -= IncreaseText;
			sideBoard.OnNewPieceRemoved.Value -= DecreaseText;
			sideBoard.OnCleared.Value -= ResetAllTextsToZero;
		}

		void Start() {
			ResetAllTextsToZero();
		}

		private void OnPieceButtonClicked( PieceType pieceType ) {
			Debug.Log( "Ho clickato" );
			Piece piece = sideBoard.CapturedPieces.First( p => p.PieceType == pieceType );
			piece.OnPointerClick( new PointerEventData( null ) );
		}

		private void ResetAllTextsToZero(){
			pawnText.text = "x0";
			lancerText.text = "x0";
			knightText.text = "x0";
			silverText.text = "x0";
			goldText.text = "x0";
			rookText.text = "x0";
			bishopText.text = "x0";

			foreach(TMP_Text text in GetComponentsInChildren<TMP_Text>()){
				text.gameObject.SetActive( false );
			}
		}

		public void IncreaseText( Piece newCapturedPiece) {
			TMP_Text targetText = GetText( newCapturedPiece.PieceType );
			IncreaseNumberLabel( targetText );
		}

		public void DecreaseText( Piece newCapturedPiece ) {
			TMP_Text targetText = GetText( newCapturedPiece.PieceType );
			DecreaseNumberLabel( targetText );
		}

		private void IncreaseNumberLabel(TMP_Text text){
			int number = GetNumberFromLabel( text ) + 1;
			
			if (number > 0) {
				text.transform.gameObject.SetActive( true );
			}
			text.text = "x"+number.ToString();
		}

		private void DecreaseNumberLabel( TMP_Text text ) {
			int number = GetNumberFromLabel( text ) - 1;

			if (number <= 0) {
				text.transform.gameObject.SetActive( false );
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
