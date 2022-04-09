using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
    public class BoardMB : MonoBehaviour
    {
		public float cellSizeUnit = 37.4f;

		public int width, height;
		public PieceMB [,] board;

		public Action<PieceMB, int, int> OnAnyPieceMoved = ( a, b, c ) => { };

		public PieceMB this [int x, int y]
		{
			get { return board [x, y]; }
			set { board [x, y] = value; }
		}

		public BoardMB( int width, int height ) {
			this.width = width;
			this.height = height;
			this.board = new PieceMB [width, height];
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

		void Awake()
        {

		}

		public void UpdateBoard( MovePieceAction action) {
			board [action.piece.x, action.piece.y] = null;
			board [action.destinationX, action.destinationY] = action.piece;
		}

		
    }
}
