using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public interface IPlayer
	{
		PlayerId PlayerId { get; }
		void Init();
		UniTask<IShogiAction> RequestAction();
	}
}
