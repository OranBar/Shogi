using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shogi {
	public class PieceTest : MonoBehaviour {
		public Piece piece;
		//We're doing depencenty injection by referencing MB from inspector
		[SerializeField, InterfaceType( typeof( IMovementStrategy ) )] 
		private Object _defaultMovement;
		public IMovementStrategy DefaultMovement
		{
			get => _defaultMovement as IMovementStrategy;
			set => _defaultMovement = (Object)value;
		}

		[SerializeField, InterfaceType( typeof( IMovementStrategy ) )]
		private Object _promotedMovement;
		public IMovementStrategy PromotedMovement
		{
			get => _promotedMovement as IMovementStrategy;
			set => _promotedMovement = (Object)value;
		}
		

		public int x, y;

		private BoardTest board;
		

		void Awake(){
			board = FindObjectOfType<BoardTest>();
		}

		void Start() {
			piece = new Piece( board.board, x, y, DefaultMovement );
		}

		public void PreviewAvailableMoves(){
			throw new NotImplementedException();
		}

		public void Promote(){
			piece.movementStrategy = PromotedMovement;
		}
	}
}
