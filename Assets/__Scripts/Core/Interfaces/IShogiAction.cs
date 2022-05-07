using Cysharp.Threading.Tasks;

namespace Shogi
{
	public interface IShogiAction
	{
		int StartX { get; set; }
		int StartY { get; set; }
		int DestinationX { get; set; }
		int DestinationY { get; set; }
		GameState GameState_beforeMove { get; }
		Piece GetActingPiece();
		Cell GetStartCell();
		UniTask ExecuteAction( ShogiGame game );
		UniTask UndoAction( ShogiGame game );
		bool IsMoveValid( ShogiGame game );
		void DisableFX();
	}
}
