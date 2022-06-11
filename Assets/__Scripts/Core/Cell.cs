using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shogi{

	public class Cell : MonoBehaviour, IPointerDownHandler
	{
		public int x, y;
		public static RefAction<Cell> OnAnyCellClicked = new RefAction<Cell>();
		private static readonly HashSet<Cell> CELLS = new HashSet<Cell>();

		void Awake(){
			CELLS.Add( this );
		}

		void OnDestroy(){
			CELLS.Remove( this );
		}

		public static Cell GetCell( int x, int y ){
			Cell cell = CELLS.First( c => c.x == x && c.y == y );
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