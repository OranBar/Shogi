using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shogi{

	public class Cell : MonoBehaviour, IPointerDownHandler
	{
		public int x, y;
		public static RefAction<Cell> OnAnyCellClicked = new RefAction<Cell>();
		


		public static Cell GetCell(int x, int y){
			Cell[] cells = FindObjectsOfType<Cell>();
			Cell cell = cells.First( c => c.x == x && c.y == y );
			return cell;
		}

		public void OnPointerDown( PointerEventData eventData ) {
			Cell.OnAnyCellClicked.Invoke( this );
		}

		void OnMouseDown() {
			Cell.OnAnyCellClicked.Invoke( this );
		}

		
	}

}