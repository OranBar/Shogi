using System.Collections;
using System.Collections.Generic;
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
	}
}
