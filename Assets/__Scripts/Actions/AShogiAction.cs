using System;
using Cysharp.Threading.Tasks;

namespace Shogi{
	[Serializable]
	public abstract class AShogiAction : IShogiAction
	{
		public int StartY { get => startY; set => startY = value; }
		public int StartX { get => _startX; set => _startX = value; }
		public int DestinationX { get => _destinationX; set => _destinationX = value; }
		public int DestinationY { get => _destinationY; set => _destinationY = value; }
		public Piece ActingPiece { get => _actingPiece; set => _actingPiece = value; }

		private int _startX;
		private int startY;
		private int _destinationY;
		private int _destinationX;
		private Piece _actingPiece;

		private GameState gameState_beforeMove;

		public AShogiAction(){
			
		}

		public AShogiAction( Piece piece ) {
			this.StartX = piece.X;
			this.StartY = piece.Y;
			this._actingPiece = piece;
		}

		public AShogiAction( Piece piece, int destinationX, int destinationY ) {
			this.StartX = piece.X;
			this.StartY = piece.Y;
			this._actingPiece = piece;
			this.DestinationX = destinationX;
			this.DestinationY = destinationY;
		}

		public override string ToString() {
			return $"Move: From ({StartX}, {StartY}) to ({DestinationX}, {DestinationY})";
		}

		public virtual async UniTask ExecuteAction( ShogiGame game ){
			//save gamestate
			gameState_beforeMove = new GameState();
		}
		public async UniTask UndoAction( ShogiGame game ){
			//reload gamestate
			if(gameState_beforeMove == null){
				throw new System.Exception( "Can't undo a move that was not executed. Did you call base.ExecuteAction()?" );
			}
			game.ApplyGameState( gameState_beforeMove );
		}
		public abstract bool IsMoveValid( ShogiGame game );
	}
}
