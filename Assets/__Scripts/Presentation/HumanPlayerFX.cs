using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shogi
{
    public class HumanPlayerFX : MonoBehaviour
    {

		[Auto] private HumanPlayer player;
		private ShogiGameSettings Settings => player.shogiGame.settings;

		private List<CellFx> previouslyHighlighted_cells = new List<CellFx>();
		private IHighlightFx previouslyHighlighted_piece;

		void Start()
        {
			player.OnPiece_Selected += PieceSelected;
			player.OnMoveCell_Selected += CellSelected;
			player.OnCapturePiece_Selected += CapturePieceSelected;

			player.shogiGame.OnBeforeActionExecuted += DisableAllHighlights;
		}

        void PieceSelected(Piece piece)
        {
			previouslyHighlighted_piece?.DisableHighlight();
			
			if (Settings.highlightAvailableMoves) {
				HighlightAvailable_MoveCells( piece );
			}

			IHighlightFx piece_highlightFx = piece.GetComponent<IHighlightFx>();
			piece_highlightFx.EnableHighlight( Settings.selectedPiece_color );
			previouslyHighlighted_piece = piece_highlightFx;

		}

		private void HighlightAvailable_MoveCells( Piece piece ) {
			foreach(var cell in previouslyHighlighted_cells){
				cell.DisableHighlight();
			}

			var availableMoves = piece.GetValidMoves();
			Cell [] cells = GameObject.FindObjectsOfType<Cell>();
			IEnumerable<Cell> validCellMoves = availableMoves.Select( m => Cell.GetCell( m.x, m.y, cells ) );

			previouslyHighlighted_cells.Clear();
			foreach (var cellToHighlight in validCellMoves) {
				CellFx cellFX = cellToHighlight.GetComponent<CellFx>();
				cellFX.EnableHighlight( Settings.availableMove_HighlightColor );
				previouslyHighlighted_cells.Add( cellFX );
			}
		}

		private void CellSelected( Cell cell ) {
			player.selectedPiece.GetComponent<IHighlightFx>().DisableHighlight();
		}

		private void CapturePieceSelected( Piece piece ) {
			player.selectedPiece.GetComponent<IHighlightFx>().DisableHighlight();
		}

		private void DisableAllHighlights(IShogiAction _){
			previouslyHighlighted_piece?.DisableHighlight();
			foreach (var cell in previouslyHighlighted_cells) {
				cell.DisableHighlight();
			}
		}
	}
}
