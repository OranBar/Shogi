using System.Collections.Generic;
using System.Linq;

namespace Shogi
{
	public class RookMovement : AMovementStrategy
	{

		public override List<(int x, int y)> GetAvailableMoves( int startX, int startY ) {
			int direction = piece.OwnerId == PlayerId.Player1 ? 1 : -1;

			List<(int x, int y)> result = new List<(int x, int y)>();
			// for (int i = 1 ; i <= 9 ; i++) {
			// 	result.Add( (startX, startY + ( i * direction )) );
			// 	result.Add( (startX, startY - ( i * direction )) );
			// 	result.Add( (startX + ( i * direction ), startY) );
			// 	result.Add( (startX - ( i * direction ), startY) );
			// }

			//Forward cells
			for (int distance = 1 ; distance <= 9 ; distance++) {
				int x = startX;
				int y = startY + ( distance * direction );
				result.Add( (x, y) );

				if (( board.IsValidBoardPosition( x, y ) == false ) || board [x, y] != null) {
					break;
				}
			}

			//Back cells
			for (int distance = 1 ; distance <= 9 ; distance++) {
				int x = startX;
				int y = startY - ( distance * direction );
				result.Add( (x, y) );

				if (( board.IsValidBoardPosition( x, y ) == false ) || board [x, y] != null) {
					break;
				}
			}

			//Right cells
			for (int distance = 1 ; distance <= 9 ; distance++) {
				int x = startX + ( distance * direction );
				int y = startY;
				result.Add( (x, y) );

				if (( board.IsValidBoardPosition( x, y ) == false ) || board [x, y] != null) {
					break;
				}
			}

			//Left cells
			for (int distance = 1 ; distance <= 9 ; distance++) {
				int x = startX - ( distance * direction );
				int y = startY;
				result.Add( (x, y) );

				if (( board.IsValidBoardPosition( x, y ) == false ) || board [x, y] != null) {
					break;
				}
			}

			result = FilterInvalidMoves( result );
			return result;
		}
	}

}