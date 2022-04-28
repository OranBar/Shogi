using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object; 

namespace Shogi
{
	public class ShogiGame : Sirenix.OdinInspector.SerializedMonoBehaviour
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

		public IPlayer Player1;
		public IPlayer Player2;
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

		public Piece[] AllPieces => FindObjectsOfType<Piece>();

		public bool manualOverride;
		private bool isGameOver;
		private CancellationTokenSource gameLoopCancelToken;
		public GameHistory gameHistory = null;

		void Awake(){
			OnAnyCellClicked = ( _ ) => { };
			OnAnyPieceClicked = ( _ ) => { };
			OnPlayer1_PieceClicked = new RefAction<Piece>();
			OnPlayer2_PieceClicked = new RefAction<Piece>();
			RegisterPieceClickedEvents_Invocation();
		}

		void Start() {
			RefreshMonobehavioursInScene();

			var startingPlayer = PlayerId.Player1;
			gameHistory = new GameHistory( new GameState( this ), startingPlayer );
			BeginGame( startingPlayer );
		}

		void OnDisable(){
			isGameOver = true;
		}

		public void BeginGame( PlayerId startingPlayer ) {
			gameLoopCancelToken?.Cancel();
			gameLoopCancelToken = new CancellationTokenSource();
			BeginGameAsync( startingPlayer ).AttachExternalCancellation( gameLoopCancelToken.Token );
		}

		private async UniTask BeginGameAsync( PlayerId startingPlayer ) {
			_currPlayer_turn = startingPlayer;
			// gameHistory = new GameHistory( new GameState( this ), startingPlayer );

			( (MonoBehaviour)Player1 ).enabled = false;
			( (MonoBehaviour)Player2 ).enabled = false;
			( (MonoBehaviour)Player1 ).enabled = true;
			( (MonoBehaviour)Player2 ).enabled = true;
			
			while(isGameOver == false && manualOverride == false){
				Debug.Log("Awaiting Turn: "+_currPlayer_turn.ToString());
				IShogiAction action = await CurrPlayer_turn.RequestAction().AttachExternalCancellation( gameLoopCancelToken.Token );
				if (action.IsMoveValid( this )) {
					Debug.Log("Valid Move: Executing");
					await action.ExecuteAction( this ).AttachExternalCancellation( gameLoopCancelToken.Token );

					gameHistory.RegisterNewMove(action);

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

		//? Forse questa funzione dovrei metterle in LoadSave_GameState
		public void ApplyGameState(GameState state) {
			ReassignPiecesData( state );
			RefreshMonobehavioursInScene();
		}

		private void RefreshMonobehavioursInScene() {
			player1_sideboard.RefreshWithPiecesInScene();
			player2_sideboard.RefreshWithPiecesInScene();
			board.RefreshWithPiecesInScene();
		}

		//? Forse questa funzione dovrei metterle in LoadSave_GameState
		private void ReassignPiecesData( GameState obj ) {
			Queue<Piece> piecesObjs = FindObjectsOfType<Piece>().ToQueue();
			foreach(PieceData piece in obj.piecesData){
				Piece currPieceObj = piecesObjs.Dequeue();
				currPieceObj.X = piece.x;
				currPieceObj.Y = piece.y;
				currPieceObj.OwnerId = piece.owner;
				currPieceObj.IsPromoted = piece.isPromoted;
				currPieceObj.IsCaptured = piece.isCaptured;
			}
		}
	}
}
