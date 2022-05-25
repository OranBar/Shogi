// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to handle typical game management requirements
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using Shogi;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 649

/// <summary>
/// Game manager.
/// Connects and watch Photon Status, Instantiate Player
/// Deals with quiting the room and the game
/// Deals with level loading (outside the in room synchronization)
/// </summary>
public class ShogiMultiplayerLauncher : MonoBehaviourPunCallbacks 
{
	[Tooltip( "The prefab to use for representing the player" )]
	public GameObject playerPrefab;


	IEnumerator Start() {

		// in case we started this demo with the wrong scene being active, simply load the menu scene
		if (!PhotonNetwork.IsConnected) {
			// SceneManager.LoadScene( "PunBasics-Launcher" );
			Debug.LogError("Not connected to PUN");
			yield break;;
		}

		// if (PlayerManager.LocalPlayerInstance == null) {
		Debug.LogFormat( "We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName );

		// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
		var newPlayer = PhotonNetwork.Instantiate( this.playerPrefab.name, new Vector3( 0f, 5f, 0f ), Quaternion.identity, 0 );
		var shogiGame = FindObjectOfType<ShogiGame>();
		
		yield return new WaitUntil( () => shogiGame.Player1 != null && shogiGame.Player2 != null );

		shogiGame.BeginGame( PlayerId.Player1 );
	}


	// [PunRPC]
	// public void RegisterPlayer_ToShogiGame_RPC(PhotonView playerView){
	// 	ShogiGame shogiGame = FindObjectOfType<ShogiGame>();
	// 	if(shogiGame.Player1 == null){
	// 		shogiGame.Player1 = playerView.GetComponent<APlayer>();
	// 	} else if(shogiGame.Player2 == null){
	// 		shogiGame.Player2 = playerView.GetComponent<APlayer>();
	// 	} else {
	// 		Debug.LogError("Do we have 3 players? What's going on");
	// 	}
	// }

	/// <summary>
	/// MonoBehaviour method called on GameObject by Unity on every frame.
	/// </summary>
	void Update() {
		// "back" button of phone equals "Escape". quit app if that's pressed
		if (Input.GetKeyDown( KeyCode.Escape )) {
			QuitApplication();
		}
	}
	public override void OnPlayerEnteredRoom( Player other ) {
		Debug.Log( "OnPlayerEnteredRoom() " + other.NickName ); // not seen if you're the player connecting

		if (PhotonNetwork.IsMasterClient) {
			Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient ); // called before OnPlayerLeftRoom
		}
	}

	/// <summary>
	/// Called when a Photon Player got disconnected. We need to load a smaller scene.
	/// </summary>
	/// <param name="other">Other.</param>
	public override void OnPlayerLeftRoom( Player other ) {
		Debug.Log( "OnPlayerLeftRoom() " + other.NickName ); // seen when other disconnects

		if (PhotonNetwork.IsMasterClient) {
			Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient ); // called before OnPlayerLeftRoom

			
		}
	}

	/// <summary>
	/// Called when the local player left the room. We need to load the launcher scene.
	/// </summary>
	public override void OnLeftRoom() {
		Debug.Log("Room Left. Load scene?");
	}

	public void LeaveRoom() {
		PhotonNetwork.LeaveRoom();
	}

	public void QuitApplication() {
		Application.Quit();
	}
}

