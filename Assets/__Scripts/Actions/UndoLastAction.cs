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
			//Side effect. GameHistory playedmoves is modified, the last move is removed from the list
			var lastMove = game.gameHistory.playedMoves.Pop();
			await lastMove.UndoAction( game );

			if(game.gameHistory.playedMoves.Count >= 1){
				var secondToLastMove = game.gameHistory.playedMoves.Last();
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

		public override void DisableFX() { }
	}
}