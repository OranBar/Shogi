using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shogi {
	public class Piece {
		public int x, y;
		public IMovementStrategy movementStrategy;
		
		private Board board;

		// public Action<int, int> OnPieceMoved = ( _, __ ) => { };


		public Piece( Board board, int x , int y ,  IMovementStrategy movementStrategy ) {
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
