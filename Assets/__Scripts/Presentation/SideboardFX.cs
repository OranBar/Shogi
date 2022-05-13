using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Shogi
{
	public class SideboardFX : MonoBehaviour, ISideboardPieceAdded
	{
		[AutoParent] private SideBoard_UI sideboard;

		public async UniTask OnNewPieceAdded( Piece newPiece ) {
			if (this.enabled == false) { return; }
			// Make the piece higher in z position based on the number of pieces of the same type in the sideboard, to ensure we always click the topmost
			//Don't disable the gameobject. It breaks a lot of stuff. Disable the child
			newPiece.transform.GetChild(0).gameObject.SetActive( false );

			var sequence = DOTween.Sequence();
			await sequence
				.PrependInterval( .3f )
				.AppendCallback( () => newPiece.transform.GetChild( 0 ).gameObject.SetActive( true ) )
				.AppendCallback( () => newPiece.transform.localScale = Vector3.one * 2 )
				.AppendCallback( () => sideboard.IncreaseText( newPiece ) )
				.Append( newPiece.transform.DOScale( Vector3.one, 1.5f ).SetEase( Ease.OutCubic ) )
				.AsyncWaitForCompletion();
		}
	}
}
