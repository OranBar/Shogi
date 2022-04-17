using System.Linq;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	public interface IShogiAction
	{
		UniTask ExecuteAction( ShogiGame game );
		bool IsMoveValid( ShogiGame game );
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

		public override string ToString() {
			return $"Move: From ({startX}, {startY}) to ({destinationX}, {destinationY})";
		}


		public async UniTask ExecuteAction( ShogiGame game ) {
			Board board = game.board;

			Piece actingPiece = board [startX, startY];

			Piece capturedPiece = board[destinationX, destinationY];
			bool wasCapturingMove = capturedPiece != null && capturedPiece.owner != actingPiece.owner;

			if (wasCapturingMove) {
				//A piece was killed. Such cruelty. 
				capturedPiece.CapturePiece();
			}
			await actingPiece.PieceMovementAnimation( this );

			//Update game data structures
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
			// Piece pieceOnDestinationCell = game.board [destinationX, destinationY];

			// bool isDestinationSquareOnBoard = game.board.IsValidBoardPosition( destinationX, destinationY );
			// bool isTargetSquare_occupiedByAllyPiece = pieceOnDestinationCell?.owner == pieceOnStartCell.owner;
			bool isValidPieceMovement = pieceOnStartCell.GetAvailableMoves().Any( m => m.x == destinationX && m.y == destinationY );

			return isValidPieceMovement;
		}
	}
}
