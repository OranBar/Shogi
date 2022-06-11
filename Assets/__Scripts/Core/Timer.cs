using TMPro;
using UnityEngine;


namespace Shogi
{
	public class Timer : MonoBehaviour
	{
		public float clockTime;
		[AutoChildren] private TMP_Text timerText;

		public RefAction OnTimerFinished = new RefAction();

		void Update() {
			if (clockTime > 0) {
				clockTime -= Time.deltaTime;
				timerText.text = ConvertToTime( clockTime );
			} else if (clockTime <= 0) {
				clockTime = 0f;
				OnTimerFinished?.Invoke();
				this.enabled = false;
			}
		}

		public string ConvertToTime( float time ) {
			var minutes = Mathf.FloorToInt( time / 60 );
			var seconds = Mathf.FloorToInt( time % 60 );
			return string.Format( "{0:00}:{1:00}", minutes, seconds );
		}
	}
}