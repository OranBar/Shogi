using System.Linq;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	public interface IShogiAction
	{
		public int StartX { get; set; }
		public int StartY { get; set; }
		public int DestinationX { get; set; }
		public int DestinationY { get; set; }
		UniTask ExecuteAction( ShogiGame game );
		bool IsMoveValid( ShogiGame game );
		public Piece GetActingPiece( ShogiGame game );
	}
	public class MovePieceAction : AShogiAction
	{
		public bool PromotePiece { get => _promotePiece; set => _promotePiece = value; }
		private bool _promotePiece = false;

		public MovePieceAction( Piece piece ) : base( piece ) {
		}

		public MovePieceAction( Piece piece, int destinationX, int destinationY ) : base( piece, destinationX, destinationY ) {
		}

		public override async UniTask ExecuteAction( ShogiGame game ) {
			Board board = game.board;
			var actingPiece = board[StartX, StartY];
			UnityEngine.Debug.Log( $"Moving piece {actingPiece} on ({DestinationX},{DestinationY})" );


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

		public override bool IsMoveValid( ShogiGame game ) {
			var actingPiece = game.board[StartX, StartY];
			bool isValidPieceMovement = actingPiece.GetAvailableMoves().Any( m => m.x == DestinationX && m.y == DestinationY );

			return isValidPieceMovement;
		}
	}
}
