using System.Collections.Generic;
using NaughtyAttributes;
using Photon.Pun;

namespace Shogi
{
	public class AnalysisBranchingManager_Multi : AnalysisBranchingManager
	{
		[Auto] PhotonView photonView;


		protected override AnalysisBranch CreateNewBranch() {
			var newBranchObj = PhotonNetwork.Instantiate( newBranchPrefab.name, newBranchPrefab.transform.position, newBranchPrefab.transform.rotation );
			var branch = newBranchObj.GetComponent<AnalysisBranch>();
			return branch;			
		}


		public List<AnalysisBranch> synchedBranches = new List<AnalysisBranch>();
		protected override void HandleHeadDetached(AnalysisEntry entry){
			if(synchedBranches.Contains(currBranch)){
				base.HandleHeadDetached(entry);
				base.ForkSelectedEntry_ToNewBranch(entry.associatedMove);
			} else {
				photonView.RPC(nameof(UpdateDetachedBranch_RPC), RpcTarget.All, entry);
			}
		}

		[PunRPC]
		protected void UpdateDetachedBranch_RPC(AnalysisEntry entry){
			//TODO: Finish serializing the analysisEntry via photonId.
			//Add photonView to AnalysisEntry
			base.HandleHeadDetached(entry);
		}

		public void SyncBranch(AnalysisBranch branch){
			//TODO: Finish serializing the analysisBranch via photonId.
			//Add photonView to AnalysisBranch
			photonView.RPC( nameof( SyncBranch_RPC ), RpcTarget.All, branch );
		}
		
		[PunRPC]
		public void SyncBranch_RPC(AnalysisBranch branch){
			synchedBranches.Add(branch);
		}

		[Button]
		public void UpdatedCurrSyncBranch(){
			SyncBranch( currBranch );
		}


	}
}