using Cysharp.Threading.Tasks;

namespace Shogi
{
	public interface IPieceMoveActionFX
	{
		UniTask DoMoveAnimation( int destinationX, int destinationY );
	}
}
