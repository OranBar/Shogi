using System.Collections.Generic;
using System.Linq;

namespace Shogi
{
	public class LancerMovement : AMovementStrategy
	{

		public override List<(int x, int y)> GetAvailableMoves( int startX, int startY ) {
			int direction = piece.OwnerId == PlayerId.Player1 ? 1 : -1;

			List<(int x, int y)> result = new List<(int x, int y)>();

			for (int distance = 1 ; distance <= 9 ; distance++) {
				int x = startX;
				int y = startY + ( distance * direction );
				result.Add( (x, y) );
				
				if ( (board.IsValidBoardPosition( x, y ) == false) || board [x, y] != null) {
					break;
				}
			}
			
			result = FilterInvalid_BoardPositions(result);
			return result;
		}
	}

}