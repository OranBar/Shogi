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
		public List<(float player1_time, float player2_time)> timersHistory = new List<(float player1_time, float player2_time)>();
		[NonSerialized] private ShogiGame game;

		public GameHistory( GameState initialGameState, PlayerId firstToMove, ShogiGame game) {
			this.initialGameState = initialGameState;
			this.firstToMove = firstToMove;
			this.game = game;
			this.gameData = new GameData( game );
		}

		public void RegisterNewMove( IShogiAction action ) {
			if (action is UndoLastAction) {
				Debug.LogWarning( "Saving an Undo action into the GameHistory is weird. The code doesn't really take that possibility into account. It will be ignored" );
				return;
			}
			timersHistory.Add( GetClockTimes() );
			playedMoves.Push( action );
			Debug.Log( "<GameHistory> New move Registered" );
		}

		private (float player1_time, float player2_time) GetClockTimes() {
			(float player1_time, float player2_time) timers = (0f, 0f);
			timers.player1_time = game.shogiClock.timer_player1.clockTime;
			timers.player2_time = game.shogiClock.timer_player2.clockTime;
			return timers;
		}

		#region Binary Serialization and Deserialization
		public void SerializeToBinaryFile( string savePath ) {
			Debug.Assert( playedMoves.Count == timersHistory.Count );
			SerializationUtils.SerializeToBinaryFile( this, savePath );
		}

		public static GameHistory DeserializeFromBinaryFile( string filePath ) {
			return SerializationUtils.DeserializeFromBinaryFile<GameHistory>( filePath );
		}

		#endregion
	}
	
}