using System.Linq;
using UnityEngine;

namespace Shogi
{
	public class HighlightLastMovedPiece : MonoBehaviour
	{
		private ShogiGame shogiGame;
		private IHighlightFx prevMovedPiece_highlighter;
		private IHighlightFx prevStartCell_highlighter;

		// private Cell[] cells;

		void Start() {
			shogiGame = FindObjectOfType<ShogiGame>();
			shogiGame.OnActionExecuted += DoHighlightLastMovedPiece;
			shogiGame.OnActionExecuted += DoHighlightStartMoveCell;

			// cells = FindObjectsOfType<Cell>(true);
		}

		public void DoHighlightLastMovedPiece(IShogiAction action){
			switch (action) {
				case UndoLastAction undoAction:
					break;
				case DropPieceAction dropPieceAction:
					DoHighlightLastMovedPiece( dropPieceAction );
					break;
				case MovePieceAction movePieceAction:
					DoHighlightLastMovedPiece( movePieceAction );
					break;
				default:
					break;
			}
		}

		public void DoHighlightStartMoveCell( IShogiAction action ) {
			switch (action) {
				case UndoLastAction undoAction:
					DoHighlightStartMoveCell( undoAction );
					break;
				case DropPieceAction dropPieceAction:
					DoHighlightStartMoveCell( dropPieceAction );
					break;
				case MovePieceAction movePieceAction:
					DoHighlightStartMoveCell( movePieceAction );
					break;
				default:
					break;
			}
		}

		public void DoHighlightLastMovedPiece( DropPieceAction action ) {
			prevMovedPiece_highlighter?.DisableHighlight();

			IHighlightFx pieceHighlight = action.ActingPiece.GetComponent<IHighlightFx>();
			pieceHighlight.EnableHighlight( shogiGame.settings.GetLastMovedPiece_Color( action.PlayerId ) );

			prevMovedPiece_highlighter = pieceHighlight;
		}

		public void DoHighlightLastMovedPiece( MovePieceAction action ) {
			prevMovedPiece_highlighter?.DisableHighlight();

			IHighlightFx pieceHighlight = action.ActingPiece.GetComponent<IHighlightFx>();
			pieceHighlight.EnableHighlight( shogiGame.settings.GetLastMovedPiece_Color( action.PlayerId ) );

			prevMovedPiece_highlighter = pieceHighlight;
		}

		public void DoHighlightStartMoveCell( UndoLastAction action ) {
			if (shogiGame.gameHistory.playedMoves.Count < 1) {
				prevStartCell_highlighter?.DisableHighlight();
			}
		}

		public void DoHighlightStartMoveCell( DropPieceAction action ) {
			prevStartCell_highlighter?.DisableHighlight();
		}

		public void DoHighlightStartMoveCell( MovePieceAction action ) {
			prevStartCell_highlighter?.DisableHighlight();

			Cell startCell = Cell.GetCell( action.StartX, action.StartY );
			IHighlightFx startCell_highlighter = startCell.GetComponent<IHighlightFx>();

			Color highlightCellColor = shogiGame.settings.GetLastMovedPiece_Color( action.PlayerId ).SetAlpha( 0.5f );
			startCell_highlighter.EnableHighlight( highlightCellColor );

			prevStartCell_highlighter = startCell_highlighter;
		}

}
}