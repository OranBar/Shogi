using System.Collections.Generic;

namespace Shogi
{
	public class BishopMovement : AMovementStrategy
	{

		public override List<(int x, int y)> GetAvailableMoves( int startX, int startY ) {
			int direction = piece.OwnerId == PlayerId.Player1 ? 1 : -1;

			List<(int x, int y)> result = new List<(int x, int y)>();
		
			//Forward-Right cells
			for (int distance = 1 ; distance <= 9 ; distance++) {
				int x = startX + ( distance * direction );
				int y = startY + ( distance * direction );
				result.Add( (x, y) );

				if (( board.IsValidBoardPosition( x, y ) == false ) || board [x, y] != null) {
					break;
				}
			}

			//Back-Left cells
			for (int distance = 1 ; distance <= 9 ; distance++) {
				int x = startX - ( distance * direction );
				int y = startY - ( distance * direction );
				result.Add( (x, y) );

				if (( board.IsValidBoardPosition( x, y ) == false ) || board [x, y] != null) {
					break;
				}
			}

			//Forward-Left cells
			for (int distance = 1 ; distance <= 9 ; distance++) {
				int x = startX - ( distance * direction );
				int y = startY + ( distance * direction ); 
				result.Add( (x, y) );

				if (( board.IsValidBoardPosition( x, y ) == false ) || board [x, y] != null) {
					break;
				}
			}

			//Back-Right cells
			for (int distance = 1 ; distance <= 9 ; distance++) {
				int x = startX + ( distance * direction );
				int y = startY - ( distance * direction ); 
				result.Add( (x, y) );

				if (( board.IsValidBoardPosition( x, y ) == false ) || board [x, y] != null) {
					break;
				}
			}

			result = FilterInvalid_BoardPositions( result );
			return result;
		}
	}

}