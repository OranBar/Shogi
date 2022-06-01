using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	public interface IPromotionPromter
	{
		UniTask<bool> GetPromotionChoice(AShogiAction action);
	}
}