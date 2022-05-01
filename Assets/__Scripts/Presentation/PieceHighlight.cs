using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	public class PieceHighlight : MonoBehaviour, IPieceHighlight
	{
		public GameObject highlightObj;

		private Image highlight;

		void Start() {
			highlight = highlightObj.GetComponent<Image>();
			highlightObj.SetActive( false );
		}

		public void EnableHighlight( Color color ) {
			highlightObj.SetActive( true );
			highlight.color = color;
		}

		public void DisableHighlight() {
			highlightObj.SetActive( false );
		}
	}
}
