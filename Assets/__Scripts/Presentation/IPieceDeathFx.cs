using Cysharp.Threading.Tasks;

namespace Shogi
{
	public interface IPieceDeathFx
	{
		UniTask DoPieceDeathAnimation();
	}
}
