using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public abstract class AActionSelectionStrategy : MonoBehaviour {
		public abstract UniTask<AShogiAction> RequestAction();
	}
}