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

		void Start()
		{
			if(currBranch != null){
				EnableBranch( currBranch );
			}
		}

		void OnEnable(){
			prevBranch_btn.onClick.AddListener( GoToPreviousBranching );
			nextBranch_btn.onClick.AddListener( GoToNextBranching );
			createNewBranch_btn.onClick.AddListener( CreateNewBranch );
		}

		void OnDisable(){
			prevBranch_btn.onClick.RemoveListener( GoToPreviousBranching );
			nextBranch_btn.onClick.RemoveListener( GoToNextBranching );
			createNewBranch_btn.onClick.RemoveListener( CreateNewBranch );
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

			branchToEnable.currentlySelectedEntry?.SelectEntry();

			//TODO: update game history
			// var entriesToCarryOver = currBranch.entries.Take( entryIndex );
			// foreach(var entry in entriesToCarryOver){
			// 	branchToEnable.CreateAndAppend_MoveEntry( entry.associatedMove, entry.moveNumber );
			// }

			// var selectedEntry = branchToEnable.entries.Last();
			// selectedEntry.SelectEntry();

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
	}
}