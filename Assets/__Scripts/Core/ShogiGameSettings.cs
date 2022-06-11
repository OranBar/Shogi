using UnityEngine;

namespace Shogi
{
	public class ShogiGameSettings : MonoBehaviour
	{
		public bool playSoundOnMove;
		public Color selectedPiece_color;
		public Color lastMovedPiece_color_player1;
		public Color lastMovedPiece_color_player2;
		public bool highlightAvailableMoves_selectedPiece;
		public bool highlightAvailableMoves_hoverPiece;
		public Color availableMove_selectedPiece_highlightColor;
		public Color availableMove_hoverPiece_highlightColor;

		public Color GetLastMovedPiece_Color(PlayerId player){
			return player == PlayerId.Player1 ? lastMovedPiece_color_player1 : lastMovedPiece_color_player2;
		}

	}
}