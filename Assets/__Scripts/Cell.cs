using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shogi{

	public class Cell : MonoBehaviour, IPointerDownHandler
	{
		public int x, y;
		public static RefAction<Cell> OnAnyCellClicked = new RefAction<Cell>();

		public void OnPointerDown( PointerEventData eventData ) {
			Cell.OnAnyCellClicked.Invoke( this );
		}

		void OnMouseDown() {
			Cell.OnAnyCellClicked.Invoke( this );
		}

	}

}