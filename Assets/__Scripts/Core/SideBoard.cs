using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Shogi{
	[Serializable]
	public class SideBoard : MonoBehaviour
	{
	
		public PlayerId ownerId;
		public APlayer Owner => GameObjectEx.FindAll_InterfaceImplementors<APlayer>().First( p => p.PlayerId == ownerId );

		public RefAction<Piece> OnNewPieceAdded = new RefAction<Piece>();
		public RefAction<Piece> OnNewPieceRemoved = new RefAction<Piece>();
		public RefAction OnCleared = new RefAction();

		[SerializeField] private List<Piece> _capturedPieces = new List<Piece>();
		public List<Piece> CapturedPieces {
			get{
				return _capturedPieces;
			}
			set{
				_capturedPieces = value;

			}
		}

		//Needed for when we restart or reload the game
		private void ClearCapturedPieces(){
			while(_capturedPieces.Count != 0){
				RemoveCapturedPiece( _capturedPieces.First() );
			}
			_capturedPieces.Clear();
			OnCleared?.Invoke();
		}

		public void RefreshWithPiecesInScene() {
			HashSet<Piece> targetSideboardPieces = new HashSet<Piece>();
			foreach (var piece in FindObjectsOfType<Piece>()) {
				if (piece.IsCaptured && piece.OwnerId == ownerId) {
					targetSideboardPieces.Add( piece );
				}
			}

			foreach(var pieceToRemove in CapturedPieces.Except( targetSideboardPieces ).ToList()){
				RemoveCapturedPiece( pieceToRemove );
			}

			foreach (var pieceToAdd in targetSideboardPieces.Except( CapturedPieces ).ToList()) {
				AddCapturedPiece( pieceToAdd );
			}
		}

		public async UniTask AddCapturedPiece(Piece piece){
			_capturedPieces.Add(piece);
			piece.X = piece.OwnerId == PlayerId.Player1 ? -1 : -2;
			piece.Y = _capturedPieces.Count;
			

			foreach(var newPieceAdded_listener in GetComponentsInChildren<ISideboardPieceAdded>()){
				await newPieceAdded_listener.OnNewPieceAdded(piece);
			}
		}

		public void RemoveCapturedPiece(Piece piece){
			_capturedPieces.Remove(piece);
			OnNewPieceRemoved?.Invoke(piece);
		}
	}
}
