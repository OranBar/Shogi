using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
    public class ShogiGame : MonoBehaviour
    {
		private const int WIDTH = 9, HEIGHT = 9;
		
		public Board<Piece> board;
		
		void Start()
        {
			SetupBoard();
		}

		void SetupBoard() {
			board = new Board<Piece>( WIDTH, HEIGHT );
		}
    }
}
