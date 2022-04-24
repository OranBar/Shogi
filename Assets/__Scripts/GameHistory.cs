using System;
using System.Collections.Generic;

namespace Shogi
{
	[Serializable]
	public class GameHistory {
		public GameState initialGameState;
		public PlayerId firstToMove;
		public List<IShogiAction> playedMoves = new List<IShogiAction>();

		public GameHistory( GameState initialGameState, PlayerId firstToMove) {
			this.initialGameState = initialGameState;
			this.firstToMove = firstToMove;
		}

		public void RegisterNewMove( IShogiAction action ) {
			if(action is UndoLastAction){
				return;
			}
			playedMoves.Push( action );
		}
	}
	
}