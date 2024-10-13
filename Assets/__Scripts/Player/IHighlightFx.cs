using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public interface IHighlightFx
	{
		UniTask EnableHighlight( Color color );
		UniTask DisableHighlight();
		bool IsHighlighted { get; }
	}
}
