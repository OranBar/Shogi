using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
	[Serializable]
	public class GameHistory {
		public GameState initialGameState;
		public PlayerId firstToMove;
		public GameData gameData;
		public List<IShogiAction> playedMoves = new List<IShogiAction>();
		public float player1_time;
		public float player2_time;

		public GameHistory( GameState initialGameState, PlayerId firstToMove, ShogiGame game) {
			this.initialGameState = initialGameState;
			this.firstToMove = firstToMove;
			this.gameData = new GameData( game );
		}

		public void RegisterNewMove( IShogiAction action ) {
			if(action is UndoLastAction){
				Debug.LogWarning("Saving an Undo action into the GameHistory is weird. The code doesn't really take that possibility into account. Can we not?");
			}
			playedMoves.Push( action );
			Debug.Log("<GameHistory> New move Registered");
		}

		#region Binary Serialization and Deserialization
		public void SerializeToBinaryFile( string savePath ) {
			SaveTimers_ClockTime();
			SerializationUtils.SerializeToBinaryFile( this, savePath );
		}

		public static GameHistory DeserializeFromBinaryFile( string filePath ) {
			return SerializationUtils.DeserializeFromBinaryFile<GameHistory>( filePath );
		}

		private void SaveTimers_ClockTime() {
			var shogiClock = GameObject.FindObjectOfType<ShogiGame>().shogiClock;
			this.player1_time = shogiClock.timer_player1.clockTime;
			this.player2_time = shogiClock.timer_player2.clockTime;
		}

		#endregion
	}
	
}