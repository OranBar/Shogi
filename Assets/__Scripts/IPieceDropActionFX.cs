using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public interface IPieceDropActionFX
	{
		UniTask DoDropAnimation( int destinationX, int destinationY );
	}
}
