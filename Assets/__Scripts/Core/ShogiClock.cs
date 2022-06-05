using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{

	public class ShogiClock : MonoBehaviour
	{
		public Timer timer_player1;
		public Timer timer_player2;

		public RefAction OnPlayer1Timer_Finished;
		private ShogiGame shogiGame;

		public Button unpauseButton, pauseButton;

		private void OnValidate() {
			if ( Application.isPlaying == false ) {
				Debug.Assert( timer_player1.enabled == false );
				Debug.Assert( timer_player2.enabled == false );
			}
		}

		void Awake() {
			shogiGame = FindObjectOfType<ShogiGame>();
		}

		public void Start() {
			OnValidate();
			pauseButton.onClick.AddListener( Pause );
			unpauseButton.onClick.AddListener( Unpause );
		}

		void OnEnable() {
			shogiGame.OnNewTurnBegun += ToggleBothTimers;
		}

		void OnDisable() {
			shogiGame.OnNewTurnBegun -= ToggleBothTimers;
		}

		public void ToggleBothTimers( PlayerId playerId ) {
			Timer currPlayerTimer = GetPlayerTimer( playerId );
			currPlayerTimer.enabled = true;

			GetPlayerTimer( playerId.GetOtherPlayer() ).enabled = false;
		}

		public Timer GetPlayerTimer( PlayerId playerId ) {
			return ( playerId == PlayerId.Player1 ) ? timer_player1 : timer_player2;
		}


		//TODO: Hook methods to buttons
		public virtual void Pause() {
			timer_player1.enabled = timer_player2.enabled = false;
			this.enabled = false;

			pauseButton.gameObject.SetActive( false );
			unpauseButton.gameObject.SetActive( true );
		}
		public virtual void Unpause() {
			ToggleBothTimers( shogiGame.CurrTurn_Player.PlayerId );
			this.enabled = true;

			pauseButton.gameObject.SetActive( true );
			unpauseButton.gameObject.SetActive( false );
		}


	}
}