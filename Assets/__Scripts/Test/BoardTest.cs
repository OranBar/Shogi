using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
    public class BoardTest : MonoBehaviour
    {
		public Board<Piece> board;
		public float cellSizeUnit = 37.4f;

		void Awake()
        {
			board = new Board<Piece>(9,9);
		}

		public void MovePiece(PieceTest piece, int destX, int destY) {
			board [piece.X, piece.Y] = null;
			board [destX, destY] = piece.piece;
			piece.X = destX;
			piece.Y = destY;
			piece.OnPieceMoved?.Invoke(destX, destY);
		}
    }
}
