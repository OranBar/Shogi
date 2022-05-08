using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Shogi
{

	public class PieceActionsFX : MonoBehaviour, IPieceMoveActionFX, IPieceDropActionFX, IPieceDeathFx
	{
		[Auto] Piece piece;
		public AudioClip movementAudio;

		private AudioSource audioSource;
		private GameSettings settings;
		private ShogiGame shogiGame;
		private Cell[] cells;

		void Awake() {
			audioSource = this.gameObject.AddOrGetComponent<AudioSource>();
			settings = FindObjectOfType<GameSettings>();
			shogiGame = FindObjectOfType<ShogiGame>();
			cells = FindObjectsOfType<Cell>( true );
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

		// public void DoHighlightStartMoveCell( IShogiAction action ) {
		// 	Cell startCell = cells.First( c => c.x == action.StartX && c.y == action.StartY );
		// 	CellFx startCellFx = startCell.GetComponent<CellFx>();

		// 	Color highlightCellColor = shogiGame.settings.lastMovedPiece_color.SetAlpha( 0.5f );
		// 	startCellFx.EnableHighlight( highlightCellColor );

		// }

		private void ReparentPiece_ToOwner( Piece piece ) {
			string parentTag = piece.OwnerId == PlayerId.Player1 ? "Player1_Pieces" : "Player2_Pieces";
			Transform newParent = GameObject.FindGameObjectWithTag( parentTag ).transform;
			piece.transform.parent = newParent;
		}

		public async UniTask DoPieceDeathAnimation() {
			//TODO: Do cool particle stuff
			piece.transform.parent = this.transform;
		}
	}
}
