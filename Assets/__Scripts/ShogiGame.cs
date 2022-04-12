using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public class ShogiGame : MonoBehaviour
	{
		#region Events
		public static Action<Cell> OnAnyCellClicked = ( _ ) => { };
		public static Action<Piece> OnAnyPieceClicked = ( _ ) => { };
		#endregion

		#region ToSerialize
		public IPlayer oran; //Player 1
		public IPlayer gio; //Player 2
		private PlayerId _currPlayer_turn;
		public IPlayer CurrPlayer_turn {
			get
			{
				return _currPlayer_turn == PlayerId.Player1 ? oran : gio;
			}
		}
		#endregion
		public Board board;
		public bool manualOverride;
		private bool isGameOver;

		void Start() {
			board.InitWithPiecesInScene();
			//TODO: black starts first
			_currPlayer_turn = PlayerId.Player1;
			BeginGame( _currPlayer_turn );
		}

		async void BeginGame( PlayerId startingPlayer ) {
			_currPlayer_turn = startingPlayer;

			while(isGameOver == false && manualOverride == false){
				IShogiAction action = await CurrPlayer_turn.RequestAction();
				await PlayAction( action );
				AdvanceTurn();
			}
			Debug.Log( "Game Finished" );
		}

		private void AdvanceTurn() {
			_currPlayer_turn = (_currPlayer_turn == PlayerId.Player1) ? PlayerId.Player2 : PlayerId.Player1;
		}

		public async Task PlayAction( IShogiAction action ) {
			if(action.GetPlayer(this) != CurrPlayer_turn){
				Debug.LogWarning("It's not your turn!");
				return;
			}

			if (action.IsMoveValid( this )) {
				await action.ExecuteAction( this );
				//memento action
			}
		}

		[ContextMenu( "Save" )]
		public void SaveGameState() {
			GameState gameState = new GameState();
			string json = JsonUtility.ToJson( gameState );
			Debug.Log( json );

			string path = Application.persistentDataPath + "/shogi.bin";
			gameState.SerializeToBinaryFile( path );
		}

		[ContextMenu( "Load bin" )]
		public void LoadGameState() {
			string path = Application.persistentDataPath + "/shogi.bin";
			GameState obj = GameState.DeserializeFromBinaryFile( path );
			string json = JsonUtility.ToJson( obj );
			Debug.Log( json );
			//TODO: reassign data
		}

	}
}
