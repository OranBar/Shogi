using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Shogi;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Shogi
{


	public class AutoSaver : MonoBehaviour
	{
		private ShogiGame shogiGame;

		public string autosaveRootName = "AutoSave";

		public string AutoSaveDir_RootPath => $"{Application.persistentDataPath}/{autosaveRootName}";

		public string SaveDirPath( int gameIndex ) => $"{AutoSaveDir_RootPath}/Game_{gameIndex}";

		private int CurrGameIndex;

		private int ComputeGameIndex() {
			DirectoryInfo [] autoSave_Directories = new DirectoryInfo( AutoSaveDir_RootPath ).GetDirectories();

			return autoSave_Directories.Length;
		}

		private int autoSaveCounter;

		void Awake() {
			shogiGame = FindObjectOfType<ShogiGame>();
			CurrGameIndex = ComputeGameIndex();
			CreateSaveDirs_IfNotExists();
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
			CreateDir_IfNotExists( SaveDirPath( CurrGameIndex ) );

			int autoSavedGames = new DirectoryInfo( AutoSaveDir_RootPath ).GetDirectories().Length;
			if (autoSavedGames >= 200) {
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
			string savePath = $"{SaveDirPath( CurrGameIndex )}/gamestate_{autoSaveCounter}.bin";
			new GameState( shogiGame ).SerializeToBinaryFile( savePath );
		}

		public void SaveGameHistory( PlayerId currTurnPlayer ) {
			//Override previous file

			string savePath = $"{SaveDirPath( CurrGameIndex )}/gamehistory.bin";
			shogiGame.gameHistory.SerializeToBinaryFile( savePath );
		}

		#endregion


		#region Loading AutoSaves Logic

		[Button]
		public void ReloadGameState( int gameId, int turnIndex ) {
			Debug.Assert( turnIndex > 0 );

			string path = SaveDirPath( gameId );
			string binFileName = $"gamestate_{turnIndex}.bin";
			string fullPath = path + "/" + binFileName;

			GameState loadedGameState = GameState.DeserializeFromBinaryFile( fullPath );

			shogiGame.ApplyGameState( loadedGameState );
		}

		[Button]
		public void ReloadGameHistory( int gameId ) {
			string path = SaveDirPath( gameId );
			string binFileName = $"gamehistory.bin";
			string fullPath = path + "/" + binFileName;

			GameHistory loadedGame = GameHistory.DeserializeFromBinaryFile( path );
			shogiGame.ApplyGameHistory( loadedGame ).Forget();
		}

		[Button]
		public void ReloadGameHistory_AtTurn( int gameId, int turnIndex ) {
			Debug.Assert( turnIndex > 0 );

			string path = SaveDirPath( gameId );
			string binFileName = $"gamehistory.bin";
			string fullPath = path + "/" + binFileName;

			GameHistory loadedGame = GameHistory.DeserializeFromBinaryFile( path );
			loadedGame.playedMoves = loadedGame.playedMoves.Take( turnIndex ).ToList();
			shogiGame.ApplyGameHistory( loadedGame ).Forget();
		}



		[ShowInInspector]
		[PropertySpace]
		public int LastAutoSavedGameId
		{
			get
			{
				DirectoryInfo [] savedGames = new DirectoryInfo( AutoSaveDir_RootPath ).GetDirectories();
				if (savedGames.Length == 0) { return 0; }

				return savedGames.Max( d => int.Parse( d.Name.SkipWhile( c => c != '_' ).Skip( 1 ).ToArray() ) );
			}
		}
		#endregion


	}
}