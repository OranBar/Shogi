using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shogi;
using UnityEngine;

namespace Shogi
{
	public class HighlightLastMovedPiece : MonoBehaviour
	{
		public GameSettings settings;
		private ShogiGame shogiGame;

		private IPieceHighlight prevMovedPiece;
		private CellFx prevStartMovedCell;

		private Cell[] cells;

		void Start() {
			shogiGame = FindObjectOfType<ShogiGame>();
			settings = FindObjectOfType<GameSettings>();
			shogiGame.OnActionExecuted += DoHighlightLastMovedPiece;
			shogiGame.OnActionExecuted += DoHighlightStartMoveCell;

			cells = FindObjectsOfType<Cell>(true);
		}

		public void DoHighlightLastMovedPiece(IShogiAction action){
			prevMovedPiece?.DisableHighlight();

			IPieceHighlight pieceHighlight = action.GetActingPiece().GetComponent<IPieceHighlight>();
			pieceHighlight.EnableHighlight( settings.lastMovedPiece_color);

			prevMovedPiece = pieceHighlight;

		}

		public void DoHighlightStartMoveCell(IShogiAction action){
			prevStartMovedCell?.DeactivateHightlight();

			Cell startCell = cells.First(c => c.x == action.StartX && c.y == action.StartY);
			CellFx startCellFx = startCell.GetComponent<CellFx>();

			Color highlightCellColor = settings.lastMovedPiece_color.SetAlpha(0.5f);
			startCellFx.ActivateHighlight( highlightCellColor );

			prevStartMovedCell = startCellFx;
		}

	}
}