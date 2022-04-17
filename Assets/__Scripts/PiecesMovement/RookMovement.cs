using System.Collections.Generic;
using System.Linq;

namespace Shogi
{
	public class RookMovement : AMovementStrategy
	{

		public override List<(int x, int y)> GetAvailableMoves( int startX, int startY ) {
			int direction = piece.OwnerId == PlayerId.Player1 ? 1 : -1;

			List<(int x, int y)> result = new List<(int x, int y)>();
			for (int i = 0 ; i < 9 ; i++) {
				result.Add( (startX, startY + ( 1 * direction )) );
				result.Add( (startX, startY - ( 1 * direction )) );
				result.Add( (startX + ( 1 * direction ), startY) );
				result.Add( (startX - ( 1 * direction ), startY) );
			}

			result = FilterInvalidMoves( result );
			return result;
		}
	}

}