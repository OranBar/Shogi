using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public interface IHighlight
	{
		UniTask EnableHighlight( Color color );
		UniTask DisableHighlight();
	}
}
