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
			piece = GetComponent<Piece>();
			board = FindObjectOfType<ShogiGame>().board;
		}

		protected List<(int x, int y)> FilterInvalidMoves( List<(int x, int y)> moves ) {
			var result = moves.ToList();

			result = result.Where( m => board.IsValidBoardPosition( m ) ).ToList();
			result = result.Where( m => DestinationIsNotAlliedPiece( m ) ).ToList();
			return result;
		}
		
		private bool DestinationIsNotAlliedPiece( (int x, int y) move ) => board [move.x, move.y]?.OwnerId != piece.OwnerId;
	}
}