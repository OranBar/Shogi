using Cysharp.Threading.Tasks;

namespace Shogi
{
	public interface IShogiAction
	{
		public int StartX { get; set; }
		public int StartY { get; set; }
		public int DestinationX { get; set; }
		public int DestinationY { get; set; }
		public Piece ActingPiece{ get; }
		UniTask ExecuteAction( ShogiGame game );
		UniTask UndoAction( ShogiGame game );
		bool IsMoveValid( ShogiGame game );
	}
}
