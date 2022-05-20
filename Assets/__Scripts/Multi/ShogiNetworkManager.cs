using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Shogi
{
    public class ShogiNetworkManager : NetworkManager
    {
		public Transform playerParent;
		private ShogiGame shogiGame;

		public override void Start()
		{
			base.Start();
			shogiGame = FindObjectOfType<ShogiGame>( true );
		}

		public override void OnServerAddPlayer( NetworkConnectionToClient conn ) {
			GameObject player = Instantiate( playerPrefab, playerParent );

			// instantiating a "Player" prefab gives it the name "Player(clone)"
			// => appending the connectionId is WAY more useful for debugging!
			player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
			NetworkServer.AddPlayerForConnection( conn, player );
			
			//Assing PlayerId
			AddPlayerReference_ToShogiGame( player );
		}

		//This needs to happen on all pcs
		void AddPlayerReference_ToShogiGame(GameObject player){	
			if(shogiGame.Player1 == null){
				shogiGame.Player1 = player.GetComponent<APlayer>();
			} else {
				shogiGame.Player2 = player.GetComponent<APlayer>();
				//Start game
				Rpc_StartGame();
			}
		}

		// [ClientRpc]
		public void Rpc_StartGame(){
			shogiGame.gameObject.SetActive( true );
			Debug.Log("Multiplayer ShogiGame started");
		}
    }
}
