using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shogi
{
	public class SettingsPanel_UI : MonoBehaviour, IPointerDownHandler
	{
		public Toggle playSoundOnMove;
		public Toggle highlightAvailableMoves;
		public Button openPanel_btn;
		public Button closePanel_btn;
		[AutoChildren] Animator panelAnimator;

		private ShogiGameSettings settings;

		public void OnPointerDown( PointerEventData eventData ) {
			Debug.Log("click");
		}

		void Start() {
			settings = FindObjectOfType<ShogiGameSettings>();
			playSoundOnMove.isOn = settings.playSoundOnMove;
			highlightAvailableMoves.isOn = settings.highlightAvailableMoves;

			playSoundOnMove.onValueChanged.AddListener(val => settings.playSoundOnMove = val);
			highlightAvailableMoves.onValueChanged.AddListener( val => settings.highlightAvailableMoves = val );

			openPanel_btn.onClick.AddListener( ()=>panelAnimator.SetBool( "IsOpen", true ) );
			closePanel_btn.onClick.AddListener( () => panelAnimator.SetBool( "IsOpen", false ) );
		}
	}
}