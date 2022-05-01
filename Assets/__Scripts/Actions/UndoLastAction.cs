using System;
using System.Linq;
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

			if(game.gameHistory.playedMoves.Count >= 1){
				lastMove = game.gameHistory.playedMoves.Last();
				await lastMove.UndoAction( game );
				await lastMove.ExecuteAction( game );
			}
		}

		public override bool IsMoveValid( ShogiGame game ) {
			return game.gameHistory.playedMoves.Count != 0;
		}
	}
}