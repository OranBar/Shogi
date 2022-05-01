using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Shogi{
	[Serializable]
	public class SideBoard : MonoBehaviour
	{
	
		public PlayerId ownerId;
		public IPlayer Owner => GameObjectEx.FindAll_InterfaceImplementors<IPlayer>().First( p => p.PlayerId == ownerId );

		public RefAction<Piece> OnNewPieceAdded { get; private set; } = new RefAction<Piece>();
		public RefAction<Piece> OnNewPieceRemoved { get; private set; } = new RefAction<Piece>();
		public RefAction OnCleared { get; private set; } = new RefAction();

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
			//We can't remove from the object we're iterating, so we create a copy with ToList()
			foreach(Piece capturedPiece in _capturedPieces.ToList()){
				RemoveCapturedPiece( capturedPiece );
			}
			_capturedPieces.Clear();
			OnCleared?.Invoke();
		}

		public void RefreshWithPiecesInScene() {
			ClearCapturedPieces();

			foreach (var piece in FindObjectsOfType<Piece>()) {
				if (piece.IsCaptured && piece.OwnerId == ownerId) {
					//Maybe I don't want to call the event here?
					AddCapturedPiece( piece );
				}
			}
		}

		public void AddCapturedPiece(Piece piece){
			_capturedPieces.Add(piece);
			piece.X = piece.OwnerId == PlayerId.Player1 ? -1 : -2;
			piece.Y = _capturedPieces.Count;
			OnNewPieceAdded?.Invoke(piece);
			SendPieceToLimbo( piece );
		}

		public void RemoveCapturedPiece(Piece piece){
			_capturedPieces.Remove(piece);
			OnNewPieceRemoved?.Invoke(piece);
			ReparentPiece_ToOwner( piece );
		}

		private void SendPieceToLimbo(Piece piece ) {
			Transform limbo = GameObject.FindGameObjectWithTag( "Limbo" ).transform;
			piece.transform.SetParent(limbo);
			piece.transform.localPosition = Vector3.zero;
		}

		private void ReparentPiece_ToOwner(Piece piece) {
			string parentTag = piece.OwnerId == PlayerId.Player1 ? "Player1_Pieces" : "Player2_Pieces";
			Transform newParent = GameObject.FindGameObjectWithTag( parentTag ).transform;
			piece.transform.parent = newParent;
		}

		
	}

}
