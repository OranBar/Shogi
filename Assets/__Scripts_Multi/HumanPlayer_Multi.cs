using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace Shogi
{
    public class HumanPlayer_Multi : HumanPlayer
    {
		[Auto] private PhotonView photonView;

		IEnumerator Start(){
			if ( photonView.IsMine == false ) { yield break; }

			HumanPlayer_Multi [] players;
			do {
				players = FindObjectsOfType<HumanPlayer_Multi>();
				yield return new WaitForSeconds( 0.5f );
			} while (players.Length != 2);

			var myPlayerId = PhotonNetwork.IsMasterClient ? PlayerId.Player1 : PlayerId.Player2;
			photonView.RPC( nameof( RegisterPlayer_ToShogiGame_RPC ), RpcTarget.AllBuffered, myPlayerId );
		}

		[PunRPC]
		public void RegisterPlayer_ToShogiGame_RPC(PlayerId playerId) {
			ShogiGame shogiGame = FindObjectOfType<ShogiGame>();

			this.PlayerId = playerId;
			if(playerId == PlayerId.Player1){
				shogiGame.Player1 = GetComponent<APlayer>();
			} else {
				shogiGame.Player2 = GetComponent<APlayer>();
			}
		}

		public async override UniTask<IShogiAction> RequestAction() {
			if(photonView.IsMine){
				var chosenAction = await base.RequestAction();
				photonView.RPC( nameof(SendMove_ToOpponent_RPC) , RpcTarget.Others, chosenAction);
				return chosenAction;
			} else {
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
