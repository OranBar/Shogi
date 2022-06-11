using System;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	[Serializable]
	public class UndoLastAction : AShogiAction
	{
		public UndoLastAction() : base() {
		}

		public override void ExecuteAction( ShogiGame game ) {
			int playedMovesCount = game.gameHistory.playedMoves.Count;

			var lastMove = game.gameHistory.playedMoves [playedMovesCount - 1];
			lastMove.UndoAction( game );
		}

		public override bool IsMoveValid( ShogiGame game ) {
			return game.gameHistory.playedMoves.Count != 0;
		}

		public override string ToString() {
			return "Undo " + base.ToString();
		}

	}
}