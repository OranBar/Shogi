using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Shogi
{
	public class Timer : MonoBehaviour
	{
		public float clockTime;
		[AutoChildren] private TMP_Text timerText;

		public RefAction OnTimerFinished;

		void Update() {
			if (clockTime > 0) {
				clockTime -= Time.deltaTime;
				timerText.text = ConvertToTime( clockTime );
			} else if (clockTime <= 0) {
				OnTimerFinished?.Invoke();
				this.enabled = false;
			}
		}

		private string ConvertToTime( float time ) {
			var minutes = Mathf.FloorToInt( time / 60 );
			var seconds = Mathf.FloorToInt( time % 60 );
			return string.Format( "{0:00}:{1:00}", minutes, seconds );
		}
	}
}