using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi {
	public class PieceTest : MonoBehaviour {
		public Piece piece;
		//We're doing depencenty injecton by referencing MB from inspector
		public IMovementStrategy defaultMovement;
		public IMovementStrategy promotedMovement;

		public int x, y;

		private BoardTest board;
		
		void Awake(){
			board = FindObjectOfType<BoardTest>();
		}

		void Start() {
			piece = new Piece( board.board, x, y, defaultMovement );
		}

		public void Promote(){
			piece.movementStrategy = promotedMovement;
		}
	}
}
