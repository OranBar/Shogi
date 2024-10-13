using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace Shogi
{
	public class SettingsPanel_UI : MonoBehaviour
	{
		public Toggle playSoundOnMove;
		public Toggle highlightAvailableMoves;
		public Button openPanel_btn;
		public Button closePanel_btn;
		[AutoChildren] Animator panelAnimator;

		private ShogiGameSettings settings;
        private HighlightLastMovedPiece lastmove_highlighter;

        void Start() {
			settings = FindObjectOfType<ShogiGameSettings>();
			lastmove_highlighter = FindObjectOfType<HighlightLastMovedPiece>();
            ShogiGame shogiGame = FindObjectOfType<ShogiGame>();

			playSoundOnMove.isOn = settings.playSoundOnMove;
			highlightAvailableMoves.isOn = settings.highlightAvailableMoves_selectedPiece;

			playSoundOnMove.onValueChanged.AddListener(val => settings.playSoundOnMove = val);
			highlightAvailableMoves.onValueChanged.AddListener( 
				val => {
					settings.highlightAvailableMoves_selectedPiece = val;
					settings.highlightAvailableMoves_hoverPiece = val;
					
					if (val == false){
						// Disable highlights if a piece was selected
						shogiGame.CurrTurn_Player.GetComponent<HumanPlayerFX>().Disable_AvailableMoveCells_Highlights();
					}
				}
			);

			openPanel_btn.onClick.AddListener( ()=>panelAnimator.SetBool( "IsOpen", true ) );
			closePanel_btn.onClick.AddListener( () => panelAnimator.SetBool( "IsOpen", false ) );
		}
	}
}