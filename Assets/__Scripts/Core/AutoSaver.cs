using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Shogi
{
	public class AutoSaver : MonoBehaviour
	{
		public string autosaveRootName = "AutoSave";

		public string AutoSaveDir_RootPath => $"{Application.persistentDataPath}/{autosaveRootName}";
		public string GetSaveDirPath( int gameIndex ) => $"{AutoSaveDir_RootPath}/Game_{gameIndex}";

		private int CurrGameIndex;
		private ShogiGame shogiGame;


		

		private int autoSaveCounter;

		void Awake() {
			shogiGame = FindObjectOfType<ShogiGame>();
			CurrGameIndex = ComputeGameIndex();
			CreateSaveDirs_IfNotExists();

			
			int ComputeGameIndex() {
				try {
					DirectoryInfo [] autoSave_Directories = new DirectoryInfo( AutoSaveDir_RootPath ).GetDirectories();
					return autoSave_Directories.Length;
				} catch (DirectoryNotFoundException) {
					return 0;
				}
			}
		}

		void Start() {
			shogiGame.OnNewTurnBegun += SaveGameState;
			shogiGame.OnNewTurnBegun += SaveGameHistory;
		}

		void OnDisable() {
			shogiGame.OnNewTurnBegun -= SaveGameState;
			shogiGame.OnNewTurnBegun -= SaveGameHistory;
		}

		#region Creating AutoSaves Logic
		private void CreateSaveDirs_IfNotExists() {
			CreateDir_IfNotExists( AutoSaveDir_RootPath );
			CreateDir_IfNotExists( GetSaveDirPath( CurrGameIndex ) );

			int autoSavedGames = new DirectoryInfo( AutoSaveDir_RootPath ).GetDirectories().Length;
			if (autoSavedGames >= 400) {
				Debug.LogWarning( "Found " + autoSavedGames + " AutoSaved games. Be aware of disk space" );
			}


			void CreateDir_IfNotExists( string dirPath ) {
				if (Directory.Exists( dirPath ) == false) {
					Directory.CreateDirectory( dirPath );
				}
			}
		}

		public void SaveGameState( PlayerId currTurnPlayer ) {
			autoSaveCounter++;
			string savePath = $"{GetSaveDirPath( CurrGameIndex )}/gamestate_{autoSaveCounter}.bin";
			new GameState( shogiGame ).SerializeToBinaryFile( savePath );
		}

		public void SaveGameHistory( PlayerId currTurnPlayer ) {
			//Override previous file
			string savePath = $"{GetSaveDirPath( CurrGameIndex )}/gamehistory.bin";
			shogiGame.gameHistory.SerializeToBinaryFile( savePath );
		}

		#endregion


		#region Loading AutoSaves Logic (UnityEditor Inspector)
		[BoxGroup("Reload Parameters")] public int gameId;
		[BoxGroup( "Reload Parameters" )] public int turnIndex;

		// [ShowNativeProperty]
		public int LastAutoSavedGameId
		{
			get
			{
				DirectoryInfo [] savedGames = new DirectoryInfo( AutoSaveDir_RootPath ).GetDirectories();
				if (savedGames.Length == 0) { return 0; }

				return savedGames.Max( d => int.Parse( d.Name.SkipWhile( c => c != '_' ).Skip( 1 ).ToArray() ) );
			}
		}


		[Button]
		void ReloadGameState_Editor(){
			ReloadGameState( gameId, turnIndex );
		}

		[Button]
		void ReloadGameHistory() {
			ReloadGameHistory( gameId );
		}

		[Button]
		void ReloadGameHistory_AtTurn( ) {
			ReloadGameHistory_AtTurn( gameId, turnIndex );
		}

		void ReloadGameState( int gameId, int turnIndex ) {
			Debug.Assert( turnIndex > 0 );

			string path = GetSaveDirPath( gameId );
			string binFileName = $"gamestate_{turnIndex}.bin";
			string fullPath = path + "/" + binFileName;

			GameState loadedGameState = GameState.DeserializeFromBinaryFile( fullPath );

			shogiGame.ApplyGameState( loadedGameState );
		}

		void ReloadGameHistory( int gameId ) {
			string path = GetSaveDirPath( gameId );
			string binFileName = $"gamehistory.bin";
			string fullPath = path + "/" + binFileName;

			GameHistory loadedGame = GameHistory.DeserializeFromBinaryFile( fullPath );
			shogiGame.ApplyGameHistory( loadedGame ).Forget();
		}

		void ReloadGameHistory_AtTurn( int gameId, int turnIndex ) {
			Debug.Assert( turnIndex > 0 );

			string path = GetSaveDirPath( gameId );
			string binFileName = $"gamehistory.bin";
			string fullPath = path + "/" + binFileName;

			GameHistory loadedGame = GameHistory.DeserializeFromBinaryFile( fullPath );
			loadedGame.playedMoves = loadedGame.playedMoves.Take( turnIndex ).ToList();
			shogiGame.ApplyGameHistory( loadedGame ).Forget();
		}
		#endregion
	}
}