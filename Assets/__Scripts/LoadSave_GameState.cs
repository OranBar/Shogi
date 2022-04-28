
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

		[ContextMenu( "Save History" )]
		public void SaveGameHistory_Editor() {
			string path = Application.persistentDataPath + $"/{fileName}.bin";
			SaveGameHistory( gameManager.gameHistory, path );
		}

		[ContextMenu("Load History")]
		public async UniTask ApplyGameHistory_Editor() {
			string path = Application.persistentDataPath + $"/{fileName}.bin";
			GameHistory loadedGameHistory = LoadGameHistory( path );
			
			gameManager.ApplyGameState( loadedGameHistory.initialGameState );
			gameManager.gameHistory = loadedGameHistory;
			Debug.Log( "Loaded initial game state" );

			//Alter timescale to fast forward?
			foreach (var move in loadedGameHistory.playedMoves) {
				await move.ExecuteAction( gameManager );
			}

			PlayerId nextPlayerTurn = GetPlayer_WhoMovesNext( loadedGameHistory );
			gameManager.BeginGame( nextPlayerTurn );

			Debug.Log( "All moves applied" );

			
			
			PlayerId GetPlayer_WhoMovesNext( GameHistory loadedGameHistory ) {
				PlayerId nextPlayerTurn;
				if (loadedGameHistory.playedMoves.Count % 2 == 0) {
					nextPlayerTurn = loadedGameHistory.firstToMove;
				} else {
					nextPlayerTurn = loadedGameHistory.firstToMove == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;
				}

				return nextPlayerTurn;
			}
		}
		#endregion
	}
}