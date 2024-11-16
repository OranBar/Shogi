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
		public TMP_InputField branchNameText;

		public RefAction<AnalysisEntry> OnHeadDetached = new RefAction<AnalysisEntry>();

		[NonSerialized] private GameHistory _branchGameHistory;
		
		public GameHistory BranchGameHistory {
			get
			{
				if (_branchGameHistory == null) {
					_branchGameHistory = shogiGame.gameHistory;
				}
				return _branchGameHistory;
			}
			set {
				_branchGameHistory = value;
			}
		}

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
		
		void OnEnable(){
			shogiGame.OnActionExecuted += CreateAndAppend_MoveEntry;

			//TODO: I need to make a RefAction that does not allow duplicates of the same function, so I can stop doing this
			entries.ForEach( e => e.OnEntrySelected -= UpdateCurrentlySelectedEntry );
			entries.ForEach( e => e.OnEntrySelected += UpdateCurrentlySelectedEntry );
		}

		void OnDisable(){
			shogiGame.OnActionExecuted -= CreateAndAppend_MoveEntry;
			entries.ForEach( e => e.OnEntrySelected -= UpdateCurrentlySelectedEntry );
			Logger.Log("[Analysis] Branch deactivated");
		}

		public void CreateAndAppend_MoveEntry( AShogiAction playedMove ) {
			if (playedMove is UndoLastAction) {
				HandleUndoAction();
				return;
			}

			AnalysisEntry newEntry = InstantiateEntry_AndInit( playedMove );

			entries.Add( newEntry );
			newEntry.OnEntrySelected += UpdateCurrentlySelectedEntry;

			currentlySelectedEntry?.DoNormalEffect();
			currentlySelectedEntry = newEntry;
			newEntry.DoSelectedEffect();


			void HandleUndoAction() {
				var entryToUndo = entries.Last();
				entries.Remove( entryToUndo );
				Destroy( entryToUndo.gameObject );

				currentlySelectedEntry = entryToUndo;
				entries.Last()?.DoSelectedEffect();
			}
		}

		protected virtual AnalysisEntry InstantiateEntry_AndInit( AShogiAction playedMove ){
			GameObject newEntryObj = Instantiate( entryPrefab, scrollRect.content );
			AnalysisEntry newEntry = newEntryObj.GetComponent<AnalysisEntry>();
			
			InitEntry(ref newEntry, playedMove);

			Canvas.ForceUpdateCanvases();
			scrollRect.verticalScrollbar.value = -0.1f;
			Canvas.ForceUpdateCanvases();
			
			return newEntryObj.GetComponent<AnalysisEntry>();
		}

		protected void InitEntry( ref AnalysisEntry newEntryObj, AShogiAction playedMove ) {
			AnalysisEntry newEntry = newEntryObj.GetComponent<AnalysisEntry>();
			newEntry.name = newEntry.name.Replace( "Clone", "" + ( entries.Count + 1 ) );

			newEntry.InitEntry( entries.Count + 1, playedMove );
			newEntry.gameState_afterMove = new GameState( shogiGame );
		}

		public async void UpdateCurrentlySelectedEntry(AnalysisEntry entry){
			UpdateUIEffect( entry );
			shogiGame.OnActionExecuted -= CreateAndAppend_MoveEntry;

			bool shouldDetachHead = shogiGame.gameHistory.playedMoves.Count == entry.moveNumber;

			var newGameHistory = BranchGameHistory.Clone( entry.moveNumber );
			Logger.Log("[Analysis] GameHistory Count "+shogiGame.gameHistory.playedMoves.Count);
			//TODO: update timers?

			shogiGame.ApplyGameState( entry.associatedMove.GameState_beforeMove );
			highlightLastMovedPiece?.DoHighlight(entry.associatedMove);
			shogiGame.ApplyGameState( entry.gameState_afterMove );
			await entry.associatedMove.ExecuteAction_FX();



			if(shouldDetachHead){
				Logger.Log("[Analysis] Okay");
				shogiGame.OnActionExecuted += CreateAndAppend_MoveEntry;
			} else {
				Logger.Log("[Analysis] Head detached");
				OnHeadDetached.Invoke( entry );
			}

			shogiGame.BeginGame( newGameHistory.GetPlayer_WhoMovesNext(), newGameHistory );
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
