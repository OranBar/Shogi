using System.Collections;
using System.Collections.Generic;
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
			Cell destinationCell = FindObjectsOfType<Cell>().First( c => c.x == action.DestinationX && c.y == action.DestinationY );
			var targetDialogPosition = destinationCell.transform.position + new Vector3( 0, 0, 1 ) * 50;
			dialogWindow.transform.position = targetDialogPosition;
			await UniTask.WaitUntil( () => choiceWasMade );
			dialogWindow.gameObject.SetActive( false );
			return choiceIsPromotion;
		}
	}
}
