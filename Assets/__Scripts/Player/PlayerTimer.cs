using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi{
	public class PlayerTimer : Timer
	{
		public PlayerId owner;
		[Auto] Image image;
		[Auto] Outline outline;

		void OnEnable()
		{
			image.color = image.color.SetAlpha( 1f );
			outline.enabled = true;
		}

		void OnDisable(){
			image.color = image.color.SetAlpha( 0.5f );
			outline.enabled = false;
		}

	}
}
