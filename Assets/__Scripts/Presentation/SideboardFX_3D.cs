using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public class SideboardFX_3D : MonoBehaviour, ISideboardFX
	{
		public float duration = 1.5f;

		public Transform piecePlacementTransform;
		public int piecesPerRow;
		public int piecesPerColumn;
		public float distanceBetweenPieces;
		[Auto] private SideBoard sideBoard;
		private List<Vector3> piecesPositions = new List<Vector3>();

		void Awake() {
			piecesPositions = ComputePiecesPositions();
		}

		private List<Vector3> ComputePiecesPositions() {
			List<Vector3> result = new List<Vector3>();
			for (int i = 0 ; i < piecesPerRow * piecesPerColumn ; i++) {
				float xPos = ( i % piecesPerRow ) * distanceBetweenPieces;
				if(sideBoard.ownerId == PlayerId.Player2){
					xPos = xPos * -1;
				}
				float yPos = ( i / piecesPerRow ) * distanceBetweenPieces;
				var positionVector = piecePlacementTransform.position + new Vector3( xPos, 0, -yPos );
				result.Add( positionVector );
			}

			return result;

		}

		public async UniTask PieceAddedToSideboard_FX( Piece newPiece ) {
			if (this.enabled == false) { return; }

			// newPiece.SetPieceGraphicsActive(false);
			newPiece.transform.position = GetNextAvailablePosition();
		}

		private Vector3 GetNextAvailablePosition() {
			var targetPos = piecesPositions [sideBoard.capturedPieces.Count - 1];
			return targetPos;
		}
	}
}
