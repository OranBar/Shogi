using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shogi{
	public class AnalysisEntry : MonoBehaviour, IPointerUpHandler
	{
		public TMP_Text numberText;
		public TMP_Text moveText;

		private AShogiAction associatedMove;

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

		public void SetEntryText( int moveNumber, AShogiAction associatedMove ) {
			this.associatedMove = associatedMove;

			numberText.text = moveNumber.ToString();
			moveText.text = associatedMove.ToString();
		}

		public void OnPointerUp(PointerEventData data){
			shogiGame.ApplyGameState(associatedMove.GameState_beforeMove);
			DoSelectionEffect();
		}

		private void DoSelectionEffect(){
			
		}
		
	}
}
