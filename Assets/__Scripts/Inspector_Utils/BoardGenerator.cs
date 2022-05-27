using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Shogi
{
    public class BoardGenerator : MonoBehaviour
    {
		private const int BOARD_SIZE = 9;
		public Board board;
		public GameObject cellPrefab;

		[Button]
		public void Generate(){
			this.transform.GetChildren().ForEach(c => DestroyImmediate(c.gameObject));
			for (int x = 0; x < BOARD_SIZE ; x++)
			{
				for (int y = 0; y < BOARD_SIZE ; y++)
				{
					Vector3 cellPos = board.GetCellPosition(x,y);
					GameObject newCell_obj = PrefabUtility.InstantiatePrefab( cellPrefab, this.transform ) as GameObject;
					newCell_obj.transform.position = cellPos;
					Cell newCell = newCell_obj.GetComponent<Cell>();
					newCell.x = x;
					newCell.y = y;

					newCell.name = $"Cell ({x},{y})";
				}
			}
		}
	}
}
#endif