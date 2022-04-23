using Cysharp.Threading.Tasks;

namespace Shogi{
	public abstract class AShogiAction : IShogiAction
	{
		public int StartY { get => startY; set => startY = value; }
		public int StartX { get => _startX; set => _startX = value; }
		public int DestinationX { get => _destinationX; set => _destinationX = value; }
		public int DestinationY { get => _destinationY; set => _destinationY = value; }

		private int _startX;
		private int startY;
		private int _destinationY;
		private int _destinationX;

		public AShogiAction( Piece piece ) {
			this.StartX = piece.X;
			this.StartY = piece.Y;
		}

		public AShogiAction( Piece piece, int destinationX, int destinationY ) {
			this.StartX = piece.X;
			this.StartY = piece.Y;
			this.DestinationX = destinationX;
			this.DestinationY = destinationY;
		}

		public Piece GetActingPiece(ShogiGame game){
			return game.board [StartX, StartY];
		}


		public override string ToString() {
			return $"Move: From ({StartX}, {StartY}) to ({DestinationX}, {DestinationY})";
		}

		public abstract UniTask ExecuteAction( ShogiGame game );
		public abstract bool IsMoveValid( ShogiGame game );
	}
}
