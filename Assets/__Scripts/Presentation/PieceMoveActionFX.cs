using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Shogi
{

	public class PieceMoveActionFX : MonoBehaviour, IPieceMoveActionFX
	{
		[Auto] Piece piece;
		public AudioClip movementAudio;

		private AudioSource audioSource;
		private GameSettings settings;
		private ShogiGame shogiGame;

		void Awake() {
			audioSource = this.gameObject.AddOrGetComponent<AudioSource>();
			settings = FindObjectOfType<GameSettings>();
			shogiGame = FindObjectOfType<ShogiGame>();
		}

		public async UniTask DoMoveAnimation( int destinationX, int destinationY ) {
			if(settings.playSoundOnMove){ PlayMoveAudio(); }
			
			var targetWorldPosition = shogiGame.board.GetCellPosition( destinationX, destinationY );
			await piece.GetComponent<RectTransform>().DOAnchorPos3D( targetWorldPosition, .55f ).SetEase( Ease.InSine );

			//Tanto per
			await UniTask.Yield();
		}

		public void PlayMoveAudio() {
			audioSource.clip = movementAudio;
			audioSource.Play();
		}

	}
}
