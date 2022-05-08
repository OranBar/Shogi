using System;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	[Serializable]
	public class UndoLastAction : AShogiAction
	{
		public UndoLastAction() : base() {
		}

		public override async UniTask ExecuteAction( ShogiGame game ) {
			int playedMovesCount = game.gameHistory.playedMoves.Count;

			var lastMove = game.gameHistory.playedMoves [playedMovesCount - 1];
			await lastMove.UndoAction( game );

			if (playedMovesCount >= 2) {
				var secondToLastMove = game.gameHistory.playedMoves [playedMovesCount - 2];
				await secondToLastMove.UndoAction( game );
				await secondToLastMove.ExecuteAction( game );
			}
		}

		public override bool IsMoveValid( ShogiGame game ) {
			return game.gameHistory.playedMoves.Count != 0;
		}

		public override string ToString() {
			return "Undo " + base.ToString();
		}

		public override void DisableLastMoveFX() { }

		public override async UniTask EnableLastMoveFX( GameSettings settings ) { }
	}
}