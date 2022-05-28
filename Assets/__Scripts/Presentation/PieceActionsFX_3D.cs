using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Shogi
{
	public class PieceActionsFX_3D : MonoBehaviour, IPieceActionsFX, IHighlightFx
	{
		[Auto] Piece piece;
		public AudiosList audios;

		private AudioSource audioSource;
		private ShogiGameSettings settings;
		private ShogiGame shogiGame;
		private ABoard board;

		void Awake() {
			audioSource = this.gameObject.AddOrGetComponent<AudioSource>();
			settings = FindObjectOfType<ShogiGameSettings>();
			shogiGame = FindObjectOfType<ShogiGame>();
			board = FindObjectOfType<ABoard>();
		}

		public async UniTask DoMoveAnimation( MovePieceAction action ) {
			if(settings.playSoundOnMove && action.IsCapturingMove(shogiGame) == false ){ 
				PlayMoveAudio(); 
			}

			await MovementAnimation( action );

			#region Local Methods -----------------------------

				void PlayMoveAudio() {
					audioSource.clip = audios.GetMoveAudio();
					audioSource.Play();
				}

			#endregion -----------------------------------------
		}

		private async UniTask MovementAnimation(IShogiAction action){
			var targetWorldPosition = shogiGame.board.GetCellPosition( action.DestinationX, action.DestinationY );
			//Here we need to call PlacePieceOnCell_Immediate for the animation instead of directly changing anchor, so it works for 3d too
			await transform.DOMove( targetWorldPosition, .15f ).SetEase( Ease.InSine );
		}

		public async UniTask DoDropAnimation( DropPieceAction action ) {
			ReparentPiece_ToOwner( piece );
			//temporary 
			await MovementAnimation( action );
		}

		private void ReparentPiece_ToOwner( Piece piece ) {
			string parentTag = piece.OwnerId == PlayerId.Player1 ? "Player1_Pieces" : "Player2_Pieces";
			Transform newParent = GameObject.FindGameObjectWithTag( parentTag ).transform;
			piece.transform.SetParent(newParent, true);
		}

		public async UniTask DoPieceDeathAnimation() {
			//TODO: Do cool particle stuff
			if (settings.playSoundOnMove) { PlayDeathAudio(); }

			void PlayDeathAudio() {
				audioSource.clip = audios.GetDeathAudio();
				audioSource.Play();
			}
		}

		public void RotatePiece( PlayerId value ) {
			if (value == PlayerId.Player1) {
				this.transform.SetLocalRotationY( 0f );
			} else {
				this.transform.SetLocalRotationY( 180f );
			}
		}

		public void PlacePieceOnCell_Immediate( int x, int y ) {
			transform.position = board.GetCellPosition( x, y );
		}

		public async UniTask EnableHighlight( Color color ) {
		}

		public async UniTask DisableHighlight() {
		}
	}
}
