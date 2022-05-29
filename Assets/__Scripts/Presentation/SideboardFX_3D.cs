using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Shogi
{
	public class SideboardFX_3D : MonoBehaviour, ISideboardPieceAdded
	{
		public float duration = 1.5f;
		// [AutoParent] private SideBoard_UI sideboard;

		public async UniTask OnNewPieceAdded( Piece newPiece ) {
			if (this.enabled == false) { return; }

			newPiece.SetPieceGraphicsActive(false);
			newPiece.transform.position = GetNextAvailablePosition();
		}

		private Vector3 GetNextAvailablePosition(){
			return Vector3.zero;
		}
	}
}
