using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Photon.Pun;
using UnityEngine;

namespace Shogi
{
    public class Foo : MonoBehaviourPun
    {
		public Piece actingPiece;

		[Button]
		public void RunTest_MovePieceAction(){
			MovePieceAction action = new MovePieceAction( actingPiece, 4, 5 );
			photonView.RPC( nameof( Log_MovePieceAction_RPC ), RpcTarget.All, action );
		}

		[PunRPC]
		public void Log_MovePieceAction_RPC( MovePieceAction action ){
			Debug.Log("Received action: "+action);
		}

		[Button]
		public void RunTest_APieceAction() {
			AShogiAction action = new MovePieceAction( actingPiece, 4, 5 );
			photonView.RPC( nameof( Log_APieceAction_RPC ), RpcTarget.All, action );
		}

		[PunRPC]
		public void Log_APieceAction_RPC( AShogiAction action ) {
			Debug.Log( "Received action: " + action );
		}

		public GameObject piecePrefab;
		[Button]
		public void Test3DSideboard(){

			GetComponent<SideBoard>().AddCapturedPiece( piecePrefab.GetComponent<Piece>() );
		}

		
    }
}
