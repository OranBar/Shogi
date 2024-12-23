using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shogi
{
    public class HumanPlayerFX : MonoBehaviour
    {

		[Auto] private HumanPlayer player;
		private ShogiGameSettings Settings => player.shogiGame.settings;

		private List<IHighlightFx> previouslyHighlighted_cells = new List<IHighlightFx>();
		private IHighlightFx previouslyHighlighted_piece;

		void OnEnable()
        {
			player.OnPiece_Selected += PieceSelected;
			// player.OnMoveCell_Selected += CellSelected;
			// player.OnCapturePiece_Selected += CapturePieceSelected;
			player.OnInvalidMove_Selected += InvalidActionSelected;

			player.shogiGame.OnBeforeActionExecuted += Disable_AvailableMoveCells_Highlights;
		}

		void OnDisable(){
			player.OnPiece_Selected -= PieceSelected;
			// player.OnMoveCell_Selected -= CellSelected;
			// player.OnCapturePiece_Selected -= CapturePieceSelected;

			player.OnInvalidMove_Selected -= InvalidActionSelected;
			player.shogiGame.OnBeforeActionExecuted -= Disable_AvailableMoveCells_Highlights;
		}

        void PieceSelected(Piece piece)
        {
			previouslyHighlighted_piece?.DisableHighlight();
			
			if (Settings.highlightAvailableMoves_selectedPiece) {
				Highlight_AvailableMoveCells( piece );
			}

			IHighlightFx piece_highlightFx = piece.GetComponent<IHighlightFx>();
			piece_highlightFx.EnableHighlight( Settings.selectedPiece_color );
			previouslyHighlighted_piece = piece_highlightFx;

		}

		public void Highlight_AvailableMoveCells( Piece piece ) {
			foreach(var cell in previouslyHighlighted_cells){
				cell.DisableHighlight();
			}

			var availableMoves = piece.GetValidMoves();
			IEnumerable<Cell> validCellMoves = availableMoves.Select( m => Cell.GetCell( m.x, m.y) );

			previouslyHighlighted_cells.Clear();
			foreach (var cellToHighlight in validCellMoves) {
				IHighlightFx cellFX = cellToHighlight.GetComponent<IHighlightFx>();
				cellFX.EnableHighlight( Settings.availableMove_selectedPiece_highlightColor );
				previouslyHighlighted_cells.Add( cellFX );
			}
		}

		// private void CellSelected( Cell cell ) {
		// 	player.selectedPiece.GetComponent<IHighlightFx>().DisableHighlight();
		// }

		// private void CapturePieceSelected( Piece piece ) {
		// 	player.selectedPiece.GetComponent<IHighlightFx>().DisableHighlight();
		// }

		private void InvalidActionSelected( AShogiAction _ = null ) {
			previouslyHighlighted_piece?.DisableHighlight();
			Disable_AvailableMoveCells_Highlights(_);
		}

		public void Disable_AvailableMoveCells_Highlights(AShogiAction _ = null){
			foreach (var cell in previouslyHighlighted_cells) {
				cell.DisableHighlight();
			}
		}
	}
}
