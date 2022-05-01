
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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

		
		#endregion

		#region GameHistory Save/Load

		public void SaveGameHistory( GameHistory toSave, string path ) {
			string json = JsonUtility.ToJson( toSave );
			Debug.Log( json );

			toSave.SerializeToBinaryFile( path );
		}

		public GameHistory LoadGameHistory( string path ) {
			GameHistory result = GameHistory.DeserializeFromBinaryFile( path );
			
			string json = JsonUtility.ToJson( result );
			Debug.Log( json );
			
			return result;
		}

		public async UniTask ApplyGameHistory( string path ) {
			GameHistory loadedGameHistory = LoadGameHistory( path );

			await gameManager.ApplyGameHistory( loadedGameHistory );
		}
		#endregion


		#region Editor 
		public string fileName = "shogi";
		[ContextMenu( "Save state" )]
		void SaveGameState_Editor() {
			GameState gameState = new GameState( gameManager );
			string path = Application.persistentDataPath + $"/{fileName}.bin";
			SaveGameState( gameState, path );
		}

		[ContextMenu( "Load state" )]
		void ApplyGameState_Editor() {
			string path = Application.persistentDataPath + $"/{fileName}.bin";
			GameState gameState = LoadGameState( path );
			gameManager.ApplyGameState( gameState );
		}

		[ContextMenu( "Save History" )]
		void SaveGameHistory_Editor() {
			string path = Application.persistentDataPath + $"/{fileName}.bin";
			SaveGameHistory( gameManager.gameHistory, path );
		}

		[ContextMenu( "Load History" )]
		async UniTask ApplyGameHistory_Editor() {
			string path = Application.persistentDataPath + $"/{fileName}.bin";
			await ApplyGameHistory( path );
		}
		#endregion
	}
}