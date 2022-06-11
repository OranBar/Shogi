using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Shogi
{
	public class SideboardFX_2D : MonoBehaviour, ISideboardFX
	{
		public float duration = 1.5f;
		[AutoParent] private SideBoard_UI sideboard;

		public async UniTask PieceAddedToSideboard_FX( Piece newPiece ) {
			if (this.enabled == false) { return; }
			// Make the piece higher in z position based on the number of pieces of the same type in the sideboard, to ensure we always click the topmost
			//Don't disable the gameobject. It breaks a lot of stuff. Disable the child
			newPiece.SetPieceGraphicsActive(false);

			var sequence = DOTween.Sequence();
			await sequence
				.PrependInterval( .3f )
				.AppendCallback( () => newPiece.transform.GetChild( 0 ).gameObject.SetActive( true ) )
				.AppendCallback( () => newPiece.transform.localScale = Vector3.one * 2 )
				.AppendCallback( () => sideboard.IncreaseText( newPiece ) )
				.Append( newPiece.transform.DOScale( Vector3.one, duration ).SetEase( Ease.OutCubic ) )
				.AsyncWaitForCompletion();
		}
	}
}
