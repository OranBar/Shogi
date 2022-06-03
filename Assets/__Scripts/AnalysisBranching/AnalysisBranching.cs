using System.Collections.Generic;
using System.Linq;
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

		public void CreateAndAppend_MoveEntry(AShogiAction lastMove) { 
			GameObject newEntryObj = Instantiate( entryPrefab, scrollRect.content );
			AnalysisEntry newEntry = newEntryObj.GetComponent<AnalysisEntry>();
			
			newEntry.InitEntry( shogiGame.TurnCount, lastMove );
			
			entries.Add( newEntry );
			newEntry.OnEntrySelected += UpdateCurrentlySelectedEntry;
		}

		public void CreateAndAppend_MoveEntry( AShogiAction lastMove, int turnCount ) {
			GameObject newEntryObj = Instantiate( entryPrefab, scrollRect.content );
			AnalysisEntry newEntry = newEntryObj.GetComponent<AnalysisEntry>();

			newEntry.InitEntry( turnCount, lastMove );

			entries.Add( newEntry );
			newEntry.OnEntrySelected += UpdateCurrentlySelectedEntry;
		}

		public async void UpdateCurrentlySelectedEntry(AnalysisEntry entry){
			currentlySelectedEntry?.DoNormalEffect();
			currentlySelectedEntry = entry;

			shogiGame.OnActionExecuted -= CreateAndAppend_MoveEntry;

			shogiGame.ApplyGameState( entry.associatedMove.GameState_beforeMove );
			await shogiGame.ExecuteAction_AndCallEvents( entry.associatedMove );

			shogiGame.OnActionExecuted += CreateAndAppend_MoveEntry;

		}
	}

	
}
