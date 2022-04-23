using System.Collections.Generic;
using System.Linq;

namespace Shogi
{
	public class PromotedRookMovement : AMovementStrategy
	{
		private KingMovement kingMovement;
		private RookMovement rookMovement;

		protected override void Awake(){
			base.Awake();
			kingMovement = this.gameObject.AddOrGetComponent<KingMovement>();
			rookMovement = this.gameObject.AddOrGetComponent<RookMovement>();
		}

		public override List<(int x, int y)> GetAvailableMoves( int startX, int startY ) {
			var rookMoves = rookMovement.GetAvailableMoves(startX, startY);
			var kingMoves = kingMovement.GetAvailableMoves(startX, startY);
			
			return rookMoves.Concat( kingMoves ).ToList();
		}

	}

}