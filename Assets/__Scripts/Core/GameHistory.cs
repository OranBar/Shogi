using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shogi
{
	[Serializable]
	public class GameHistory {
		public GameState initialGameState;
		public PlayerId firstToMove;
		public GameData gameData;
		public List<AShogiAction> playedMoves = new List<AShogiAction>();
		public List<(float player1_time, float player2_time)> timersHistory = new List<(float player1_time, float player2_time)>();
		[NonSerialized] private ShogiGame game;

		public GameHistory( GameState initialGameState, PlayerId firstToMove, ShogiGame game) {
			this.initialGameState = initialGameState;
			this.firstToMove = firstToMove;
			this.game = game;
			this.gameData = new GameData( game );
		}

		public GameHistory Clone(){
			GameHistory result = new GameHistory( this.initialGameState, this.firstToMove, this.game );
			result.playedMoves = new List<AShogiAction>( this.playedMoves );
			result.timersHistory = new List<(float player1_time, float player2_time)>( this.timersHistory );
			return result;
		}

		public GameHistory Clone(int movesCount_toClone) {
			GameHistory result = new GameHistory( this.initialGameState, this.firstToMove, this.game );
			result.playedMoves = this.playedMoves.Take( movesCount_toClone ).ToList();
			result.timersHistory = this.timersHistory.Take( movesCount_toClone ).ToList();
			return result;
		}

		public void RegisterNewMove( AShogiAction action ) {
			if (action is UndoLastAction) {
				playedMoves.Pop();
				timersHistory.Pop();
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

		public PlayerId GetPlayer_WhoMovesNext( ) {
			PlayerId nextPlayerTurn;
			if (this.playedMoves.Count % 2 == 0) {
				nextPlayerTurn = this.firstToMove;
			} else {
				nextPlayerTurn = this.firstToMove == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;
			}

			return nextPlayerTurn;
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