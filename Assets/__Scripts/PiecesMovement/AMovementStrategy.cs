using System.Collections.Generic;
using System.Linq;

namespace Shogi
{
	public abstract class AMovementStrategy : UnityEngine.MonoBehaviour, IMovementStrategy
	{
		protected Piece piece;
		protected Board board;

		public abstract List<(int x, int y)> GetAvailableMoves( int x, int y );

		protected virtual void Awake() {
			piece = GetComponent<Piece>();
			board = FindObjectOfType<ShogiGame>().board;
		}
	}
}