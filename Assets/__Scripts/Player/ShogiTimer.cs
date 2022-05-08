using NaughtyAttributes;
using UnityEngine.UI;

namespace Shogi{
	public enum TimerType{
		Standard = 0,
		Increment,
	}

	public class ShogiTimer : Timer
	{
		public PlayerId owner;
		public TimerType timerType;
		[BoxGroup("Increment Timer Settings"), ShowIf( "@timerType == TimerType.Increment" )]
		public float incrementAmount;

		[Auto] Image image;
		[Auto] Outline outline;


		void OnEnable()
		{
			image.color = image.color.SetAlpha( 1f );
			outline.enabled = true;
			if(timerType == TimerType.Increment){
				this.clockTime += incrementAmount;
			}
		}

		void OnDisable(){
			image.color = image.color.SetAlpha( 0.5f );
			outline.enabled = false;
		}
	}
}
