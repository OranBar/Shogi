using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public interface IPieceDropActionFX
	{
		UniTask DoDropAnimation( DropPieceAction action );
	}
}
