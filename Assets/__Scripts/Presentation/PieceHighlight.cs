using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	public class PieceHighlight : MonoBehaviour, IHighlightFx
	{
		public GameObject highlightObj;

		private Image highlight;
		private bool isHighlighted = false;
		public bool IsHighlighted {
			get => isHighlighted;
		}


		void Start() {
			highlight = highlightObj.GetComponent<Image>();
			highlightObj.SetActive( false );
		}

		public async UniTask EnableHighlight( Color color ) {
			highlightObj.SetActive( true );
			highlight.color = color;
			isHighlighted = true;
		}

		public async UniTask DisableHighlight() {
			highlightObj.SetActive( false );
			isHighlighted = false;
		}
	}
}
