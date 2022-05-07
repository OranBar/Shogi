using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shogi
{
	public abstract class AMovementStrategy : MonoBehaviour
	{
		protected Piece piece;
		protected Board board;

		public abstract List<(int x, int y)> GetAvailableMoves( int x, int y );

		protected virtual void Awake() {
			board = FindObjectOfType<ShogiGame>().board;
			piece = GetComponent<Piece>();
		}

		protected List<(int x, int y)> FilterInvalid_BoardPositions(List<(int x, int y)> moves){
			var result = moves.Where( m => board.IsValidBoardPosition( m ) ).ToList();
			return result;
		}
	}
}