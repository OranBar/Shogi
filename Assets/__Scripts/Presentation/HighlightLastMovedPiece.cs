using System.Linq;
using UnityEngine;

namespace Shogi
{
	//This class should not exist. Or maybe it can only take care of deactivating highlights from previous move?
	public class HighlightLastMovedPiece : MonoBehaviour
	{
		private ShogiGame shogiGame;

		private IHighlightFx prevMovedPiece_highlighter;
		private IHighlightFx prevStartCell_highlighter;

		private Cell[] cells;

		void Start() {
			shogiGame = FindObjectOfType<ShogiGame>();
			// settings = FindObjectOfType<GameSettings>();
			shogiGame.OnActionExecuted += DoHighlightLastMovedPiece;
			shogiGame.OnActionExecuted += DoHighlightStartMoveCell;

			cells = FindObjectsOfType<Cell>(true);
		}

		public void DoHighlightLastMovedPiece(IShogiAction action){
			if (action is UndoLastAction) { return; }
			
			prevMovedPiece_highlighter?.DisableHighlight();

			IHighlightFx pieceHighlight = action.GetActingPiece().GetComponent<IHighlightFx>();
			// pieceHighlight.EnableHighlight( shogiGame.settings.lastMovedPiece_color);

			prevMovedPiece_highlighter = pieceHighlight;
		}

		public void DoHighlightStartMoveCell(IShogiAction action){
			//I really dislike this if. It's ugly. I wish I could put it inside the IShogiAciton and do something like
			//action.HandleFx();
			//But then my action does BOTH logic AND effects, and I think I should avoid mixing them. 
			if (action is UndoLastAction) { 
				if(shogiGame.gameHistory.playedMoves.Count < 1){
					prevStartCell_highlighter?.DisableHighlight();
				}
				return; 
			}
			//-------------------

			prevStartCell_highlighter?.DisableHighlight();
			// if (action is DropPieceAction) { return; }

			Cell startCell = Cell.GetCell( action.StartX, action.StartY );
			IHighlightFx startCell_highlighter = startCell.GetComponent<IHighlightFx>();

			// Color highlightCellColor = shogiGame.settings.lastMovedPiece_color.SetAlpha(0.5f);
			// startCellFx.ActivateHighlight( highlightCellColor );

			prevStartCell_highlighter = startCell_highlighter;
		}

	}
}