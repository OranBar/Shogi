using System.Collections;
using System.Collections.Generic;
using Shogi;
using UnityEngine;

namespace Shogi
{
	public class HighlightLastMovedPiece : MonoBehaviour
	{
		public GameSettings settings;
		private ShogiGame shogiGame;

		private Piece lastMovedPiece;

		void Start() {
			shogiGame = FindObjectOfType<ShogiGame>();
			settings = FindObjectOfType<GameSettings>();
			shogiGame.OnActionExecuted += DoHighlightLastMovedPiece;
		}

		public void DoHighlightLastMovedPiece(IShogiAction action){
			lastMovedPiece?.GetComponent<IPieceHighlight>()?.DisableHighlight();
			action.GetActingPiece().GetComponent<IPieceHighlight>().EnableHighlight(settings.lastMovedPiece_color);
			lastMovedPiece = action.GetActingPiece();
		}

	}
}