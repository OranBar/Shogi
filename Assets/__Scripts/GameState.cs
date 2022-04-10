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
		public GameData game;
		public PieceData[] pieces;

		public GameState()
		{
			Piece [] piecesInGame = GameObject.FindObjectsOfType<Piece>();
			pieces = piecesInGame.Select( p => p.pieceData ).ToArray();
		}

		#region Serialization and Deserialization
		public void SerializeToBinaryFile(string savePath){
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream( savePath, FileMode.Create, FileAccess.Write, FileShare.None );
			formatter.Serialize( stream, this );
			stream.Close();
			Debug.Log( "File serialized at " + savePath );
		}

		public static GameState DeserializeFromBinaryFile(string filepPath){
			IFormatter formatter = new BinaryFormatter();
			string path = Application.persistentDataPath + "/shogi.bin";
			Stream stream = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read );
			GameState deserializedData = (GameState)formatter.Deserialize( stream );
			stream.Close();

			return deserializedData;
		}
		#endregion
	}

	[Serializable]
	public class GameData
	{

	}

	

	
}
