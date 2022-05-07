using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public interface IPieceHighlight
	{
		UniTask EnableHighlight( Color color );
		UniTask DisableHighlight();
	}
}
