
using UnityEngine;

namespace Shogi{
	public class LoadSave_GameState : MonoBehaviour {
		[Auto] ShogiGame gameManager;

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

		[ContextMenu( "Save" )]
		public void SaveGameState_Editor() {
			GameState gameState = new GameState( gameManager );
			string path = Application.persistentDataPath + "/shogi.bin";
			SaveGameState( gameState, path );
		}

		[ContextMenu( "Load bin" )]
		public void ApplyGameState_Editor() {
			string path = Application.persistentDataPath + "/shogi.bin";
			GameState gameState = LoadGameState( path );
			gameManager.ApplyGameState( gameState );
		}
	}
}