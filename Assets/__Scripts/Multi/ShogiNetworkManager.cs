using System.Collections;
using System.Collections.Generic;
using BarbarO.ExtensionMethods;
using Mirror;
using UnityEngine;

namespace Shogi
{
	public struct StartGameMessage : NetworkMessage{

	}	


    public class ShogiNetworkManager : NetworkManager
    {
		public Transform playerParent;
		private ShogiGame shogiGame;

		private List<APlayer> players = new List<APlayer>();

		public override void Start()
		{
			base.Start();
			shogiGame = FindObjectOfType<ShogiGame>( true );
		}

		public override void OnServerAddPlayer( NetworkConnectionToClient conn ) {
			Debug.Log("MeHere");
			GameObject playerGO = Instantiate( playerPrefab, playerParent );
			APlayer player = playerGO.GetComponent<APlayer>();

			// instantiating a "Player" prefab gives it the name "Player(clone)"
			// => appending the connectionId is WAY more useful for debugging!
			playerGO.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
			NetworkServer.AddPlayerForConnection( conn, playerGO );

			//Assing PlayerId
			players.Add( player );
			if(players.Count == 1){
				player.PlayerId = PlayerId.Player1;
			}
			else if(players.Count == 2){
				player.PlayerId = PlayerId.Player2;
				shogiGame.Player1 = players[0];
				shogiGame.Player2 = player;
				//Okay, everybody should be here. Let's go. 
				// this.ExecuteDelayed( ()=> shogiGame.BeginGame(PlayerId.Player1), 1f);
			}
			// StartCoroutine(AddPlayerReference_ToShogiGame( playerGO ));
		}

		//This needs to happen on all pcs
		// IEnumerator AddPlayerReference_ToShogiGame(GameObject player){
		// 	yield return new WaitForSeconds( 2f );
		// 	if(shogiGame.Player1 == null){
		// 		shogiGame.Player1 = player.GetComponent<APlayer>();
		// 	} else {
		// 		shogiGame.Player2 = player.GetComponent<APlayer>();
		// 		//Start game
		// 		// Rpc_StartGame();
		// 		// NetworkServer.SendToAll( new StartGameMessage() );
		// 	}
		// }

		// [ClientRpc]
		public void Rpc_StartGame(){
			shogiGame.gameObject.SetActive( true );
			Debug.Log("Multiplayer ShogiGame started");
		}
    }
}
