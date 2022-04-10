using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
	public class BoardMB : MonoBehaviour
    {
		#region ToSerialize
		//I Actually don't want the references. 
		//I do want what's inside tho
		public PieceMB [,] board = new PieceMB[9,9];
		#endregion

		public float cellSizeUnit = 37.4f;


		public PieceMB this [int x, int y]
		{
			get { return board [x, y]; }
			set { board [x, y] = value; }
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
