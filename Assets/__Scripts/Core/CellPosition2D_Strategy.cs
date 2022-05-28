using UnityEngine;

namespace Shogi
{
	public class CellPosition2D_Strategy : ACellPosition_Provider
	{
		public float cellSizeUnit = 37.4f;

		public override Vector3 GetCellPosition( int x, int y ) {
			Vector3 cellCenterOffset = Vector3.one * cellSizeUnit * 0.5f;
			return new Vector3( x, y ) * cellSizeUnit + cellCenterOffset;
		}
	}
}
