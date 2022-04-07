using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi {
	public class Board<T> {
		public int width, height;
		public T [,] board;

		public T this [int x , int y]
		{
			get { return board [x , y]; }
			set { board [x , y] = value; }
		}

		// public T this [(int x, int y) pos]
		// {
		// 	get { return board [pos.x , pos.y]; }
		// 	set { board [pos.x , pos.y] = value; }
		// }

		public Board( int width , int height ) {
			this.width = width;
			this.height = height;
			this.board = new T [width , height];
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
