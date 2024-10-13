using System.Collections.Generic;

namespace Shogi
{
	public class PawnDropMovement : AMovementStrategy
	{
		public override List<(int x, int y)> GetAvailableMoves( int startX, int startY ) {
			// int direction = piece.OwnerId == PlayerId.Player1 ? 1 : -1;

			List<(int x, int y)> result = new List<(int x, int y)>();
			//Get All tiles that have no pieces on them.
				//Validation: Can't place pawn if there already is one on the same column
			for (int x = 0 ; x < 9 ; x++) {
				if(AnyUnpromotedPawns_OnColumn(x)){
					continue;
				}
				for (int y = 0 ; y < 9 ; y++) {
					if (board [x, y] == null) {
						result.Add( (x, y) );
					}
				}
			}
			result = FilterInvalid_BoardPositions( result );
			//Validation: Can't checkmate with pawn
			//Validation: Can't place piece where it would have no available moves. I think we need to do this check last, in the Action class, or we might have troubles with pieces that could eventually move, but can't right now because another piece is blocking them (like a horse on row 7, but 2 other allied pieces block further movement)
			return result;
		}

		bool AnyUnpromotedPawns_OnColumn(int column){
			for (int y = 0 ; y < 9 ; y++) {
				Piece curr_piece = board [column, y];
				bool isAlliedPiece = curr_piece?.OwnerId == piece.OwnerId;

				bool isPawn = curr_piece?.PieceType == PieceType.Pawn;
				bool isUnpromotedPawn = isPawn && curr_piece.IsPromoted == false;

				if (isAlliedPiece && isUnpromotedPawn) {
					return true;
				}
			}
			return false;
		}

	}
	
}