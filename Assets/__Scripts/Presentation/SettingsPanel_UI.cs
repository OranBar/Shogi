using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

		void Start() {
			settings = FindObjectOfType<ShogiGameSettings>();
			playSoundOnMove.isOn = settings.playSoundOnMove;
			highlightAvailableMoves.isOn = settings.highlightAvailableMoves_selectedPiece;

			playSoundOnMove.onValueChanged.AddListener(val => settings.playSoundOnMove = val);
			highlightAvailableMoves.onValueChanged.AddListener( val => settings.highlightAvailableMoves_selectedPiece = val );

			openPanel_btn.onClick.AddListener( ()=>panelAnimator.SetBool( "IsOpen", true ) );
			closePanel_btn.onClick.AddListener( () => panelAnimator.SetBool( "IsOpen", false ) );
		}
	}
}