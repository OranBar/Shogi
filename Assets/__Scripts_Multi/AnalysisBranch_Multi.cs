using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	public class AnalysisBranch_Multi : AnalysisBranch
	{
		[Auto] private PhotonView photonView;
		protected override AnalysisEntry InstantiateEntry_AndInit( AShogiAction playedMove ) {
			GameObject newEntryObj = PhotonNetwork.Instantiate( entryPrefab.name, entryPrefab.transform.position, entryPrefab.transform.rotation );
			newEntryObj.transform.parent = scrollRect.content;
			AnalysisEntry newEntry = newEntryObj.GetComponent<AnalysisEntry>();
			photonView.RPC( nameof(InitEntry_RPC), RpcTarget.All, newEntry, playedMove );

			return newEntry;
		}

		[PunRPC]
		private void InitEntry_RPC(AnalysisEntry newEntry, AShogiAction playedMove){
			InitEntry( ref newEntry, playedMove );
		}
	}

	
}
