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
			int direction = piece.OwnerId == PlayerId.Player1 ? 1 : -1;

			List<(int x, int y)> result = new List<(int x, int y)>();
			result.Add( (startX, startY + ( 1 * direction )) );

			result = result.Where( m => board.IsValidBoardPosition( m ) ).ToList();
			result = result.Where( m => board [m.x, m.y]?.OwnerId != piece.OwnerId ).ToList();
			return result;
		}
	}

}