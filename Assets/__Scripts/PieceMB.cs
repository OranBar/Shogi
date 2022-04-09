using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Shogi
{
	public class PieceMB : MonoBehaviour, IPointerClickHandler
	{
		#region Movement Strategy
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

		public void PieceDeathAnimation() {
			throw new NotImplementedException();
		}
		#endregion

		public Piece piece;
		public int startX, startY;

		// public static Action<PieceTest, int, int> OnAnyPieceMoved = (a,b,c) => { };
		
		private BoardMB board;
		private ShogiGameMB gameManager;
		private RectTransform rectTransform;

		public int X
		{
			get
			{
				return piece.x;
			}
		}

		public int Y
		{
			get
			{
				return piece.y;
			}
		}


		void Awake() {
			board = FindObjectOfType<BoardMB>();
			rectTransform = this.GetComponent<RectTransform>();
			piece = new Piece( board.board, startX, startY, DefaultMovement );
		}

		void Start() {

			// piece.OnPieceMoved += ( x, y ) => Debug.Log( $"Moving to {x}, {y}" );
			// piece.OnPieceMoved += PieceMovementAnimation;
			// piece.OnPieceMoved += (x , y) => OnAnyPieceMoved.Invoke(this, x, y);
		}

		public void PieceMovementAnimation( MovePieceAction action ) {
			PieceMovementAnimation( action.destinationX, action.destinationY );
		}

		public void PieceMovementAnimation( int x, int y ) {
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
			gameManager.PlayAction( new MovePieceAction(this, move.x, move.y ) );
		}

	}
}
