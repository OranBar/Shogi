using System.Collections;
using System.Collections.Generic;
using BarbarO.ExtensionMethods;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
    public class CellFx : MonoBehaviour
    {
		private Image highlightImage;

		void Start()
		{
			highlightImage = this.transform.GetChild( 0 ).GetComponent<Image>();
			highlightImage.enabled = false;
		}

		public void ActivateHighlight( Color color ) {
			highlightImage.enabled = true;
			highlightImage.color = color;
		}

		public void DeactivateHightlight() {
			highlightImage.enabled = false;
		}
    }
}
