using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Shogi
{
	//Does it make sense to ahve a pieceId assigned to every piece, considering they never actually die/get destroyed?
	//This way I can reference them using id instead of x and y position (those change with moves, which makes it hard to do undo, for example)
	[Serializable]
	public struct PieceData
	{
		public int x, y;
		public PieceType pieceType;
		public bool isPromoted;
		public PlayerId owner;
		public bool isCaptured;
	}

	public class Piece : MonoBehaviour, IPointerClickHandler
	{
		public static RefAction<Piece> OnAnyPieceClicked = new RefAction<Piece>();
		[HideInInspector] public RefAction OnPieceClicked = new RefAction();
		public IPieceActionsFX pieceFx;

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
			set
			{
				pieceData.owner = value;
				//Maybe here invoke an event, and let PieceActionsFX_*D react by rotating the piece
				pieceFx.RotatePiece(value);
			}
		}

		public bool IsCaptured{ 
			get { return pieceData.isCaptured; }
			set { 
				pieceData.isCaptured = value;
			}
		}

		[SerializeReference] public AMovementStrategy defaultMovement;
		[SerializeReference] public AMovementStrategy promotedMovement;
		[SerializeReference] private AMovementStrategy dropMovement;

		public AMovementStrategy MovementStrategy{
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

		[FormerlySerializedAs("defaultImage")]
		public Image pieceIconImage;
		public Sprite defaultSprite;
		public Sprite promotedSprite;

		public GameObject pieceGraphics;

		private ABoard board;
		private ShogiGame gameManager;

		private void OnValidate() {
			if(Application.isPlaying == false){
				if(pieceIconImage != null) { pieceIconImage.sprite = defaultSprite; }
			}
		}

		void Awake() {
			board = FindObjectOfType<ABoard>();
			gameManager = FindObjectOfType<ShogiGame>();
			pieceFx = GetComponent<IPieceActionsFX>();
		}

		public List<(int x, int y)> GetValidMoves() {
			var result = MovementStrategy.GetAvailableMoves( X, Y );
			
			result = result.Where( Destination_IsNot_OccupiedByAlliedPiece ).ToList();
			return result;
			
			bool Destination_IsNot_OccupiedByAlliedPiece( (int x, int y) move ){ 
				return board [move.x, move.y]?.OwnerId != OwnerId;
			}
		}

		public void SetPieceGraphicsActive( bool enable ) {
			this.pieceGraphics.gameObject.SetActive( enable );
		}

		public void PlacePieceOnCell_Immediate( int x, int y ) {
			pieceFx.PlacePieceOnCell_Immediate( x, y );
		}

		public async UniTask CapturePiece() {
			//Thou shall live again
			this.IsCaptured = true;
			this.IsPromoted = false;
			await pieceFx.DoPieceDeathAnimation();
			await SendToSideboard();
			
			async UniTask SendToSideboard() {
				if (OwnerId == PlayerId.Player1) {	
					OwnerId = PlayerId.Player2;
					await gameManager.player2_sideboard.AddCapturedPiece( this );
				} else {
					OwnerId = PlayerId.Player1;
					await gameManager.player1_sideboard.AddCapturedPiece( this );
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
			pieceIconImage.color = Color.red;
		}

		private void Unpromote() {
			Debug.Log( "Piece Unromoted =(" );
			//TODO: change icon
			pieceIconImage.color = Color.black;
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
	}
}
