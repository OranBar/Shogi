using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Shogi
{

	//Se tutti i pezzi fanno la stessa cosa, posso usare questa classe.
	//Se un giorno devo avere due IPieceMoveActionFx, posso smettere di ereditare qui, e portare il codice in una classe a se', ereditanto di la'
	//Alla fine dei conti posso arrivare ad avere 3 classi, che implementano rispettivamente ognuna delle 3 interfacce.
	//Tantovale gia' creare la differenziazione fin da subito, e accettare il fatto che i miei gameobject saranno pieni di componenti?
	//O fa parte dello scrivere codice scalabile farlo cosi', e ?
	public class PieceActionsFX : MonoBehaviour, IPieceMoveActionFX, IPieceDropActionFX, IPieceDeathFx
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
			await piece.GetComponent<RectTransform>().DOAnchorPos3D( targetWorldPosition, .15f ).SetEase( Ease.InSine );

			//Tanto per
			await UniTask.Yield();

			void PlayMoveAudio() {
				audioSource.clip = movementAudio;
				audioSource.Play();
			}
		}

		public async UniTask DoDropAnimation( int destinationX, int destinationY ) {
			ReparentPiece_ToOwner( piece );
			//temporary 
			await DoMoveAnimation( destinationX, destinationY );
		}

		private void ReparentPiece_ToOwner( Piece piece ) {
			string parentTag = piece.OwnerId == PlayerId.Player1 ? "Player1_Pieces" : "Player2_Pieces";
			Transform newParent = GameObject.FindGameObjectWithTag( parentTag ).transform;
			// piece.transform.parent = newParent;
			piece.transform.SetParent(newParent, true);
		}

		public async UniTask DoPieceDeathAnimation() {
			//TODO: Do cool particle stuff
			// piece.transform.parent = this.transform;
			piece.transform.SetParent( this.transform, true );
		}
	}
}
