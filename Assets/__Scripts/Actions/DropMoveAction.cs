using System.Linq;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	public class DropPieceAction : AShogiAction
	{
		private Piece _actingPiece;

		public DropPieceAction( Piece piece ) : base( piece ) {
		}

		public DropPieceAction( Piece piece, int destinationX, int destinationY ) : base( piece, destinationX, destinationY ) {
		}

		public override string ToString() {
			return "Drop "+base.ToString();
		}

		public override async UniTask ExecuteAction( ShogiGame game ) {
			Board board = game.board;
			var actingPiece = board [StartX, StartY];
			UnityEngine.Debug.Log($"Dropping piece {actingPiece} on ({DestinationX},{DestinationY})");

			game.GetSideBoard( actingPiece.OwnerId ).RemoveCapturedPiece( actingPiece );
			await actingPiece.PieceMovementAnimation( DestinationX, DestinationY );

			//Update game data structures
			UpdateBoard( board );
			actingPiece.X = DestinationX;
			actingPiece.Y = DestinationY;
			actingPiece.IsCaptured = false;
		}

		public void UpdateBoard( Board board ) {
			var actingPiece = board [StartX, StartY];
			board [DestinationX, DestinationY] = actingPiece;
		}

		public override bool IsMoveValid( ShogiGame game ) {
			var actingPiece = game.board [StartX, StartY];

			bool isValidPieceMovement = actingPiece.GetAvailableMoves().Any( m => m.x == DestinationX && m.y == DestinationY );
			return isValidPieceMovement;
		}
	}
}
