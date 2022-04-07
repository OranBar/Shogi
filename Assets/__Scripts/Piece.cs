using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shogi {
	public class Piece {
		public int x, y;
		public IMovementStrategy movementStrategy;
		
		private Board<Piece> board;

		public Piece( Board<Piece> board, int x , int y ,  IMovementStrategy movementStrategy ) {
			this.x = x;
			this.y = y;
			this.board = board;
			this.movementStrategy = movementStrategy;
		}

		public List<(int x, int y)> GetAvailableMoves() {
			var moves = movementStrategy.GetAvailableMoves( x , y );
			var result = moves.Where( m => board.IsValidBoardPosition( m ) ).ToList();
			return result;
		}
	}
}
