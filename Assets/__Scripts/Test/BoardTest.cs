using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
    public class BoardTest : MonoBehaviour
    {
		public Board<Piece> board;

		void Start()
        {
			board = new Board<Piece>(5,10);

		}

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
