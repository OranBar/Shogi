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
			int count = game.gameHistory.playedMoves.Count;
			//Side effect. GameHistory playedmoves is modified, the last move is removed from the list
			var lastMove = game.gameHistory.playedMoves[count - 1];
			await lastMove.UndoAction( game );

			if(count >= 1){
				var secondToLastMove = game.gameHistory.playedMoves[count - 2];
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