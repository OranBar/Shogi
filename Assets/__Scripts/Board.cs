using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
	public class Board : MonoBehaviour
    {
		public Piece [,] board = new Piece[9,9];


		public float cellSizeUnit = 37.4f;


		public Piece this [int x, int y]
		{
			get { return board [x, y]; }
			set { board [x, y] = value; }
		}

		void Awake()
        {

		}

		public void UpdateBoard( MovePieceAction action) {
			board [action.piece.X, action.piece.Y] = null;
			board [action.destinationX, action.destinationY] = action.piece;
		}

		public Vector3 GetCellWorldPosition(  int x, int y ) {
			return new Vector3( x, y ) * cellSizeUnit;
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
