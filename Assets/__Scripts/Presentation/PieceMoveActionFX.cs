using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{

	public class PieceMoveActionFX : MonoBehaviour, IPieceMoveActionFX
	{
		[Auto] Piece piece;
		public AudioClip movementAudio;

		private AudioSource audioSource;
		private GameSettings settings;

		void Awake() {
			audioSource = this.gameObject.AddOrGetComponent<AudioSource>();
			settings = FindObjectOfType<GameSettings>();
		}

		public async UniTask DoMoveAnimation( int destinationX, int destinationY ) {
			piece.PlacePieceOnCell_Immediate( destinationX, destinationY );
			if(settings.playSoundOnMove){ PlayMoveAudio(); }

			await UniTask.Yield();
		}

		public void PlayMoveAudio() {
			audioSource.clip = movementAudio;
			audioSource.Play();
		}

	}
}
