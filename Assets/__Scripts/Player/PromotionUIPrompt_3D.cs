using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	public class PromotionUIPrompt_3D : APromotionPrompt
	{
		public float yOffset = 7.39f;
		protected override Vector3 GetTargetDialogPosition( AShogiAction action ){
			Cell destinationCell = FindObjectsOfType<Cell>().First( c => c.x == action.DestinationX && c.y == action.DestinationY );
			// float offsetDirection = action.ActingPiece.OwnerId == PlayerId.Player1 ? -1 : 1;
			return destinationCell.transform.position + Vector3.up * yOffset;
		} 
	}
}
