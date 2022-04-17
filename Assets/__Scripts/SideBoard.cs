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
		public IPlayer Owner => GameObjectEx.FindAll_InterfaceImplementors<IPlayer>().First( p => p.OwnerId == ownerId );

		public RefAction<Piece> OnNewPieceCaptured { get; private set; } = new RefAction<Piece>();

		[SerializeField] private List<Piece> _capturedPieces = new List<Piece>();
		public List<Piece> CapturedPieces => _capturedPieces;

		public void AddCapturedPiece(Piece piece){
			_capturedPieces.Add(piece);
			OnNewPieceCaptured?.Invoke(piece);
		}

		
	}

}
