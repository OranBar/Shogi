using System;
using NaughtyAttributes;
using UnityEngine;

namespace Shogi
{
	public abstract class ABoard : MonoBehaviour
	{
		private Piece [,] board = new Piece [9, 9];

		public Piece this [int x, int y]
		{
			get { return board [x, y]; }
			set { board [x, y] = value; }
		}

		private void ClearBoard() {
			board = new Piece [9, 9];
		}

		[Button]
		public void RefreshWithPiecesInScene() {
			ClearBoard();
			foreach (var piece in FindObjectsOfType<Piece>()) {
				if (piece.IsCaptured == false) {
					PlacePiece( piece, piece.X, piece.Y );
				}
			}
			Debug.Log( "Board Init complete" );
		}

		public void PlacePiece( Piece piece, int x, int y ) {
			if (IsPieceOnBoard( piece )) {
				throw new Exception( "Can't place a piece that already is on the board" );
			}

			board [x, y] = piece;
			PlacePieceOnCell_Immediate( x, y, piece );
		}


		private bool IsPieceOnBoard( Piece newPiece ) {
			foreach (var piece in board) {
				if (piece == newPiece) {
					return true;
				}
			}
			return false;
		}

		public abstract Vector3 GetCellPosition( int x, int y );
		public abstract void PlacePieceOnCell_Immediate( int x, int y, Piece piece );

		public bool IsValidBoardPosition( (int x, int y) pos ) {
			return IsValidBoardPosition( pos.x, pos.y );
		}

		public bool IsValidBoardPosition( int x, int y ) {
			try {
				var _ = board [x, y];
				return true;
			} catch (IndexOutOfRangeException) {
				return false;
			}
		}

		public bool IsPromotionZone( int x, int y, PlayerId playerId ) {
			if (playerId == PlayerId.Player1) {
				return y >= 6;
			} else if (playerId == PlayerId.Player2) {
				return y <= 2;
			}
			throw new Exception( "PlayerId unknown" );
		}
	}
}
