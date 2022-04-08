using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
    public class ShogiGameTest : MonoBehaviour
    {
		private const int WIDTH = 9, HEIGHT = 9;

		public BoardTest board;
		
		void Start()
        {
			AddPiecesFromScene();
		}

		void AddPiecesFromScene(){
			foreach(var piece in FindObjectsOfType<PieceTest>()){
				board.board [piece.X, piece.Y] = piece.piece;
			}
		}
    }
}
