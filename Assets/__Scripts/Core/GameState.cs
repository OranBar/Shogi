using System;
using System.Linq;

namespace Shogi
{
	[Serializable]
    public class GameState 
    {
		public PlayerId currPlayerTurn;
		public PieceData[] piecesData;
		public float player1_time;
		public float player2_time;

		public GameState(){ }

		public GameState( ShogiGame game )
		{
			this.piecesData = game.AllPieces.Select( p => p.pieceData ).ToArray();
			this.currPlayerTurn = game.CurrTurn_Player.PlayerId;
			ShogiClock shogiClock = game.shogiClock;
			this.player1_time = shogiClock.timer_player1.clockTime;
			this.player2_time = shogiClock.timer_player2.clockTime;
		}


		#region Binary Serialization and Deserialization
		public void SerializeToBinaryFile( string savePath ) {
			SerializationUtils.SerializeToBinaryFile( this, savePath );
		}

		public static GameState DeserializeFromBinaryFile( string filePath ) {
			return SerializationUtils.DeserializeFromBinaryFile<GameState>( filePath );
		}
		#endregion

		public override string ToString() {
			var resultArr = piecesData
				.Select( p => (p.x, p.y) )
				.OrderBy( pos => pos.x + pos.y * 10 )
				.ToList();

			return resultArr.ToStringPretty();
		}
	}

	[Serializable]
	public class GameData
	{
		public string player1_name;
		public string player2_name;

		public GameData( ShogiGame game ) {
			this.player1_name = game.Player1.PlayerName;
			this.player2_name = game.Player2.PlayerName;
		}
	}
	
}
