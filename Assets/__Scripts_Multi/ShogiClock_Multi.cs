using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Shogi
{

	public class ShogiClock_Multi : ShogiClock
	{
		[Auto] private PhotonView photonView;
		public override void Pause() {
			photonView.RPC( nameof( Pause_RPC ), RpcTarget.AllBuffered );
		}
		public override void Unpause() {
			photonView.RPC( nameof( Unpause_RPC ), RpcTarget.AllBuffered );
		}

		[PunRPC]
		private void Pause_RPC(){
			base.Pause();
		}

		[PunRPC]
		private void Unpause_RPC() {
			base.Unpause();
		}


	}
}