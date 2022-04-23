using System.Linq;
using Cysharp.Threading.Tasks;

namespace Shogi
{
	public class DropPieceAction : IShogiAction
	{

		public int DestinationX { get => _destinationX; set => _destinationX = value; }
		public int DestinationY { get => _destinationY; set => _destinationY = value; }

		private int _destinationY;
		private int _destinationX;
		public Piece actingPiece;
		
		public DropPieceAction( Piece piece ) {
			this.actingPiece = piece;
		}

		public DropPieceAction( Piece piece, int destinationX, int destinationY ) {
			this.actingPiece = piece;
			this._destinationX = destinationX;
			this._destinationY = destinationY;
		}

		public override string ToString() {
			return $"Drop to ({_destinationX}, {_destinationY})";
		}


		public async UniTask ExecuteAction( ShogiGame game ) {
			UnityEngine.Debug.Log($"Dropping piece {actingPiece} on ({DestinationX},{DestinationY})");
			Board board = game.board;

			game.GetSideBoard( actingPiece.OwnerId ).RemoveCapturedPiece( actingPiece );
			await actingPiece.PieceMovementAnimation( _destinationX, _destinationY );

			//Update game data structures
			UpdateBoard( board );
			actingPiece.X = _destinationX;
			actingPiece.Y = _destinationY;
			actingPiece.IsCaptured = false;
		}

		public void UpdateBoard( Board board ) {
			board [_destinationX, _destinationY] = actingPiece;
		}

		public bool IsMoveValid( ShogiGame game ) {
			bool isValidPieceMovement = actingPiece.GetAvailableMoves().Any( m => m.x == _destinationX && m.y == _destinationY );			return isValidPieceMovement;
			return isValidPieceMovement;
		}
	}
}
