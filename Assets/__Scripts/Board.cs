using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi {
	public class Board {
		public int width, height;
		public Piece [,] board;

		public Action<Piece, int, int> OnAnyPieceMoved = (a,b,c) => { };

		public Piece this [int x , int y]
		{
			get { return board [x , y]; }
			set { board [x , y] = value; }
		}

		public Board( int width , int height ) {
			this.width = width;
			this.height = height;
			this.board = new Piece [width , height];
		}

		public bool IsValidBoardPosition( (int x, int y) pos ) {
			return IsValidBoardPosition( pos.x, pos.y );
		}
		
		public bool IsValidBoardPosition( int x , int y ) {
			try {
				var _ = board [x , y];
				return true;
			} catch (IndexOutOfRangeException) {
				return false;
			}
		}


	}
}
