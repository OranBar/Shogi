using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
	public enum PlayerId{
		Player1,
		Player2
	}
	
    public class Player : MonoBehaviour
    {
		public string playerName;
		public PlayerId playerId;

	}
}
