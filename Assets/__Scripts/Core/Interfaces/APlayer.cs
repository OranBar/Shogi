using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public abstract class APlayer : MonoBehaviour
	{
		public abstract string PlayerName { get; set; }
		public abstract PlayerId PlayerId { get; set; }
		public abstract UniTask<IShogiAction> RequestAction();
	}
}
