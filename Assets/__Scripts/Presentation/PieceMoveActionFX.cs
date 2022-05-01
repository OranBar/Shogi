using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
    public class PieceMoveActionFX : MonoBehaviour
    {
		[Auto] Piece piece;
		public AudioClip movementAudio;

		private AudioSource audioSource;

		void Awake()
		{
			audioSource = this.gameObject.AddOrGetComponent<AudioSource>();
		}

		public async UniTask DoMoveAnimation( int destinationX, int destinationY ) {
			piece.PlacePieceOnCell_Immediate( destinationX, destinationY );
			PlayMoveAudio();
			await UniTask.Yield();
		}

		public void PlayMoveAudio(){
			audioSource.clip = movementAudio;
			audioSource.Play();
		}

    }
}
