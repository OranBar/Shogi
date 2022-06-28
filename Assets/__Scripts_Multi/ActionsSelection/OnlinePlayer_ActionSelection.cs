using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	[RequireComponent(typeof(LocalPlayer_ActionSelection))]
	public class OnlinePlayer_ActionSelection : AActionSelectionStrategy 
	{
		[Auto] private PhotonView photonView;
		[Auto] private LocalPlayer_ActionSelection localPlayerActionSelection;
		private AShogiAction currAction;
		private bool actionReady;

		public async override UniTask<AShogiAction> RequestAction() {
			if (photonView.IsMine) {
				var chosenAction = await localPlayerActionSelection.RequestAction();
				//TODO: If we're in the local deatached analysis, we don't want to send RPCs to the other user
				photonView.RPC( nameof( SendMove_ToOpponent_RPC ), RpcTarget.Others, chosenAction );
				return chosenAction;
			} else {
				//TODO: If we're in local detached analysis, we want to do base.RequestAction to allow for the player to play the opponent's moves
				currAction = null;
				await UniTask.WaitUntil( () => currAction != null );
				return currAction;
			}
		}

		[PunRPC]
		private void SendMove_ToOpponent_RPC( AShogiAction chosenAction ) {
			currAction = chosenAction;
			actionReady = true;
		}
	}
}