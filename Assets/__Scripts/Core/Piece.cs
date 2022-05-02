using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shogi
{
	[Serializable]
	public struct PieceData
	{
		public int x, y;
		public PieceType pieceType;
		public bool isPromoted;
		public PlayerId owner;
		public bool isCaptured;
	}

	public class Piece : Sirenix.OdinInspector.SerializedMonoBehaviour, IPointerClickHandler
	{
		public static RefAction<Piece> OnAnyPieceClicked = new RefAction<Piece>();
		public RefAction OnPieceClicked = new RefAction();

		public PieceData pieceData;

		public int X { get => pieceData.x; set => pieceData.x = value; }
		public int Y { get => pieceData.y; set => pieceData.y = value; }
		public PieceType PieceType { get => pieceData.pieceType; set => pieceData.pieceType = value; }
		
		public bool IsPromoted { 
			get { return pieceData.isPromoted; }
			set {
				if (pieceData.isPromoted == value) { return; }

				pieceData.isPromoted = value; 
				if(value){
					Promote();
				} else {
					Unpromote();
				}
			}
		}

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
			}
		}

		public IPlayer owner;

		public IMovementStrategy defaultMovement;
		public IMovementStrategy promotedMovement;
		private IMovementStrategy dropMovement;

		public IMovementStrategy MovementStrategy{
			get{
				if(IsCaptured){
					return dropMovement;
				}
				if(IsPromoted){
					return promotedMovement;
				}
				return defaultMovement;
			}
		}

		public Image defaultImage;
		public Image promotedImage;

		public Board board;
		private ShogiGame gameManager;

		#region Presentation
		[Auto] private RectTransform rectTransform;
		// public IPieceMoveActionFX moveFx;
		// public PieceDropAnimation dropAnimation;
		// public IPieceDeathFx deathFx;

		
		#endregion

		void Awake() {
			board = FindObjectOfType<Board>();
			gameManager = FindObjectOfType<ShogiGame>();
			dropMovement = this.gameObject.AddOrGetComponent<DropMovement>();
			
			owner = FindObjectsOfType<HumanPlayer>().First( p => p.PlayerId == OwnerId );
			// moveFx = GetComponent<IPieceMoveActionFX>();
			// deathFx = GetComponent<IPieceDeathFx>();
		}

		public List<(int x, int y)> GetValidMoves() {
			var result = MovementStrategy.GetAvailableMoves( X, Y );
			
			result = result.Where( Destination_IsNot_OccupiedByAlliedPiece ).ToList();
			return result;
			
			bool Destination_IsNot_OccupiedByAlliedPiece( (int x, int y) move ){ 
				return board [move.x, move.y]?.OwnerId != OwnerId;
			}
		}

		public void CapturePiece() {
			//Thou shall live again
			this.IsCaptured = true;
			this.IsPromoted = false;
			this.transform.SetLocalRotationZ( this.transform.eulerAngles.z + 180 );
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

		public bool HasPromotion() {
			bool hasPromotion = promotedMovement != null;
			return hasPromotion;
		}

		private void Promote() {
			Debug.Log( "Piece Promoted" );
			//TODO: change icon
			defaultImage.color = Color.red;
		}

		private void Unpromote() {
			Debug.Log( "Piece Unromoted =(" );
			//TODO: change icon
			defaultImage.color = Color.black;
		}

		public void OnPointerClick( PointerEventData eventData ) {
			Debug.Log("Piece Clicked");
			Piece.OnAnyPieceClicked.Invoke(this);
			OnPieceClicked.Invoke();
		}

		public void LogAvailableMoves() {
			Debug.Log( "Available moves: \n" + GetValidMoves().ToStringPretty() );
		}

		public override string ToString() {
			return $"Piece ({X}, {Y})";
		}


		public void PlacePieceOnCell_Immediate( int x, int y ) {
			rectTransform.anchoredPosition = board.GetCellPosition( x, y );
		}
		
	}
}
