namespace Shogi
{
	public interface IShogiAction
	{
		void ExecuteAction( ShogiGame game );
		bool IsMoveValid( ShogiGame game );
		Player GetPlayer( ShogiGame game );
	}
	public class MovePieceAction : IShogiAction
	{

		public int startX, startY;
		public int destinationX, destinationY;

		public MovePieceAction( int startX, int startY, int destinationX, int destinationY ) {
			this.startX = startX;
			this.startY = startY;
			this.destinationX = destinationX;
			this.destinationY = destinationY;
		}

		public MovePieceAction( Piece piece, int destinationX, int destinationY ) {
			this.startX = piece.X;
			this.startY = piece.Y;
			this.destinationX = destinationX;
			this.destinationY = destinationY;
		}


		public void ExecuteAction( ShogiGame game ) {
			Board board = game.board;
			UpdateBoard( board, this );

			Piece actingPiece = board [startX, startY];
			actingPiece.PieceMovementAnimation( this );

			Piece capturedPiece = board.board [destinationX, destinationY];
			bool wasCapturingMove = capturedPiece != null && capturedPiece.owner != actingPiece.owner;

			if (wasCapturingMove) {
				//A piece was killed. Such cruelty. 
				capturedPiece.CapturePiece();
			}
		}

		public void UpdateBoard( Board board, MovePieceAction action ) {
			Piece piece = board [action.startX, action.startY];
			board [piece.X, piece.Y] = null;
			board [action.destinationX, action.destinationY] = piece;
		}

		public bool IsMoveValid( ShogiGame game ) {
			Piece pieceOnStartCell = game.board [destinationX, destinationY];
			Piece pieceOnDestinationCell = game.board [destinationX, destinationY];

			bool isDestinationSquareOnBoard = game.board.IsValidBoardPosition( destinationX, destinationY );
			bool isTargetSquare_occupiedByAllyPiece = pieceOnDestinationCell?.owner == pieceOnStartCell.owner;

			return isDestinationSquareOnBoard && isTargetSquare_occupiedByAllyPiece;
		}

		public Player GetPlayer( ShogiGame game ) {
			return game.board [startX, startY].owner;
		}
	}
}
