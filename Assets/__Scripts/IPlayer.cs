using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public interface IPlayer
	{
		UniTask<IShogiAction> RequestAction();
	}
}
