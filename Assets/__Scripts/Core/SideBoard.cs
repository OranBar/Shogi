using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Shogi
{
	public abstract class SideBoard : MonoBehaviour
	{
		public PlayerId ownerId;
		public RefAction<Piece> OnNewPieceAdded = new RefAction<Piece>();
		public RefAction<Piece> OnNewPieceRemoved = new RefAction<Piece>();

		public List<Piece> capturedPieces = new List<Piece>();

		public void RefreshWithPiecesInScene() {
			HashSet<Piece> targetSideboardPieces = new HashSet<Piece>();
			foreach (var piece in FindObjectsOfType<Piece>()) {
				if (piece.IsCaptured && piece.OwnerId == ownerId) {
					targetSideboardPieces.Add( piece );
				}
			}

			foreach (var pieceToRemove in capturedPieces.Except( targetSideboardPieces ).ToList()) {
				RemoveCapturedPiece( pieceToRemove );
			}

			foreach (var pieceToAdd in targetSideboardPieces.Except( capturedPieces ).ToList()) {
				AddCapturedPiece( pieceToAdd );
				PlacePiece_OnSideboard_Immediate( pieceToAdd );
			}
		}

		public void AddCapturedPiece( Piece piece ) {
			capturedPieces.Add( piece );
			piece.X = piece.OwnerId == PlayerId.Player1 ? -1 : -2;
			piece.Y = capturedPieces.Count;

			//TODO: So... I think I want to make an abstract method in this class: async DoNewPieceAddedAnimation()
			// foreach (var newPieceAdded_listener in GetComponentsInChildren<ISideboardFX>()) {
			// 	await newPieceAdded_listener.OnNewPieceAdded( piece );
			// }
		}

		public abstract void PlacePiece_OnSideboard_Immediate(Piece piece);

		public void RemoveCapturedPiece( Piece piece ) {
			capturedPieces.Remove( piece );
			OnNewPieceRemoved?.Invoke( piece );
		}
		
	}
}
