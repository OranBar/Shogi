using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi{
	public class AnalysisBranchingManager : MonoBehaviour
	{
		private List<AnalysisBranching> branches = new List<AnalysisBranching>();
		public AnalysisBranching currBranch;

		public Transform branchesContainer;

		public GameObject newBranchPrefab;
		public Button prevBranch_btn;
		public Button nextBranch_btn;
		public Button createNewBranch_btn;
		public Button deleteBranch_btn;

		private ShogiGame shogiGame;

		void Awake(){
			shogiGame = FindObjectOfType<ShogiGame>();
		}

		void Start()
		{
			if(currBranch != null){
				branches.Add( currBranch );
			}
		}

		void OnEnable(){
			prevBranch_btn.onClick.AddListener( GoToPreviousBranching );
			nextBranch_btn.onClick.AddListener( GoToNextBranching );
			createNewBranch_btn.onClick.AddListener( CreateNewBranch );
			deleteBranch_btn.onClick.AddListener( DeleteCurrentBranch );
		}

		void OnDisable(){
			prevBranch_btn.onClick.RemoveListener( GoToPreviousBranching );
			nextBranch_btn.onClick.RemoveListener( GoToNextBranching );
			createNewBranch_btn.onClick.RemoveListener( CreateNewBranch );
			deleteBranch_btn.onClick.RemoveListener( DeleteCurrentBranch );
		}

		[Button]
		public void CreateNewBranch() {
			var newBranchObj = Instantiate( newBranchPrefab, branchesContainer );
			
			AnalysisBranching branch = CopyCurrBranch_UpToCurrSelectedEntry( newBranchObj );
			branch.BranchName = "Branch " + ( branchesContainer.childCount - 1 ); ;

			EnableBranch( branch );
		}

		private AnalysisBranching CopyCurrBranch_UpToCurrSelectedEntry( GameObject newBranchObj ) {
			AnalysisBranching branch = newBranchObj.GetComponent<AnalysisBranching>();

			int entryIndex = currBranch.currentlySelectedEntry.moveNumber;

			var entriesToCarryOver = currBranch.entries.Take( entryIndex );
			foreach (var entry in entriesToCarryOver) {
				branch.CreateAndAppend_MoveEntry( entry.associatedMove, entry.moveNumber );
			}

			//Copy game history
			GameHistory trimmedGameHistory = currBranch.branchGameHistory.Clone( entryIndex );
			branch.branchGameHistory = trimmedGameHistory;

			branch.currentlySelectedEntry = branch.entries.Last();
			return branch;
		}

		public void EnableBranch( AnalysisBranching branchToEnable ) {
			if (branches.Contains( branchToEnable ) == false) {
				branches.Add( branchToEnable );
			}
			EnableBranch( branches.IndexOf( branchToEnable ) );
		}

		public void EnableBranch( int index ) {
			var branchToEnable = branches [index];

			currBranch?.gameObject?.SetActive( false );
			branchToEnable.gameObject.SetActive( true );


			//TODO: update game history
			shogiGame.gameHistory = branchToEnable.branchGameHistory;
			//Restart the game from this turn

			shogiGame.BeginGame( branchToEnable.branchGameHistory.GetPlayer_WhoMovesNext() );

			branchToEnable.currentlySelectedEntry?.SelectEntry();
			currBranch = branchToEnable;
		}

		public void GoToNextBranching(){
			int index = branches.IndexOf( currBranch ) + 1;
			if(index < 0 || index >= branches.Count){
				return;
			}

			EnableBranch( index );
		}

		public void GoToPreviousBranching(){
			int index = branches.IndexOf( currBranch ) - 1;
			if (index < 0 || index >= branches.Count) {
				return;
			}

			EnableBranch( index );
		}

		public void DeleteCurrentBranch(){
			if(branches.Count == 1){
				Debug.LogError("NO! You can't delete ALL of the branches. That would be genocide. Leave at least 1 alive please");
				return;
			}

			int currBranchIndex = branches.IndexOf( currBranch );
			branches.RemoveAt( currBranchIndex );
			Destroy( currBranch.gameObject );

			EnableBranch( currBranchIndex - 1);
		}
	}
}