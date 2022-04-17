using System.Linq;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	public class DropPieceAction : IShogiAction
	{

		public int startX, startY;
		public int destinationX, destinationY;

		public DropPieceAction( int startX, int startY, int destinationX, int destinationY ) {
			this.startX = startX;
			this.startY = startY;
			this.destinationX = destinationX;
			this.destinationY = destinationY;
		}

		public DropPieceAction( Piece piece, int destinationX, int destinationY ) {
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

			await actingPiece.PieceMovementAnimation( destinationX, destinationY );

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
			bool isValidPieceMovement = pieceOnStartCell.GetAvailableMoves().Any( m => m.x == destinationX && m.y == destinationY );			return isValidPieceMovement;
		}
	}
}
