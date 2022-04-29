using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
	public class PieceHighlight : MonoBehaviour, IPieceHighlight
	{
		public GameObject highlightObj;

		void Start() {
			highlightObj.SetActive( false );
		}

		public void SetHighlight( bool enable ) {
			highlightObj.SetActive( enable );
		}
	}
}
