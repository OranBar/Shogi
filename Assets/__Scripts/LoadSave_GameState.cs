
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Shogi{
	public class LoadSave_GameState : MonoBehaviour {
		[Auto] ShogiGame gameManager;

		#region GameState Save/Load
		public void SaveGameState( GameState gameState, string path ) {
			string json = JsonUtility.ToJson( gameState );
			Debug.Log( json );

			gameState.SerializeToBinaryFile( path );
		}

		public GameState LoadGameState( string path ) {
			GameState result = GameState.DeserializeFromBinaryFile( path );
			string json = JsonUtility.ToJson( result );
			Debug.Log( json );
			return result;
		}

		
		public string fileName = "shogi";
		[ContextMenu( "Save" )]
		public void SaveGameState_Editor() {
			GameState gameState = new GameState( gameManager );
			string path = Application.persistentDataPath + $"/{fileName}.bin";
			SaveGameState( gameState, path );
		}

		[ContextMenu( "Load bin" )]
		public void ApplyGameState_Editor() {
			string path = Application.persistentDataPath + $"/{fileName}.bin";
			GameState gameState = LoadGameState( path );
			gameManager.ApplyGameState( gameState );
		}
		#endregion

		#region GameHistory Save/Load

		// public void SaveGameHistory( GameHistory toSave, string path ) {
		// 	string json = JsonUtility.ToJson( toSave );
		// 	Debug.Log( json );

		// 	toSave.SerializeToBinaryFile( path );
		// }

		// public GameState LoadGameHistory( string path ) {
		// 	GameState result = GameState.DeserializeFromBinaryFile( path );
			
		// 	string json = JsonUtility.ToJson( result );
		// 	Debug.Log( json );
			
		// 	return result;
		// }

		#endregion
}
}