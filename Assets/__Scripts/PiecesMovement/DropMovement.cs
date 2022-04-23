using System.Collections.Generic;

namespace Shogi
{
	public class DropMovement : AMovementStrategy
	{
		public override List<(int x, int y)> GetAvailableMoves( int startX, int startY ) {
			// int direction = piece.OwnerId == PlayerId.Player1 ? 1 : -1;

			List<(int x, int y)> result = new List<(int x, int y)>();
			//Get All tiles that have no pieces on them.
			for (int x = 0 ; x < 9 ; x++) {
				for (int y = 0 ; y < 9 ; y++) {
					if (board [x, y] == null) {
						result.Add( (x, y) );
					}
				}
			}
			result = FilterInvalid_BoardPositions( result );
			//Validation: Can't checkmate with pawn
			//Validation: Can't place pawn if there already is one on the same column
			//Validation: Can't place piece where it would have no available moves
			return result;
		}
	}

}