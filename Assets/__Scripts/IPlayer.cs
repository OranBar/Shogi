using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public interface IPlayer
	{
		PlayerId OwnerId { get; }
		UniTask<IShogiAction> RequestAction();
	}
}
