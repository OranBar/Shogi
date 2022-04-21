using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Shogi
{
	[Serializable]
	public class PieceData
	{
		public int x, y;
		public PieceType pieceType;
		public bool isPromoted;
		public PlayerId owner;
		public bool isCaptured;
	}

	public class Piece : MonoBehaviour, IPointerClickHandler
	{
	
		public PieceData pieceData;

		public int X { get => pieceData.x; set => pieceData.x = value; }
		public int Y { get => pieceData.y; set => pieceData.y = value; }
		public PieceType PieceType { get => pieceData.pieceType; set => pieceData.pieceType = value; }
		public bool IsPromoted { get => pieceData.isPromoted; set => pieceData.isPromoted = value; }
		public PlayerId OwnerId {
			get { return pieceData.owner; }
			set {
				pieceData.owner = value;
				owner = GameObjectEx.FindAll_InterfaceImplementors<IPlayer>().First( p => p.PlayerId == value );
			}
		}
		public bool IsCaptured{ 
			get { return pieceData.isCaptured; }
			set { 
				pieceData.isCaptured = value;
				if(value){
					X = Y = -1;
				}
			}
		}

		public IPlayer owner;
		
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
		
		#endregion

		private IMovementStrategy dropMovementStrategy;
		public IMovementStrategy MovementStrategy{
			get{
				if(IsCaptured){
					return dropMovementStrategy;
				}
				if(IsPromoted){
					return PromotedMovement;
				}
				return DefaultMovement;
			}
		}

	
		private Board board;
		private ShogiGame gameManager;
		[Auto, HideInInspector] public RectTransform rectTransform;

		void Awake() {
			board = FindObjectOfType<Board>();
			gameManager = FindObjectOfType<ShogiGame>();
			dropMovementStrategy = this.gameObject.AddOrGetComponent<DropMovement>();
			
			owner = FindObjectsOfType<HumanPlayer>().First( p => p.PlayerId == OwnerId );
		}

		public List<(int x, int y)> GetAvailableMoves() {
			var moves = MovementStrategy.GetAvailableMoves( X, Y );
			// var result = moves.Where( m => board.IsValidBoardPosition( m ) ).ToList();
			// result = moves.Where( m => board [m.x, m.y]?.OwnerId != OwnerId ).ToList();
			return moves;
		}

		public async UniTask PieceMovementAnimation( int destinationX, int destinationY ) {
			PlacePieceOnCell_Immediate( destinationX, destinationY );
			await UniTask.Yield();
		}

		

		public void PlacePieceOnCell_Immediate( int x, int y ) {
			rectTransform.anchoredPosition = board.GetCellWorldPosition(x,y);
		}

		public void PieceDeathAnimation() {
			// Destroy( this.gameObject);
		}
	
		public void LogAvailableMoves() {
			Debug.Log("Available moves: \n"+GetAvailableMoves().ToStringPretty());			
		}

		public void CapturePiece() {
			PieceDeathAnimation();

			X = -1;
			Y = -1;
			//Thou shall live again
			this.IsCaptured = true;
			SendToSideboard();

			void SendToSideboard() {
				if (OwnerId == PlayerId.Player1) {
					OwnerId = PlayerId.Player2;
					gameManager.player2_sideboard.AddCapturedPiece( this );
				} else {
					OwnerId = PlayerId.Player1;
					gameManager.player1_sideboard.AddCapturedPiece( this );
				}
			}
		}

		public void Promote() {
			IsPromoted = true;
			//TODO: change icon
		}

		public void OnPointerClick( PointerEventData eventData ) {
			Debug.Log("Piece Clicked");
			ShogiGame.OnAnyPieceClicked.Invoke(this);
		}

		public override string ToString() {
			return $"Piece ({X}, {Y})";
		}
	}
}
