using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
	public class Board : MonoBehaviour
    {
		public Piece [,] board = new Piece[9,9];
		public float cellSizeUnit = 37.4f;
		public List<Piece> gio_capturedPieces = new List<Piece>();
		public List<Piece> oran_capturedPieces = new List<Piece>();

		public Piece this [int x, int y]
		{
			get { return board [x, y]; }
			set { board [x, y] = value; }
		}

		public void InitWithPiecesInScene() {
			foreach (var piece in FindObjectsOfType<Piece>()) {
				PlacePiece( piece, piece.X, piece.Y );
			}
		}

		public void PlacePiece( Piece piece, int x, int y ) {
			if(IsPieceOnBoard( piece )){
				throw new Exception( "Can't place a piece that already is on the board" );
			}

			board [x, y] = piece;
			piece.PlacePieceOnCell_Immediate( x, y );
		}

		private bool IsPieceOnBoard(Piece newPiece) {
			foreach(var piece in board){
				if(piece == newPiece){
					return true;
				}
			}
			return false;
		}

		// public void UpdateBoard( MovePieceAction action) {
		// 	Piece piece = board [action.startX, action.startY];
		// 	board [piece.X, piece.Y] = null;
		// 	board [action.destinationX, action.destinationY] = piece;
		// }

		public Vector3 GetCellWorldPosition(  int x, int y ) {
			return new Vector3( x, y ) * cellSizeUnit + Vector3.one * cellSizeUnit * 0.5f;
		}
		
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
		
    }
}
