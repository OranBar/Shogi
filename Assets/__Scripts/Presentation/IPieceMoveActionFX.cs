using Cysharp.Threading.Tasks;

namespace Shogi
{
	public interface IPieceMoveActionFX
	{
		UniTask DoMoveAnimation( MovePieceAction action );
	}
}
