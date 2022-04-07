using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi {
	public class Piece {
		public int x, y;
		public IMovementStrategy movementStrategy;

		public Piece( int x , int y , IMovementStrategy movementStrategy ) {
			this.x = x;
			this.y = y;
			this.movementStrategy = movementStrategy;
		}

		public List<(int x, int y)> GetAvailableMoves() {
			return movementStrategy.GetAvailableMoves( x , y );
		}
	}
}
