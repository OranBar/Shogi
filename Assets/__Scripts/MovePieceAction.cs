using System.Linq;
using System.Threading.Tasks;

namespace Shogi
{
	public interface IShogiAction
	{
		Task ExecuteAction( ShogiGame game );
		bool IsMoveValid( ShogiGame game );
		IPlayer GetPlayer( ShogiGame game );
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


		public async Task ExecuteAction( ShogiGame game ) {
			Board board = game.board;

			Piece actingPiece = board [startX, startY];
			await actingPiece.PieceMovementAnimation( this );

			Piece capturedPiece = board.board [destinationX, destinationY];
			bool wasCapturingMove = capturedPiece != null && capturedPiece.owner != actingPiece.owner;

			if (wasCapturingMove) {
				//A piece was killed. Such cruelty. 
				capturedPiece.CapturePiece();
			}
			
			UpdateBoard( board );
			actingPiece.X = destinationX;
			actingPiece.Y = destinationY;
		}

		public void UpdateBoard( Board board ) {
			Piece piece = board [startX, startY];
			board [piece.X, piece.Y] = null;
			board [destinationX, destinationY] = piece;
		}

		public bool IsMoveValid( ShogiGame game ) {
			Piece pieceOnStartCell = game.board [startX, startY];
			Piece pieceOnDestinationCell = game.board [destinationX, destinationY];

			bool isDestinationSquareOnBoard = game.board.IsValidBoardPosition( destinationX, destinationY );
			bool isTargetSquare_occupiedByAllyPiece = pieceOnDestinationCell?.owner == pieceOnStartCell.owner;
			bool isValidPieceMovement = pieceOnStartCell.GetAvailableMoves().Any( m => m.x == destinationX && m.y == destinationY );

			return isDestinationSquareOnBoard 
				&& isTargetSquare_occupiedByAllyPiece == false
				&& isValidPieceMovement;
		}

		public IPlayer GetPlayer( ShogiGame game ) {
			return game.board [startX, startY].owner;
		}
	}
}
