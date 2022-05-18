using System.Collections;
using System.Collections.Generic;
using BarbarO.ExtensionMethods;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
    public class CellFX : MonoBehaviour, IHighlightFx
    {
		private Image highlightImage;

		void Start()
		{
			highlightImage = this.transform.GetChild( 0 ).GetComponent<Image>();
			highlightImage.enabled = false;
		}

		public async UniTask EnableHighlight( Color color ) {
			highlightImage.enabled = true;
			highlightImage.color = color;
		}

		public async UniTask DisableHighlight() {
			highlightImage.enabled = false;
		}
	}
}
