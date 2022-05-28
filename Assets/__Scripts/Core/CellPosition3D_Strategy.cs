using UnityEngine;

namespace Shogi
{
	public class CellPosition3D_Strategy : ACellPosition_Provider 
	{
		public float cellSizeUnit = 37.4f;
		public float boardHeight;

		public override Vector3 GetCellPosition( int x, int y ) {
			Vector3 cellCenterOffset = new Vector3( 1, 0, 1 ) * cellSizeUnit * 0.5f;
			var result = new Vector3( x, 0, y ) * cellSizeUnit + cellCenterOffset;
			result.y = boardHeight;
			return result;
		}
	}
}
