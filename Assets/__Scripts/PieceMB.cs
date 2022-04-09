using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

		public int x, y;
		public IMovementStrategy movementStrategy;

		public List<(int x, int y)> GetAvailableMoves() {
			var moves = movementStrategy.GetAvailableMoves( x, y );
			var result = moves.Where( m => board.IsValidBoardPosition( m ) ).ToList();
			return result;
		}

		public int startX, startY;

		private BoardMB board;
		private ShogiGameMB gameManager;
		private RectTransform rectTransform;

	
		void Awake() {
			board = FindObjectOfType<BoardMB>();
			rectTransform = this.GetComponent<RectTransform>();
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
			movementStrategy = PromotedMovement;
		}



		public void OnPointerClick( PointerEventData eventData ) {
			var move = movementStrategy.GetAvailableMoves(x,y) [0];
			gameManager.PlayAction( new MovePieceAction( this, move.x, move.y ) );
		}

	}
}
