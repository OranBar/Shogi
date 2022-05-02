using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	public class PromotionUIPrompt : MonoBehaviour, IPromotionPromter
	{
		public Transform dialogWindow;
		public Button promoteBtn, dontPromoteBtn;

		private bool choiceWasMade;
		private bool choiceIsPromotion;

		void Start() {
			promoteBtn.onClick.AddListener( () =>
			{
				choiceWasMade = true; 
				choiceIsPromotion = true;
			} );
			
			dontPromoteBtn.onClick.AddListener( () =>
			{
				choiceWasMade = true; 
				choiceIsPromotion = false;
			} );

			dialogWindow.gameObject.SetActive( false );
		}

		public async UniTask<bool> GetPromotionChoice() {
			choiceWasMade = false;
			choiceIsPromotion = false;
			
			dialogWindow.gameObject.SetActive( true );
			await UniTask.WaitUntil( () => choiceWasMade );
			dialogWindow.gameObject.SetActive( false );
			return choiceIsPromotion;
		}

		public async UniTask<bool> GetPromotionChoice( IShogiAction action ) {
			choiceWasMade = false;
			choiceIsPromotion = false;

			dialogWindow.gameObject.SetActive( true );
			SetDialogPosition_RelativeToTargetCell( action );

			await UniTask.WaitUntil( () => choiceWasMade );
			dialogWindow.gameObject.SetActive( false );
			return choiceIsPromotion;

			void SetDialogPosition_RelativeToTargetCell( IShogiAction action ) {
				Cell destinationCell = FindObjectsOfType<Cell>().First( c => c.x == action.DestinationX && c.y == action.DestinationY );
				float offsetDirection = action.GetActingPiece().OwnerId == PlayerId.Player1 ? -1 : 1;
				var targetDialogPosition = destinationCell.transform.position + Vector3.up * 70 * offsetDirection;
				dialogWindow.transform.position = targetDialogPosition;
			}
		}
	}
}
