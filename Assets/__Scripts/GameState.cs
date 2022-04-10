using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shogi
{
	[Serializable]
    public class GameState 
    {
		public GameData game;
		public PieceData[] pieces;

		public GameState(PieceMB[] piecesInGame)
		{
			pieces = piecesInGame.Select( p => new PieceData( p ) ).ToArray();
		}
	}

	public class GameData
	{

	}

	public class PieceData
	{
		public int x, y;
		public PieceType pieceType;
		private bool isPromoted;
		public PlayerId owner;

		public PieceData( int x, int y, PieceType pieceType, bool isPromoted, PlayerId owner ) {
			Construct( x, y, pieceType, isPromoted, owner );
		}

		public PieceData( PieceMB piece ) {
			Construct( piece.x, piece.y, piece.pieceType, piece.isPromoted, piece.owner.playerId );
		}

		private void Construct( int x, int y, PieceType pieceType, bool isPromoted, PlayerId owner ) {
			this.x = x;
			this.y = y;
			this.pieceType = pieceType;
			this.isPromoted = isPromoted;
			this.owner = owner;
		}
	}

	
}
