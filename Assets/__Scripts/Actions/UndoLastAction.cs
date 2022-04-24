using System;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	[Serializable]
	public class UndoLastAction : AShogiAction
	{
		public UndoLastAction( ) : base( ) {
		}

		public override async UniTask ExecuteAction( ShogiGame game ) {
			var lastMove = game.gameHistory.playedMoves.Pop();
			await lastMove.UndoAction( game );
		}

		public override bool IsMoveValid( ShogiGame game ) {
			return game.gameHistory.playedMoves.Count == 0;
		}
	}
}