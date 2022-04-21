using System.Collections.Generic;
using System.Linq;

namespace Shogi
{
	public class LancerMovement : AMovementStrategy
	{

		public override List<(int x, int y)> GetAvailableMoves( int startX, int startY ) {
			int direction = piece.OwnerId == PlayerId.Player1 ? 1 : -1;

			List<(int x, int y)> result = new List<(int x, int y)>();

			int i = 1;
			bool isCellOccupied;
			do {
				int currX = startX;
				int currY = startY + ( i * direction );
				result.Add( (currX, currY) );
				isCellOccupied = board [currX, currY] != null;
			} while (i++ <= 9 && (isCellOccupied==false) );
			
			result = FilterInvalidMoves(result);
			return result;
		}
	}

}