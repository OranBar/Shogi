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

		void Start() {
			piece = new Piece( x, y, defaultMovement );
		}

		public void Promote(){
			piece.movementStrategy = promotedMovement;
		}
	}
}
