using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	public class AnalysisBranching : MonoBehaviour
	{
		public ScrollRect scrollRect;
		public GameObject entryPrefab;
		private ShogiGame shogiGame;
		public AnalysisEntry currentlySelectedEntry = null;
		public TMP_Text branchNameText;

		public string BranchName{
			get{
				return this.name;
			}
			set{
				this.name = value;
				branchNameText.text = value;
			}
		}

		public List<AnalysisEntry> entries = new List<AnalysisEntry>();

		void Awake(){
			shogiGame = FindObjectOfType<ShogiGame>();
		}

		void OnEnable(){
			shogiGame.OnActionExecuted += CreateAndAppend_MoveEntry;
		}

		void OnDisable(){
			shogiGame.OnActionExecuted -= CreateAndAppend_MoveEntry;
		}

		public void CreateAndAppend_MoveEntry( AShogiAction lastMove ) {
			CreateAndAppend_MoveEntry(lastMove, shogiGame.TurnCount);
		}

		public void CreateAndAppend_MoveEntry( AShogiAction lastMove, int turnCount ) {
			GameObject newEntryObj = Instantiate( entryPrefab, scrollRect.content );
			AnalysisEntry newEntry = newEntryObj.GetComponent<AnalysisEntry>();
			newEntry.name = newEntry.name.Replace( "Clone", "" + ( entries.Count + 1 ) );

			newEntry.InitEntry( turnCount, lastMove );

			entries.Add( newEntry );
			newEntry.OnEntrySelected += UpdateCurrentlySelectedEntry;
			
			currentlySelectedEntry?.DoNormalEffect();
			currentlySelectedEntry = newEntry;
			newEntry.DoSelectedEffect();
		}
		
		public async void UpdateCurrentlySelectedEntry(AnalysisEntry entry){
			UpdateUIEffect( entry );

			shogiGame.OnActionExecuted -= CreateAndAppend_MoveEntry;

			shogiGame.ApplyGameState( entry.associatedMove.GameState_beforeMove );
			await shogiGame.ExecuteAction_AndCallEvents( entry.associatedMove );

			shogiGame.OnActionExecuted += CreateAndAppend_MoveEntry;

		}

		private void UpdateUIEffect( AnalysisEntry newSelectedEntry ) {
			currentlySelectedEntry?.DoNormalEffect();
			currentlySelectedEntry = newSelectedEntry;
			newSelectedEntry.DoSelectedEffect();
		}

	}

	
}
