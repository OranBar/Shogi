using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Shogi
{
	public class LoadSave_GameState_Editor : MonoBehaviour {
		[Auto] ShogiGame gameManager;

		#region Editor 
		public string fileName = "shogi";
		[Button]
		void SaveGameState() {
			GameState gameState = new GameState( gameManager );
			string path = Application.persistentDataPath + $"/{fileName}.bin";
			gameState.SerializeToBinaryFile( path );
		}

		[Button]
		void ApplyGameState() {
			string path = Application.persistentDataPath + $"/{fileName}.bin";
			GameState gameState = GameState.DeserializeFromBinaryFile( path );
			
			gameManager.ApplyGameState( gameState );
		}

		[Button]
		void SaveGameHistory() {
			string path = Application.persistentDataPath + $"/{fileName}.bin";
			gameManager.gameHistory.SerializeToBinaryFile( path );
		}

		[Button]
		void ApplyGameHistory() {
			string path = Application.persistentDataPath + $"/{fileName}.bin";
			GameHistory gameHistory = GameHistory.DeserializeFromBinaryFile( path );

			gameManager.ApplyGameHistory( gameHistory ).Forget();
		}
		#endregion
	}
}