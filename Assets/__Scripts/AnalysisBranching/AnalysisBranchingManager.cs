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

		public GameObject newBranchPrefab;

		[Button]
		public void CreateNewBranch() {
			var newBranchObj = Instantiate( newBranchPrefab );
			EnableBranch( newBranchObj.GetComponent<AnalysisBranching>() );
		}

		public void EnableBranch( AnalysisBranching branchToEnable ) {
			if (branches.Contains( branchToEnable ) == false) {
				branches.Add( branchToEnable );
			}
			EnableBranch( branches.IndexOf( branchToEnable ) );
		}

		public void EnableBranch( int index ) {
			int entryIndex = currBranch.currentlySelectedEntry.moveNumber;
			var branchToEnable = branches [index];

			currBranch?.gameObject?.SetActive( false );
			branchToEnable.gameObject.SetActive( true );
			
			//TODO: update game history
			// var entriesToDelete = branchToEnable.entries.Skip( entryIndex );
			// foreach(var toDelete in entriesToDelete){
			// 	Destroy( toDelete.gameObject );
			// }
			var entriesToCarryOver = currBranch.entries.Take( entryIndex );
			foreach(var entry in entriesToCarryOver){
				branchToEnable.CreateAndAppend_MoveEntry( entry.associatedMove, entry.moveNumber );
			}

			var selectedEntry = branchToEnable.entries.Last();
			selectedEntry.SelectEntry();
			branchToEnable.UpdateCurrentlySelectedEntry( selectedEntry );

			currBranch = branchToEnable;
		}
	}
}