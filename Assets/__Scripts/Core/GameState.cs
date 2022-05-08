using System;
using System.Linq;

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
			gameData = new GameData( game );
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
		public float player1_time;
		public float player2_time;

		public GameData( ShogiGame game ) {
			this.player1_name = game.Player1.PlayerName;
			this.player2_name = game.Player2.PlayerName;
			var shogiClock = UnityEngine.GameObject.FindObjectOfType<ShogiClock>();
			this.player1_time = shogiClock.timer_player1.clockTime;
			this.player2_time = shogiClock.timer_player2.clockTime;
		}
	}

	

	
}
