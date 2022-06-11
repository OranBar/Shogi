using System;
using NaughtyAttributes;
using UnityEngine;

namespace Shogi
{
	public class Board_2D : ABoard
	{
		public float cellSizeUnit = 101.2f;
		
		public override Vector3 GetCellPosition( int x, int y ) {
			Vector3 cellCenterOffset = Vector3.one * cellSizeUnit * 0.5f;
			return new Vector3( x, y ) * cellSizeUnit + cellCenterOffset;
		}

		public override void PlacePieceOnCell_Immediate( int x, int y, Piece piece ){
			piece.GetComponent<RectTransform>().anchoredPosition = GetCellPosition( x, y );
		}

	}
}
