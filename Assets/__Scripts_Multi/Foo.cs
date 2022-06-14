using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

		[Button]
		private void GetResults(  )
		{
			string input = "ro";
			List<string> mockData = new List<string>() 
			{ 
			"la rosa (nom.)", "della rosa", "alla rosa", "la rosa (acc.)", "oh, rosa", "con la rosa", "le rose (nom.)", "delle rose", "alle rose", "le rose (acc.)", "con le rose"

			};
			var result = mockData.Where(str => str.Substring(0, input.Length).IndexOf(input) >= 0).ToList();
			Debug.Log( result.Count );
			//return mockData.FindAll( (str) => str.IndexOf( input.ToLower() ) >= 0 );
		}

		
    }
}
