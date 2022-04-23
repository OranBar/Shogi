using System.Collections.Generic;

namespace Shogi {
	public class KingMovement : AMovementStrategy
	{
		public override List<(int x, int y)> GetAvailableMoves( int startX , int startY ) {
			List<(int x, int y)> result = new List<(int x, int y)>();
			result.Add( (startX,     startY + 1) );
			result.Add( (startX - 1, startY + 1) );
			result.Add( (startX + 1, startY + 1) );
			
			result.Add( (startX - 1, startY) );
			result.Add( (startX + 1, startY) );

			result.Add( (startX,     startY - 1) );
			result.Add( (startX - 1, startY - 1) );
			result.Add( (startX + 1, startY - 1) );

			result = FilterInvalid_BoardPositions( result );
			return result;
		}
	}
}