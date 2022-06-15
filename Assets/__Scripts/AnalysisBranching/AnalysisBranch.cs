using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	public class AnalysisBranch : MonoBehaviour
	{
		public ScrollRect scrollRect;
		public GameObject entryPrefab;
		private ShogiGame shogiGame;
		public AnalysisEntry currentlySelectedEntry = null;
		public TMP_Text branchNameText;

		public RefAction<AnalysisEntry> OnHeadDetached = new RefAction<AnalysisEntry>();

		[NonSerialized] public GameHistory branchGameHistory = null;

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
		private HighlightLastMovedPiece highlightLastMovedPiece;

		void Awake(){
			shogiGame = FindObjectOfType<ShogiGame>();
			highlightLastMovedPiece = FindObjectOfType<HighlightLastMovedPiece>();

		}
		
		void Start(){
			if (branchGameHistory == null) {
				branchGameHistory = shogiGame.gameHistory;
			}
		}

		void OnEnable(){
			shogiGame.OnActionExecuted += CreateAndAppend_MoveEntry;
		}

		void OnDisable(){
			shogiGame.OnActionExecuted -= CreateAndAppend_MoveEntry;
		}

		public void CreateAndAppend_MoveEntry( AShogiAction playedMove ) {
			if(playedMove is UndoLastAction){
				var entryToUndo = entries.Last();
				entries.Remove( entryToUndo );
				Destroy( entryToUndo.gameObject );

				currentlySelectedEntry = entryToUndo;
				entries.Last()?.DoSelectedEffect();
				return;
			}
			GameObject newEntryObj = Instantiate( entryPrefab, scrollRect.content );
			AnalysisEntry newEntry = newEntryObj.GetComponent<AnalysisEntry>();
			newEntry.name = newEntry.name.Replace( "Clone", "" + ( entries.Count + 1 ) );

			newEntry.InitEntry( entries.Count + 1, playedMove );
			newEntry.gameState_afterMove = new GameState( shogiGame );

			entries.Add( newEntry );
			newEntry.OnEntrySelected += UpdateCurrentlySelectedEntry;

			currentlySelectedEntry?.DoNormalEffect();
			currentlySelectedEntry = newEntry;
			newEntry.DoSelectedEffect();
		}
		
		public async void UpdateCurrentlySelectedEntry(AnalysisEntry entry){
			UpdateUIEffect( entry );

			shogiGame.OnActionExecuted -= CreateAndAppend_MoveEntry;

			shogiGame.gameHistory = branchGameHistory.Clone( entry.moveNumber );
			Debug.Log("GameHistory Count "+shogiGame.gameHistory.playedMoves.Count);
			//TODO: update timers?

			shogiGame.ApplyGameState( entry.associatedMove.GameState_beforeMove );
			highlightLastMovedPiece?.DoHighlight(entry.associatedMove);
			await entry.associatedMove.ExecuteAction_FX();

			shogiGame.ApplyGameState( entry.gameState_afterMove );


			if(shogiGame.gameHistory.playedMoves.Count == branchGameHistory.playedMoves.Count){
				Debug.Log("Okay");
				shogiGame.OnActionExecuted += CreateAndAppend_MoveEntry;
			} else {
				Debug.Log("Head detached");
				OnHeadDetached.Invoke( entry );
			}

			shogiGame.BeginGame( shogiGame.gameHistory.GetPlayer_WhoMovesNext() );
		}

		internal void ClearEntries() {
			foreach(var entry in entries){
				Destroy( entry.gameObject );
			}
			entries.Clear();
			currentlySelectedEntry = null;
		}

		private void UpdateUIEffect( AnalysisEntry newSelectedEntry ) {
			currentlySelectedEntry?.DoNormalEffect();
			currentlySelectedEntry = newSelectedEntry;
			newSelectedEntry.DoSelectedEffect();
		}

	}

	
}
