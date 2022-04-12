using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public enum PlayerId{
		Player1 = 1,
		Player2
	}

	public interface IPlayer
	{
		Task<IShogiAction> RequestAction();
	}

	public class HumanPlayer : MonoBehaviour, IPlayer
	{
		public string playerName;
		public PlayerId playerId;

		void Start() {

		}

		public Task<IShogiAction> RequestAction() {
			throw new NotImplementedException();
		}
	}
}
