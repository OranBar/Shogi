using System;
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
			board = FindObjectOfType<ShogiGame>().board;
			piece = GetComponent<Piece>();
		}

		// protected List<(int x, int y)> FilterInvalidMoves( List<(int x, int y)> moves ) {
		// 	var result = moves.ToList();

		// 	result = result.Where( m => board.IsValidBoardPosition( m ) ).ToList();
		// 	result = result.Where( m => Destination_IsNot_OccupiedByAlliedPiece( m ) ).ToList();
		// 	return result;
		// }

		protected List<(int x, int y)> FilterInvalid_BoardPositions(List<(int x, int y)> moves){
			var result = moves.Where( m => board.IsValidBoardPosition( m ) ).ToList();
			return result;
		}
	}
}