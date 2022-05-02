using System.Linq;
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
			if (action is UndoLastAction) { return; }
			
			prevMovedPiece?.DisableHighlight();

			IPieceHighlight pieceHighlight = action.GetActingPiece().GetComponent<IPieceHighlight>();
			pieceHighlight.EnableHighlight( settings.lastMovedPiece_color);

			prevMovedPiece = pieceHighlight;
		}

		public void DoHighlightStartMoveCell(IShogiAction action){
			//I really dislike this if. It's ugly. I wish I could put it inside the IShogiAciton and do something like
			//action.HandleFx();
			//But then my action does BOTH logic AND effects, and I think I should avoid mixing them. 
			if (action is UndoLastAction) { 
				if(shogiGame.gameHistory.playedMoves.Count < 1){
					prevStartMovedCell?.DeactivateHightlight();
				}
				return; 
			}
			//-------------------

			prevStartMovedCell?.DeactivateHightlight();
			if (action is DropPieceAction) { return; }

			Cell startCell = cells.First(c => c.x == action.StartX && c.y == action.StartY);
			CellFx startCellFx = startCell.GetComponent<CellFx>();

			Color highlightCellColor = settings.lastMovedPiece_color.SetAlpha(0.5f);
			startCellFx.ActivateHighlight( highlightCellColor );

			prevStartMovedCell = startCellFx;
		}

	}
}