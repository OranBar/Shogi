using System;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Shogi{
	[Serializable]
	public abstract class AShogiAction : IShogiAction
	{
		public int StartY { get => _startY; set => _startY = value; }
		public int StartX { get => _startX; set => _startX = value; }
		public int DestinationX { get => _destinationX; set => _destinationX = value; }
		public int DestinationY { get => _destinationY; set => _destinationY = value; }
		// protected Piece ActingPiece { get; set; }

		private int _startX;
		private int _startY;
		private int _destinationY;
		private int _destinationX;
		private PieceData _actingPieceData;

		private GameState _gameState_beforeMove;
		public GameState GameState_beforeMove => _gameState_beforeMove;

		[NonSerialized] private Piece actingPiece;

		public AShogiAction(){

		}

		public AShogiAction( Piece piece ) {
			this.StartX = piece.X;
			this.StartY = piece.Y;
			this.actingPiece = piece;
			this._actingPieceData = piece.pieceData;
		}

		public AShogiAction( Piece piece, int destinationX, int destinationY ) {
			this.StartX = piece.X;
			this.StartY = piece.Y;
			this.actingPiece = piece;
			this._actingPieceData = piece.pieceData;
			this.DestinationX = destinationX;
			this.DestinationY = destinationY;
		}

		public Piece GetActingPiece( ) {
			// if (_actingPieceData.isCaptured) {
			// 	var sideboard = game.GetSideBoard( _actingPieceData.owner );
			// 	return sideboard.CapturedPieces.First( p => p.PieceType == _actingPieceData.pieceType );
			// } else {
			// 	return game.board [StartX, StartY];
			// }
			return actingPiece;
		}

		public override string ToString() {
			return $"Move: From ({StartX}, {StartY}) to ({DestinationX}, {DestinationY})";
		}

		public virtual async UniTask ExecuteAction( ShogiGame game ){
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
	}
}
