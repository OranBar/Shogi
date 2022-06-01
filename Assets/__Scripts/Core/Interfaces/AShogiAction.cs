using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi{
	[Serializable]
	public abstract class AShogiAction : IShogiAction
	{
		public int StartY { get => _startY; set => _startY = value; }
		public int StartX { get => _startX; set => _startX = value; }
		public int DestinationX { get => _destinationX; set => _destinationX = value; }
		public int DestinationY { get => _destinationY; set => _destinationY = value; }

		private int _startX;
		private int _startY;
		private int _destinationY;
		private int _destinationX;

		[NonSerialized] private GameState _gameState_beforeMove;
		public GameState GameState_beforeMove => _gameState_beforeMove;
		
		private PlayerId _playerId;
		public PlayerId PlayerId { get => _playerId; set => _playerId = value; }

		[NonSerialized] private Piece _actingPiece;
		public Piece ActingPiece => FindActingPiece();



		public AShogiAction(){
		}

		public AShogiAction( Piece piece ) {
			this.StartX = piece.X;
			this.StartY = piece.Y;
			this._actingPiece = piece;
			this.PlayerId = piece.OwnerId;
		}

		public AShogiAction( Piece piece, int destinationX, int destinationY ) {
			this.StartX = piece.X;
			this.StartY = piece.Y;
			this._actingPiece = piece;
			this.DestinationX = destinationX;
			this.DestinationY = destinationY;
			this.PlayerId = piece.OwnerId;
		}


		//This only works before ExecuteMove is called				
		//I should probably fetch the piece from the GameState_beforeMove somehow.
		//Or maybe use a pieceId?
		private Piece FindActingPiece() {
			if(_actingPiece == null){
				Debug.Log($"Looking for piece {StartX}, {StartY}");
				_actingPiece = GameObject.FindObjectsOfType<Piece>(true).First( p => p.X == StartX && p.Y == StartY );
			}
			return _actingPiece;
		}

		public override string ToString() {
			return $"{ActingPiece.PieceType.ToString()} From ({StartX}, {StartY}) to ({DestinationX}, {DestinationY})";
		}

		public virtual async UniTask ExecuteAction( ShogiGame game ){
			_actingPiece = FindActingPiece();
			//save gamestate
			_gameState_beforeMove = new GameState( game );
		}

		public async UniTask UndoAction( ShogiGame game ){
			//reload gamestate
			if(_gameState_beforeMove == null){
				throw new System.Exception( "Can't undo a move that was not executed. Did you call base.ExecuteAction()?" );
			}
			game.ApplyGameState( _gameState_beforeMove );

		}
		public abstract bool IsMoveValid( ShogiGame game );

		// public abstract UniTask EnableLastMoveFX( GameSettings settings );
		// public abstract void DisableLastMoveFX();
	}
}
