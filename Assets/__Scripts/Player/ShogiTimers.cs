using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{


	public class ShogiTimers : MonoBehaviour
	{
		public PlayerTimer timer_player1;
		public PlayerTimer timer_player2;

		public RefAction OnPlayer1Timer_Finished;


		public void Start(){
			var shogiGame = FindObjectOfType<ShogiGame>();
			shogiGame.OnNewTurnBegun += ToggleBothTimers;

			timer_player1.enabled = false;
			timer_player2.enabled = false;
		}

		public void ToggleBothTimers( PlayerId playerId ){
			GetPlayerTimer( playerId ).enabled = true;
			GetPlayerTimer( playerId.GetOtherPlayer() ).enabled = false;
		}

		public Timer GetPlayerTimer( PlayerId playerId ) {
			return ( playerId == PlayerId.Player1 ) ? timer_player1 : timer_player2;
		}
	}
}