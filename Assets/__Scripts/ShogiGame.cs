using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object; 

namespace Shogi
{
	public class ShogiGame : MonoBehaviour
	{
		#region Events
		public static Action<Cell> OnAnyCellClicked;
		public static Action<Piece> OnAnyPieceClicked;
		public static RefAction<Piece> OnPlayer1_PieceClicked = new RefAction<Piece>();
		public static RefAction<Piece> OnPlayer2_PieceClicked;
		public static RefAction<Piece> Get_OnPieceClickedEvent( PlayerId player ) {
			return player == PlayerId.Player1 ? OnPlayer1_PieceClicked : OnPlayer2_PieceClicked;
		}

		void RegisterPieceClickedEvents_Invocation(){
			ShogiGame.OnAnyPieceClicked += ( piece ) =>
			{
				if (piece.OwnerId == PlayerId.Player1) {
					OnPlayer1_PieceClicked.Value.Invoke( piece );
				} else {
					OnPlayer2_PieceClicked.Value.Invoke( piece );
				}
			};
		}

		
		#endregion

		[SerializeField, RequireInterface( typeof( IPlayer ) )]
		private Object _player1;
		public IPlayer Player1
		{
			get => _player1 as IPlayer;
			set => _player1 = (Object)value;
		}
		[SerializeField, RequireInterface( typeof( IPlayer ) )]
		private Object _player2;
		public IPlayer Player2
		{
			get => _player2 as IPlayer;
			set => _player2 = (Object)value;
		}
		#region ToSerialize
		
		private PlayerId _currPlayer_turn;
		public IPlayer CurrPlayer_turn {
			get
			{
				return _currPlayer_turn == PlayerId.Player1 ? Player1 : Player2;
			}
		}

		public Board board;
		#endregion
		public SideBoard player1_sideboard;
		public SideBoard player2_sideboard;
		public SideBoard GetSideBoard( PlayerId owner ) {
			return owner == PlayerId.Player1 ? player1_sideboard : player2_sideboard;
		}

		public bool manualOverride;
		private bool isGameOver;

		void Awake(){
			OnAnyCellClicked = ( _ ) => { };
			OnAnyPieceClicked = ( _ ) => { };
			OnPlayer1_PieceClicked = new RefAction<Piece>();
			OnPlayer2_PieceClicked = new RefAction<Piece>();
			RegisterPieceClickedEvents_Invocation();
		}

		void Start() {
			//TODO: black starts first
			BeginGame( PlayerId.Player1 );
		}

		void OnDisable(){
			isGameOver = true;
		}

		async UniTask BeginGame( PlayerId startingPlayer ) {
			board.InitWithPiecesInScene();
			player1_sideboard.InitWithPiecesInScene();
			player2_sideboard.InitWithPiecesInScene();

			( (MonoBehaviour)Player1 ).enabled = true;
			( (MonoBehaviour)Player2 ).enabled = true;
			
			_currPlayer_turn = startingPlayer;

			while(isGameOver == false && manualOverride == false){
				Debug.Log("Awaiting Turn: "+_currPlayer_turn.ToString());
				IShogiAction action = await CurrPlayer_turn.RequestAction();
				if (action.IsMoveValid( this )) {
					Debug.Log("Valid Move: Executing");
					await action.ExecuteAction( this );
					Debug.Log( "Finish Move Execution" );
				} else {
					Debug.Log("Invalid Action: Try again");
					continue;
				}

				AdvanceTurn();
			}
			Debug.Log( "Game Finished" );
		}

		

		private void AdvanceTurn() {
			_currPlayer_turn = (_currPlayer_turn == PlayerId.Player1) ? PlayerId.Player2 : PlayerId.Player1;
		}

		public void ApplyGameState(GameState state){
			ReassignPiecesData( state );
			
			_currPlayer_turn = state.currPlayerTurn;
			( (MonoBehaviour)Player1 ).enabled = false;
			( (MonoBehaviour)Player2 ).enabled = false;

			BeginGame( state.currPlayerTurn );
		}

		private void ReassignPiecesData( GameState obj ) {
			Queue<Piece> piecesObjs = FindObjectsOfType<Piece>().ToQueue();
			foreach(PieceData piece in obj.pieces){
				Piece currPieceObj = piecesObjs.Dequeue();
				currPieceObj.X = piece.x;
				currPieceObj.Y = piece.y;
				currPieceObj.OwnerId = piece.owner;
				currPieceObj.IsPromoted = piece.isPromoted;
				currPieceObj.IsCaptured = piece.isCaptured;
			}
			//Update sideboards
			
		}

		
	}
}
