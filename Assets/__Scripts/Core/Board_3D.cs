using System;
using NaughtyAttributes;
using UnityEngine;

namespace Shogi
{
	public class Board_3D : ABoard
	{
		public float cellSizeUnit = 1;
		
		public override Vector3 GetCellPosition( int x, int y ) {
			Vector3 cellCenterOffset = new Vector3( 1, 0, 1 ) * cellSizeUnit * 0.5f;
			var result = new Vector3( x, 0, y ) * cellSizeUnit + cellCenterOffset;
			return result;
		}

		public override void PlacePieceOnCell_Immediate( int x, int y, Piece piece ){
			piece.transform.position = GetCellPosition( x, y );
		}

	}
}
