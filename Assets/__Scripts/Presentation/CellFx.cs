using System.Collections;
using System.Collections.Generic;
using BarbarO.ExtensionMethods;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
    public class CellFx : MonoBehaviour, IHighlightFx
    {
		private Image highlightImage;
		public bool isHighlighted = false;

		void Start()
		{
			highlightImage = this.transform.GetChild( 0 ).GetComponent<Image>();
			highlightImage.enabled = false;
		}

		public async UniTask EnableHighlight( Color color ) {
			if(isHighlighted){ return; }

			highlightImage.enabled = true;
			highlightImage.color = color;
			isHighlighted = true;
		}

		public async UniTask DisableHighlight() {
			highlightImage.enabled = false;
			isHighlighted = false;
		}
	}
}
