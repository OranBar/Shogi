using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

namespace Shogi
{
	public class PieceActionsFX_3D : MonoBehaviour, IPieceActionsFX, IHighlightFx
	{
		[Auto] Piece piece;
		public AudiosList audios;

		[AutoChildren] private MeshRenderer meshRenderer;
		private AudioSource audioSource;
		private ShogiGameSettings settings;
		private ShogiGame shogiGame;
		private ABoard board;

		private Color defaultColor;

		public MoveAnimation_Params_3D_ScriptableObj moveAnimParams;

		void Awake() {
			audioSource = this.gameObject.AddOrGetComponent<AudioSource>();
			settings = FindObjectOfType<ShogiGameSettings>();
			shogiGame = FindObjectOfType<ShogiGame>();
			board = FindObjectOfType<ABoard>();
			defaultColor = meshRenderer.material.color;
		}

		public async UniTask DoMoveAnimation( MovePieceAction action ) {
			if(settings.playSoundOnMove && action.IsCapturingMove(shogiGame) == false ){ 
				PlayMoveAudio(); 
			}

			await MovementAnimation( action );
			Enable_BoardCrack_FX( action );

			#region Local Methods -----------------------------

			void PlayMoveAudio() {
					audioSource.clip = audios.GetMoveAudio();
					audioSource.Play();
				}

			#endregion -----------------------------------------
		}

		private void Enable_BoardCrack_FX( MovePieceAction action ) {
			GameObject crackObj = GameObject.FindGameObjectWithTag("CrackFX");
			crackObj.transform.position = shogiGame.board.GetCellPosition( action.DestinationX, action.DestinationY );

			crackObj.GetComponentInChildren<ParticleSystem>().Stop();
			crackObj.GetComponentInChildren<ParticleSystem>().Play();
		}

		private async UniTask MovementAnimation(AShogiAction action){
			var targetWorldPosition = board.GetCellPosition( action.DestinationX, action.DestinationY );

			float t = 0;
			Vector3 startPosition = this.transform.position;
			while(t < 1){
				Vector3 lerpedPosition = Vector3.LerpUnclamped( startPosition, targetWorldPosition, moveAnimParams.forwardAnimCurve.Evaluate(t) );
				lerpedPosition.y = Mathf.LerpUnclamped( startPosition.y, startPosition.y + moveAnimParams.yLift_onMovement, moveAnimParams.liftPieceAnimCurve.Evaluate( t ) );
				this.transform.position = lerpedPosition;
				await UniTask.Yield();
				t += Time.deltaTime / moveAnimParams.animDuration;
			}
			this.transform.position = targetWorldPosition;

			// await transform.DOMove( targetWorldPosition, .15f ).SetEase( Ease.InSine );

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
			// meshRenderer.material.color = color;
		}


		public async UniTask DisableHighlight() {
			// meshRenderer.material.color = defaultColor;
		}
	}
}
