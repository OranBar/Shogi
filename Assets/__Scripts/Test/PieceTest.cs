using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace Shogi
{
	public class PieceTest : MonoBehaviour, IPointerClickHandler
	{
		public Piece piece;
		//We're doing depencenty injection by referencing MB from inspector
		[SerializeField, RequireInterface( typeof( IMovementStrategy ) )]
		private Object _defaultMovement;
		public IMovementStrategy DefaultMovement
		{
			get => _defaultMovement as IMovementStrategy;
			set => _defaultMovement = (Object)value;
		}

		[SerializeField, RequireInterface( typeof( IMovementStrategy ) )]
		private Object _promotedMovement;
		public IMovementStrategy PromotedMovement
		{
			get => _promotedMovement as IMovementStrategy;
			set => _promotedMovement = (Object)value;
		}


		public int X
		{
			get
			{
				return piece.x;
			}
			set
			{
				piece.x = value;
			}
		}

		public int Y
		{
			get
			{
				return piece.y;
			}
			set
			{
				piece.y = value;
			}
		}

		public int startX, startY;

		private BoardTest board;
		private RectTransform rectTransform;
		public Action<int, int> OnPieceMoved = ( _, __ ) => { };

		void Awake() {
			board = FindObjectOfType<BoardTest>();
			rectTransform = this.GetComponent<RectTransform>();
		}

		void Start() {
			piece = new Piece( board.board, startX, startY, DefaultMovement );

			OnPieceMoved += ( x, y ) => Debug.Log( $"Moving to {x}, {y}" );
			OnPieceMoved += PieceMovementAnimation;
		}


		private void PieceMovementAnimation( int x, int y ) {
			rectTransform.anchoredPosition = new Vector3( x, y ) * board.cellSizeUnit;
		}

		public void PreviewAvailableMoves() {
			throw new NotImplementedException();
		}

		public void Promote() {
			piece.movementStrategy = PromotedMovement;
		}

		public void OnPointerClick( PointerEventData eventData ) {
			var move = piece.GetAvailableMoves() [0];
			board.MovePiece( this, move.x, move.y );
		}

	}
}
