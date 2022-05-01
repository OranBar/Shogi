using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public interface IPlayer
	{
		public string PlayerName { get; set; }
		PlayerId PlayerId { get; }
		UniTask<IShogiAction> RequestAction();
	}
}
