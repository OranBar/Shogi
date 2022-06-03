using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi{
	public class AnalysisBranchingManager : MonoBehaviour
	{
		private List<AnalysisBranching> branches = new List<AnalysisBranching>();
		private AnalysisBranching currBranch;

		public void CreateNewBranch() {
			var currBranchClone = Instantiate( currBranch.gameObject );
			EnableBranch( currBranchClone.GetComponent<AnalysisBranching>() );
		}

		public void EnableBranch( AnalysisBranching branchToEnable ) {
			if (branches.Contains( branchToEnable ) == false) {
				branches.Add( branchToEnable );
			}
			EnableBranch( branches.IndexOf( branchToEnable ) );
		}

		public void EnableBranch( int index ) {
			currBranch?.gameObject?.SetActive( false );
			branches [index].gameObject.SetActive( true );
			currBranch = branches [index];

			//TODO: update game history
		}

	}
}