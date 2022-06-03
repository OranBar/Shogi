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
			// entries = GetComponentsInChildren<AnalysisEntry>()
			// 	.OrderBy(entry => entry.moveNumber)
			// 	.ToList();

			// foreach(var entry in entries){
			// 	entry.OnEntrySelected = new RefAction<AnalysisEntry>();
			// 	entry.OnEntrySelected += UpdateCurrentlySelectedEntry;
			// }
			
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

		public void UpdateCurrentlySelectedEntry(AnalysisEntry entry){
			currentlySelectedEntry?.DoNormalEffect();
			currentlySelectedEntry = entry;
		}
	}

	
}
