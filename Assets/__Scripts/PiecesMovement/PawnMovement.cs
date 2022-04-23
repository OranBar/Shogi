using System.Collections.Generic;
using System.Linq;

namespace Shogi
{
	public class PawnMovement : AMovementStrategy
	{
		public override List<(int x, int y)> GetAvailableMoves( int startX, int startY ) {
			int direction = piece.OwnerId == PlayerId.Player1 ? 1 : -1;

			List<(int x, int y)> result = new List<(int x, int y)>();
			result.Add( (startX, startY + ( 1 * direction )) );

			result = FilterInvalid_BoardPositions( result );
			return result;
		}

	}

}