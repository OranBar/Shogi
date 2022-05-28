using System.Collections.Generic;
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
		public AudiosList audios;

		private AudioSource audioSource;
		private ShogiGameSettings settings;
		private ShogiGame shogiGame;



		void Awake() {
			audioSource = this.gameObject.AddOrGetComponent<AudioSource>();
			settings = FindObjectOfType<ShogiGameSettings>();
			shogiGame = FindObjectOfType<ShogiGame>();
		}

		public async UniTask DoMoveAnimation( MovePieceAction action ) {
			if(settings.playSoundOnMove && action.IsCapturingMove(shogiGame) == false ){ PlayMoveAudio(); }

			await MovementAnimation( action );

			//Tanto per
			await UniTask.Yield();

			void PlayMoveAudio() {
				audioSource.clip = audios.GetMoveAudio();
				audioSource.Play();
			}
		}

		private async UniTask MovementAnimation(IShogiAction action){
			var targetWorldPosition = shogiGame.board.GetCellPosition( action.DestinationX, action.DestinationY );
			//Here we need to call PlacePIeceOnCell_Immediate for the animation instead of directly changing anchor, so it works for 3d too
			await piece.GetComponent<RectTransform>().DOAnchorPos3D( targetWorldPosition, .15f ).SetEase( Ease.InSine );
		}

		public async UniTask DoDropAnimation( DropPieceAction action ) {
			ReparentPiece_ToOwner( piece );
			//temporary 
			await MovementAnimation( action );
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
			// piece.transform.SetParent( this.transform, true );
			if (settings.playSoundOnMove) { PlayDeathAudio(); }

			void PlayDeathAudio() {
				audioSource.clip = audios.GetDeathAudio();
				audioSource.Play();
			}
		}
	}
}
