using System.Linq;
using UnityEngine;

namespace Shogi
{
	public class HighlightLastMovedPiece : MonoBehaviour
	{
		private ShogiGame shogiGame;
		private IHighlightFx prevMovedPiece_highlighter;
		private IHighlightFx prevStartCell_highlighter;


		void Start() {
			shogiGame = FindObjectOfType<ShogiGame>();
			shogiGame.OnBeforeActionExecuted += DoHighlightLastMovedPiece;
			shogiGame.OnBeforeActionExecuted += DoHighlightStartMoveCell;
		}

		public void DoHighlightLastMovedPiece(AShogiAction action){
			switch (action) {
				case UndoLastAction undoAction:
					DoHighlightLastMovedPiece( undoAction );
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

		public void DoHighlightStartMoveCell( AShogiAction action ) {
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

		public void DoHighlightLastMovedPiece( UndoLastAction action ) {
			prevMovedPiece_highlighter?.DisableHighlight();
			
			if(shogiGame.gameHistory.playedMoves.Count >= 2){
				AShogiAction actionBefore_undoneAction = shogiGame.gameHistory.playedMoves[^2];
				DoHighlightLastMovedPiece( actionBefore_undoneAction );
			}
		}

		public void DoHighlightStartMoveCell( UndoLastAction action ) {
			prevStartCell_highlighter?.DisableHighlight();
			
			if (shogiGame.gameHistory.playedMoves.Count >= 2) {
				AShogiAction actionBefore_undoneAction = shogiGame.gameHistory.playedMoves [^2];
				DoHighlightStartMoveCell( actionBefore_undoneAction );
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