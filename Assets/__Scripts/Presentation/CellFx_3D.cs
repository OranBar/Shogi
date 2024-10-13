using System.Collections;
using System.Collections.Generic;
using BarbarO.ExtensionMethods;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
    public class CellFx_3D : MonoBehaviour, IHighlightFx
    {
		[AutoChildren] private MeshRenderer meshRenderer;

		private Color defaultColor;
		private bool isHighlighted = false;
		public bool IsHighlighted {
			get => isHighlighted;
		}


		void Awake() {
			defaultColor = meshRenderer.material.color;
		}

		public async UniTask EnableHighlight( Color color ) {
			if(isHighlighted){ return; }

			meshRenderer.material.color = color;
			isHighlighted = true;
		}

		public async UniTask DisableHighlight() {
			meshRenderer.material.color = defaultColor;
			isHighlighted = false;
		}
	}
}
