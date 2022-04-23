using System.Collections.Generic;
using System.Linq;

namespace Shogi
{
	public class PromotedBishopMovement : AMovementStrategy
	{
		private KingMovement kingMovement;
		private BishopMovement bishopMovement;

		protected override void Awake(){
			base.Awake();
			kingMovement = this.gameObject.AddOrGetComponent<KingMovement>();
			bishopMovement = this.gameObject.AddOrGetComponent<BishopMovement>();
		}

		public override List<(int x, int y)> GetAvailableMoves( int startX, int startY ) {
			var bishopMoves = bishopMovement.GetAvailableMoves(startX, startY);
			var kingMoves = kingMovement.GetAvailableMoves(startX, startY);
			
			return bishopMoves.Concat( kingMoves ).ToList();
		}

	}

}