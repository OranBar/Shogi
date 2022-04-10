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

		public GameState(Piece[] piecesInGame)
		{
			pieces = piecesInGame.Select( p => p.pieceData ).ToArray();
		}
	}

	public class GameData
	{

	}

	

	
}
