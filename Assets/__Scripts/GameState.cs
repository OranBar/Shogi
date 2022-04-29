using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Shogi
{
	[Serializable]
    public class GameState 
    {
		public GameData gameData;
		public PlayerId currPlayerTurn;
		public PieceData[] piecesData;

		public GameState(){

		}

		public GameState( ShogiGame game)
		{
			piecesData = game.AllPieces.Select( p => p.pieceData ).ToArray();
			currPlayerTurn = game.CurrTurn_Player.PlayerId;
			gameData = new GameData( game.Player1.PlayerName, game.Player2.PlayerName );
		}

		#region Binary Serialization and Deserialization
		public void SerializeToBinaryFile( string savePath ) {
			SerializationUtils.SerializeToBinaryFile( this, savePath );
		}

		public static GameState DeserializeFromBinaryFile( string filePath ) {
			return SerializationUtils.DeserializeFromBinaryFile<GameState>( filePath );
		}


		public override string ToString() {
			var resultArr = piecesData
				.Select( p => (p.x, p.y))
				.OrderBy( pos => pos.x + pos.y * 10 )
				.ToList();
				
			return resultArr.ToStringPretty();
		}
		#endregion
	}

	[Serializable]
	public class GameData
	{
		public string player1_name;
		public string player2_name;

		public GameData( string player1_name, string player2_name ) {
			this.player1_name = player1_name;
			this.player2_name = player2_name;
		}
	}

	

	
}
