namespace Shogi
{
	public class MovePieceAction
	{

		public PieceTest piece;
		public int destinationX, destinationY;

		public MovePieceAction( PieceTest piece, int destinationX, int destinationY ) {
			this.piece = piece;
			this.destinationX = destinationX;
			this.destinationY = destinationY;
		}


	}
}
