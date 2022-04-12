using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shogi{

	public class Cell : MonoBehaviour, IPointerDownHandler
	{
		public int x, y;

		public void OnPointerDown( PointerEventData eventData ) {
			ShogiGame.OnAnyCellClicked.Invoke( this );
		}

		void OnMouseDown() {
			ShogiGame.OnAnyCellClicked.Invoke( this );
		}

	}

}