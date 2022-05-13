using UnityEngine;

namespace Shogi
{

	public class GameSettings : MonoBehaviour
	{
		public bool playSoundOnMove;
		public Color selectedPiece_color;
		public Color lastMovedPiece_color_player1;
		public Color lastMovedPiece_color_player2;

		public Color GetLastMovedPiece_Color(PlayerId player){
			return player == PlayerId.Player1 ? lastMovedPiece_color_player1 : lastMovedPiece_color_player2;
		}

	}
}