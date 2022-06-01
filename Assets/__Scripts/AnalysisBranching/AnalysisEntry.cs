using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shogi{
	public class AnalysisEntry : MonoBehaviour, IPointerDownHandler
	{
		public static RefAction<AnalysisEntry> OnEntrySelected = new RefAction<AnalysisEntry>();
		public static AnalysisEntry currentlySelectedEntry = null;

		public TMP_Text numberText;
		public TMP_Text moveText;

		private AShogiAction associatedMove;
		private GameState associatedGameState;

		private ShogiGame shogiGame;

		[ReadOnly] public Color defaultColor;
		public Color highlightColor;
		public Color selectedColor;

		[Auto] private Image image;

		void OnValidate()
		{
			image = GetComponent<Image>();
			if(image != null){
				defaultColor = image.color;
			}
		}

		void Start()
		{
			shogiGame = FindObjectOfType<ShogiGame>();
			defaultColor = image.color;
		}

		public void InitEntry(int moveNumber, AShogiAction associatedMove){
			// associatedGameState = gameState;
			this.associatedMove = associatedMove;
			SetEntryText( moveNumber, associatedMove );
		}

		private void SetEntryText( int moveNumber, AShogiAction associatedMove ) {
			numberText.text = moveNumber.ToString();
			moveText.text = associatedMove?.ToString() ?? "Game Begin";
		}

		public void OnPointerDown(PointerEventData data){
			Debug.Log("MeHere");
			DoSelectionEffect();

			// shogiGame.ApplyGameState( associatedGameState );
			shogiGame.ApplyGameState( associatedMove.GameState_beforeMove );
			shogiGame.ExecuteAction_AndCallEvents( associatedMove );
		}

		private void DoSelectionEffect(){
			AnalysisEntry.currentlySelectedEntry?.DoNormalEffect();
			AnalysisEntry.currentlySelectedEntry = this;

			// AnalysisEntry.OnEntrySelected.Invoke( this );
			image.color = selectedColor;
		}

		private void DoNormalEffect() {
			image.color = defaultColor;
		}
		
	}
}
