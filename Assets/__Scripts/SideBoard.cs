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

		public RefAction<Piece> OnNewPieceCaptured { get; private set; }

		[SerializeField] private List<Piece> capturedPieces = new List<Piece>();

		public void AddCapturedPiece(Piece piece){
			capturedPieces.Add(piece);
			OnNewPieceCaptured?.Invoke(piece);
		}

		
	}

}
