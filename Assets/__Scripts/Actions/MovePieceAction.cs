using System.Linq;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	public interface IShogiAction
	{
		public int DestinationX { get; set; }
		public int DestinationY { get; set; }
		UniTask ExecuteAction( ShogiGame game );
		bool IsMoveValid( ShogiGame game );
	}
	public class MovePieceAction : IShogiAction
	{
		public Piece actingPiece;
		public int StartX => actingPiece.X;
		public int StartY => actingPiece.Y;

		public int DestinationX { get => _destinationX; set => _destinationX = value; }
		public int DestinationY { get => _destinationY; set => _destinationY = value; }

		private int _destinationY;
		private int _destinationX;

		public MovePieceAction( Piece piece ) {
			this.actingPiece = piece;
		}

		public MovePieceAction( Piece piece, int destinationX, int destinationY ) {
			this.actingPiece = piece;
			this.DestinationX = destinationX;
			this.DestinationY = destinationY;
		}

		public override string ToString() {
			return $"Move: From ({StartX}, {StartY}) to ({DestinationX}, {DestinationY})";
		}

		public async UniTask ExecuteAction( ShogiGame game ) {
			Board board = game.board;

			Piece capturedPiece = board[DestinationX, DestinationY];
			bool wasCapturingMove = capturedPiece != null && capturedPiece.owner != actingPiece.owner;

			if (wasCapturingMove) {
				//A piece was killed. Such cruelty. 
				capturedPiece.CapturePiece();
			}
			await actingPiece.PieceMovementAnimation( DestinationX, DestinationY );

			//Update game data structures
			UpdateBoard( board );
			actingPiece.X = DestinationX;
			actingPiece.Y = DestinationY;
		}

		public void UpdateBoard( Board board ) {
			Piece piece = board [StartX, StartY];
			board [piece.X, piece.Y] = null;
			board [DestinationX, DestinationY] = piece;
		}

		public bool IsMoveValid( ShogiGame game ) {
			Piece pieceOnStartCell = game.board [StartX, StartY];
			// Piece pieceOnDestinationCell = game.board [destinationX, destinationY];

			// bool isDestinationSquareOnBoard = game.board.IsValidBoardPosition( destinationX, destinationY );
			// bool isTargetSquare_occupiedByAllyPiece = pieceOnDestinationCell?.owner == pieceOnStartCell.owner;
			bool isValidPieceMovement = pieceOnStartCell.GetAvailableMoves().Any( m => m.x == DestinationX && m.y == DestinationY );

			return isValidPieceMovement;
		}
	}
}
