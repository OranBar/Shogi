using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;


namespace Shogi
{
	[Serializable]
	public class SideBoard_UI : MonoBehaviour, ISideboardFX
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
		}

		void Start() {
			ResetAllTextsToZero();
		}

		void OnDisable(){
			sideBoard.OnNewPieceAdded.Value -= IncreaseText;
			sideBoard.OnNewPieceRemoved.Value -= DecreaseText;
		}

		public async UniTask PieceAddedToSideboard_FX( Piece newPiece ) {
			PieceAddedToSideboardFx( newPiece );

			
			void PieceAddedToSideboardFx( Piece newPiece ) {
				Transform sideboardPiece = GetText( newPiece.PieceType ).transform.parent;
				newPiece.transform.position = sideboardPiece.transform.position;
			}
		}

		// private void OnPieceButtonClicked( PieceType pieceType ) {
		// 	Logger.Log( "Ho clickato" );
		// 	Piece piece = sideBoard.capturedPieces.First( p => p.PieceType == pieceType );
		// 	piece.OnPointerClick( new PointerEventData( null ) );
		// }

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


			void IncreaseNumberLabel( TMP_Text text ) {
				int number = GetNumberFromLabel( text ) + 1;

				if (number > 0) {
					text.transform.gameObject.SetActive( true );
				}
				text.text = "x" + number.ToString();
			}
		}

		public void DecreaseText( Piece newCapturedPiece ) {
			TMP_Text targetText = GetText( newCapturedPiece.PieceType );
			DecreaseNumberLabel( targetText );


			void DecreaseNumberLabel( TMP_Text text ) {
				int number = GetNumberFromLabel( text ) - 1;

				if (number <= 0) {
					text.transform.gameObject.SetActive( false );
				}
				text.text = "x" + number.ToString();
			}
		}

		private int GetNumberFromLabel( TMP_Text text ) {
			string number_raw = text.text.Skip( 1 ).Replace( "\n", "" );
			int number = int.Parse( number_raw );
			return number;
		}

	}

}
