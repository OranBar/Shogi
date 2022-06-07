using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shogi{

	public class HighlightMoves_OnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
	{
		[Auto] private Piece piece;
		private ShogiGameSettings settings;
		private List<Cell> highlightedCells = new List<Cell>();

		void Awake()
		{
			settings = FindObjectOfType<ShogiGameSettings>();
			ShogiGame shogiGame = FindObjectOfType<ShogiGame>();
			shogiGame.OnBeforeActionExecuted += DisableSelf;
			shogiGame.OnActionExecuted += EnableSelf;
		}

		private List<Cell> GetMovesCells() {
			// return piece.GetValidMoves().Select( m => Cell.GetCell( m.x, m.y ) ).ToList();
			return piece.MovementStrategy.GetAvailableMoves(piece.X, piece.Y).Select( m => Cell.GetCell(m.x, m.y) ).ToList();
		}

		public void OnPointerEnter( PointerEventData eventData ) {
			Debug.Log("Pointer Enter");
			if (settings.highlightAvailableMoves_hoverPiece == false) { return; }

			List<Cell> availableMoveCells = GetMovesCells();
			foreach(var cell in availableMoveCells)	{
				cell.GetComponent<IHighlightFx>().EnableHighlight(settings.availableMove_hoverPiece_highlightColor);
			}
			highlightedCells = availableMoveCells;
		}
		

		public void OnPointerExit( PointerEventData eventData ) {
			Debug.Log( "Pointer Exit" );

			if (settings.highlightAvailableMoves_hoverPiece == false) { return; }

			DisableCellsHighlight();
		}

		private void DisableCellsHighlight() {
			foreach (var cell in highlightedCells) {
				cell.GetComponent<IHighlightFx>().DisableHighlight();
			}
		}

		private void DisableCellsHighlight(AShogiAction action) {
			DisableCellsHighlight();
		}

		private void DisableSelf( AShogiAction action ) {
			this.enabled = false;
			DisableCellsHighlight();
		}

		private void EnableSelf( AShogiAction action ) {
			this.enabled = true;
		}

		public void OnPointerDown( PointerEventData eventData ) {
			OnPointerExit( eventData );
		}
	}
}