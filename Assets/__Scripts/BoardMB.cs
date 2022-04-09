using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
    public class BoardMB : MonoBehaviour
    {
		public Board board;
		public float cellSizeUnit = 37.4f;

		void Awake()
        {
			board = new Board(9,9);
		}

		public void UpdateBoard( MovePieceAction action) {
			board [action.piece.X, action.piece.Y] = null;
			board [action.destinationX, action.destinationY] = action.piece.piece;
		}
    }
}
