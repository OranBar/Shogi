namespace Shogi
{
	public class MovePieceAction
	{

		public PieceMB piece;
		public int destinationX, destinationY;

		public MovePieceAction( PieceMB piece, int destinationX, int destinationY ) {
			this.piece = piece;
			this.destinationX = destinationX;
			this.destinationY = destinationY;
		}


	}
}
