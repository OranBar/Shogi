using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Shogi{
	public class Foo : SerializedMonoBehaviour
	{
		[SceneObjectsOnly]
		public IPlayer player;
		[ContextMenu( "AssignCellPosition" )]
		void AssignCellPosition() {
			// List<Cell> cells = GetComponentsInChildren<Cell>().ToList();
			List<Transform> children = this.transform.GetChildren();
			for (int y = 9 - 1; y >= 0 ; y--)
			{
				for (int x = 0; x < 9; x++)
				{
					Cell currCell = children[0].gameObject.AddOrGetComponent<Cell>();
					currCell.x = x;
					currCell.y = y;
					children.RemoveAt(0);
				}
			}
		}	
	}
}
