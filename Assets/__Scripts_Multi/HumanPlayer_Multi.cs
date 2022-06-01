using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Auto.Utils;
using BarbarO.ExtensionMethods;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace Shogi
{
    public class HumanPlayer_Multi : HumanPlayer
    {
		[Auto] private PhotonView photonView;

		public void DisableUndoButton_OnOpponentTurn(PlayerId currTurn_playerId){
			if(currTurn_playerId != this.PlayerId){
				undoButton.interactable = false;
			}
		}

		public void EnableUndoButton_OnOurTurn( PlayerId currTurn_playerId ) {
			if (currTurn_playerId == this.PlayerId) {
				undoButton.interactable = true;
			}
		}

		IEnumerator Start(){
			if ( photonView.IsMine == false ) {	yield break; }

			FindObjectOfType<ShogiGame>().OnNewTurnBegun += DisableUndoButton_OnOpponentTurn;
			FindObjectOfType<ShogiGame>().OnNewTurnBegun += EnableUndoButton_OnOurTurn;

			var myPlayerId = PhotonNetwork.IsMasterClient ? PlayerId.Player1 : PlayerId.Player2;
			photonView.RPC( nameof( RegisterPlayer_ToShogiGame_RPC ), RpcTarget.AllBuffered, myPlayerId );

			if(PhotonNetwork.IsMasterClient == false) {
				RotateBoard();
				RotateSideboards();
			}

			#region Local Methods -----------------------------

				void RotateBoard() {
					var boardRect = FindObjectOfType<ABoard>().GetComponent<RectTransform>();
					var newRotation = boardRect.localEulerAngles;
					newRotation.z = 180;
					boardRect.localEulerAngles = newRotation;
				}

				void RotateSideboards(){
					var sideBoards = FindObjectsOfType<SideBoard>();
					var newRotation = new Vector3(0,0,180);
					sideBoards [0].transform.localEulerAngles = newRotation;
					sideBoards [1].transform.localEulerAngles = newRotation;
				}
				
			#endregion -----------------------------------------

		}

		[PunRPC]
		public void RegisterPlayer_ToShogiGame_RPC(PlayerId playerId) {
			ShogiGame shogiGame = FindObjectOfType<ShogiGame>();

			this.PlayerId = playerId;
			if (playerId == PlayerId.Player1) {
				shogiGame.Player1 = GetComponent<APlayer>();
			} else {
				shogiGame.Player2 = GetComponent<APlayer>();
			}
			this.gameObject.ExecuteDelayed( ShowOwnedPieces, .6f );


			#region Local Methods -----------------------------

				void ShowOwnedPieces( ) {
						shogiGame.AllPieces.Where( p => p.OwnerId == PlayerId ).ForEach( p => p.SetPieceGraphicsActive( true ) );
						Debug.Log( "EnablePieces " + PlayerId );
				}
			
			#endregion -----------------------------------------

		}

		public async override UniTask<AShogiAction> RequestAction() {
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
