using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	public abstract class APromotionPrompt : MonoBehaviour, IPromotionPromter
	{
		public Transform dialogWindow;
		public Button promoteBtn;
		public Button dontPromoteBtn;

		private bool choiceWasMade;
		private bool choiceIsPromotion;

		public async UniTask<bool> GetPromotionChoice( AShogiAction action ) {
			choiceWasMade = false;
			choiceIsPromotion = false;

			dialogWindow.gameObject.SetActive( true );
			
			var targetDialogPosition = GetTargetDialogPosition(action);
			dialogWindow.transform.position = targetDialogPosition;

			await UniTask.WaitUntil( () => choiceWasMade );
			dialogWindow.gameObject.SetActive( false );
			return choiceIsPromotion;

		}
		protected abstract Vector3 GetTargetDialogPosition( AShogiAction action );

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
	}
}
