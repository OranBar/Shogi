using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;


/* TODO: Sideboard_UI should probably extend Sideboard, and have different implementations for how the piece is placed/added to the sideboard.
In 3D, we just want to change its position, but when we are in 2D, we want to also handle the numbers that show how many pieces of each are contained in the sideboard.
We can still move the objects in 2D, but probably it needs more logic to prevent overlap clicks, like playing with the z-axis of sideboard elements.
*/

namespace Shogi
{
	[Serializable]
	public class SideBoard_UI : SideBoard, ISideboardFX
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

		public float add_piece_anim_duration = 0.6f;

		public async UniTask PieceAddedToSideboard_FX( Piece newPiece ) {
			if (this.enabled == false) { return; }
			// Make the piece higher in z position based on the number of pieces of the same type in the sideboard, to ensure we always click the topmost
			//Don't disable the gameobject. It breaks a lot of stuff. Disable the child
			newPiece.SetPieceGraphicsActive(false);

			
			var sequence = DOTween.Sequence();
			await sequence
				.PrependInterval( .1f )
				.AppendCallback( ()=> PlacePiece_OnSideboard_Immediate(newPiece) )
				.AppendCallback( () => newPiece.SetPieceGraphicsActive( true ) )
				.AppendCallback( () => newPiece.SetPieceGraphicsActive( true ) )
				.AppendCallback( () => newPiece.transform.localScale = Vector3.one * 2 )
				.AppendCallback( () => IncreaseText( newPiece ) )
				.Append( newPiece.transform.DOScale( Vector3.one, add_piece_anim_duration ).SetEase( Ease.OutCubic ) )
				.AsyncWaitForCompletion();
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

        public override void PlacePiece_OnSideboard_Immediate(Piece piece)
        {
            Transform sideboardPiece = GetText( piece.PieceType ).transform.parent;
			piece.transform.position = sideboardPiece.transform.position;
        }
    }

}
