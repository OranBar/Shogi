using System;
using System.Collections.Generic;

namespace Shogi
{
	[Serializable]
	public class GameHistory {
		public GameState initialGameState;
		public PlayerId firstToMove;
		public Stack<IShogiAction> playedMoves = new Stack<IShogiAction>();

		public GameHistory( GameState initialGameState, PlayerId firstToMove) {
			this.initialGameState = initialGameState;
			this.firstToMove = firstToMove;
		}

		internal void RegisterNewMove( IShogiAction action ) {
			if(action is not UndoLastAction){
				playedMoves.Push( action );
			}
		}
	}
	
}