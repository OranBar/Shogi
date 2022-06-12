using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shogi{
	public class AnalysisEntry : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public RefAction<AnalysisEntry> OnEntrySelected = new RefAction<AnalysisEntry>();


		public TMP_Text numberText;
		public TMP_Text moveText;

		[ReadOnly] public AShogiAction associatedMove;
		[ReadOnly] public GameState gameState_afterMove;
		[ReadOnly] public int moveNumber;
		[ReadOnly] public Color defaultColor;
		public Color highlightColor;
		public Color selectedColor;

		[AutoParent] private AnalysisBranch analysisManager;
		[Auto] public Image myImage;
		
		private ShogiGame shogiGame;

		void OnValidate()
		{
			if(Application.isPlaying){
				return;
			}
			myImage = GetComponent<Image>();
			if(myImage != null){
				defaultColor = myImage.color;
			}
		}

		void Start()
		{
			shogiGame = FindObjectOfType<ShogiGame>();
		}

		public void InitEntry(int moveNumber, AShogiAction associatedMove){
			this.associatedMove = associatedMove;
			this.moveNumber = moveNumber;
			SetEntryText( moveNumber, associatedMove );
		}

		private void SetEntryText( int moveNumber, AShogiAction associatedMove ) {
			numberText.text = moveNumber.ToString();
			moveText.text = associatedMove?.ToString() ?? "Game Begin";
		}

		public void OnPointerDown(PointerEventData data){
			
		}

		public void SelectEntry(){
			OnEntrySelected.Invoke( this );
		}

		public void DoSelectedEffect() {
			myImage.color = selectedColor;
		}

		public void DoNormalEffect() {
			myImage.color = defaultColor;
		}

		public void OnPointerUp( PointerEventData eventData ) {
			if(eventData.dragging == false){
				SelectEntry();
			}
		}
	}
}
