using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Shogi{
	public class AssignCellPositions_UI : MonoBehaviour
	{

		//Assumes hierarchy is ordered. Order is as follows
		//first row: (8,0), (8,1), ...., (8,8); 
		//then second row: (7,0), (7,1), ...., (7,8);
		// .... ; 
		//until last row (0,0), (7,1), ...., (0,8)
		[Button]
		void AssignCellPosition() {
			List<Transform> children = this.transform.GetChildren();
			for (int y = 9 - 1; y >= 0 ; y--)
			{
				for (int x = 0; x < 9; x++)
				{
					Cell currCell = children[0].gameObject.AddOrGetComponent<Cell>();
					currCell.x = x;
					currCell.y = y;
					PrefabUtility.RecordPrefabInstancePropertyModifications( currCell );
					children.RemoveAt(0);
				}
			}
		}	
	}
}
#endif