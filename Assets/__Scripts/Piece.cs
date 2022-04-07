using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi {
	public class Piece {
		public int x, y;
		public IMovementStrategy movementStrategy;

		public List<(int x, int y)> GetAvailableMoves() {
			return movementStrategy.GetAvailableMoves();
		}

		public void Promote() {
			for (int i = 0; i < 4; i++)
			{
				
			}
		}
	}
}
