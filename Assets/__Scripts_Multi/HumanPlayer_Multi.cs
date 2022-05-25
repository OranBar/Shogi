using System.Collections;
using System.Collections.Generic;
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

			photonView.RPC( nameof( RegisterPlayer_ToShogiGame_RPC ), RpcTarget.All );
		}

		[PunRPC]
		public void RegisterPlayer_ToShogiGame_RPC() {
			ShogiGame shogiGame = FindObjectOfType<ShogiGame>();
			if (shogiGame.Player1 == null) {
				shogiGame.Player1 = GetComponent<APlayer>();
				this.PlayerId = PlayerId.Player1;
			} else if (shogiGame.Player2 == null) {
				shogiGame.Player2 = GetComponent<APlayer>();
				this.PlayerId = PlayerId.Player2;

				shogiGame.BeginGame(PlayerId.Player1);
			} else {
				Debug.LogError( "Do we have 3 players? What's going on" );
			}
		}
	}
}
