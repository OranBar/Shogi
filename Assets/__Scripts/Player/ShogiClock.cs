using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{


	public class ShogiClock : MonoBehaviour
	{
		public Timer timer_player1;
		public Timer timer_player2;

		public RefAction OnPlayer1Timer_Finished;
		private ShogiGame shogiGame;

		private void OnValidate(){
			Debug.Assert( timer_player1.enabled == false );
			Debug.Assert( timer_player2.enabled == false );
		}
		
		void Awake()
		{
			shogiGame = FindObjectOfType<ShogiGame>();
		}

		public void Start(){
			OnValidate();
			
			timer_player1.enabled = false;
			timer_player2.enabled = false;
		}

		void OnEnable(){
			shogiGame.OnNewTurnBegun += ToggleBothTimers;
		}

		void OnDisable(){
			shogiGame.OnNewTurnBegun -= ToggleBothTimers;
		}

		public void ToggleBothTimers( PlayerId playerId ){
			Timer currPlayerTimer = GetPlayerTimer( playerId );
			currPlayerTimer.enabled = true;

			GetPlayerTimer( playerId.GetOtherPlayer() ).enabled = false;
		}

		public Timer GetPlayerTimer( PlayerId playerId ) {
			return ( playerId == PlayerId.Player1 ) ? timer_player1 : timer_player2;
		}
	}
}