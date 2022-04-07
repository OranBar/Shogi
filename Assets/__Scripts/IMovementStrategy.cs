using System.Collections.Generic;

namespace Shogi
{
	public interface IMovementStrategy
	{
		List<(int x, int y)> GetAvailableMoves();
	}
}