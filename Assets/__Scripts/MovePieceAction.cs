namespace Shogi
{
	public class MovePieceAction
	{

		public Piece piece;
		public int destinationX, destinationY;

		public MovePieceAction( Piece piece, int destinationX, int destinationY ) {
			this.piece = piece;
			this.destinationX = destinationX;
			this.destinationY = destinationY;
		}


	}
}
