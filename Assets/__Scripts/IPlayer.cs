using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public interface IPlayer
	{
		Task<IShogiAction> RequestAction();
	}
}
