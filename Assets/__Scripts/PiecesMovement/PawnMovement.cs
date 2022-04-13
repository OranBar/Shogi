using System.Collections.Generic;

namespace Shogi
{
	public class PawnMovement : UnityEngine.MonoBehaviour, IMovementStrategy
	{
		private Piece piece;
		void Start() {
			piece = GetComponent<Piece>();
		}

		public List<(int x, int y)> GetAvailableMoves( int startX, int startY ) {
			List<(int x, int y)> result = new List<(int x, int y)>();
			result.Add( GetCellIndex_Forward( startX, startY, 1 ) );
			return result;
		}


		private (int x, int y) GetCellIndex_Forward( int startX, int startY, int distance ) {
			if (piece.OwnerId == PlayerId.Player1) {
				return (startX, startY + distance);
			} else {
				return (startX, startY - distance);
			}
		}
	}

}