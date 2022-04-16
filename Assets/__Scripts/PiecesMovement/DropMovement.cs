using System.Collections.Generic;

namespace Shogi
{
	public class DropMovement : UnityEngine.MonoBehaviour, IMovementStrategy
	{
		private Piece piece;
		void Start() {
			piece = GetComponent<Piece>();
		}

		public List<(int x, int y)> GetAvailableMoves( int startX, int startY ) {
			int direction = piece.OwnerId == PlayerId.Player1 ? 1 : -1;

			List<(int x, int y)> result = new List<(int x, int y)>();
			return result;
		}
	}

}