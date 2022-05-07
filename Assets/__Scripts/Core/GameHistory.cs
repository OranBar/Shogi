using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Shogi
{
	[Serializable]
	public class GameHistory {
		public GameState initialGameState;
		public PlayerId firstToMove;
		public List<IShogiAction> playedMoves = new List<IShogiAction>();

		public GameHistory( GameState initialGameState, PlayerId firstToMove) {
			this.initialGameState = initialGameState;
			this.firstToMove = firstToMove;
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
			SerializationUtils.SerializeToBinaryFile(this, savePath);
		}

		public static GameHistory DeserializeFromBinaryFile( string filePath ) {
			return SerializationUtils.DeserializeFromBinaryFile<GameHistory>( filePath );
		}

		#endregion
	}
	
}