using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Shogi
{
	public class ShogiGame : MonoBehaviour
	{
		#region Events
		// public RefAction<Cell> OnAnyCellClicked => Cell.OnAnyCellClicked;
		// public RefAction<Piece> OnAnyPieceClicked => Piece.OnAnyPieceClicked;
		public RefAction<Piece> OnPlayer1_PieceClicked = new RefAction<Piece>();
		public RefAction<Piece> OnPlayer2_PieceClicked = new RefAction<Piece>();
		public RefAction<PlayerId> OnNewTurnBegun = new RefAction<PlayerId>();
		public RefAction<IShogiAction> OnActionExecuted = new RefAction<IShogiAction>();

		public RefAction<Piece> Get_OnPieceClickedEvent( PlayerId player ) {
			return player == PlayerId.Player1 ? OnPlayer1_PieceClicked : OnPlayer2_PieceClicked;
		}

		void RegisterPieceClickedEvents_Invocation(){
			Piece.OnAnyPieceClicked += ( piece ) =>
			{
				if (piece.OwnerId == PlayerId.Player1) {
					OnPlayer1_PieceClicked.Value.Invoke( piece );
				} else {
					OnPlayer2_PieceClicked.Value.Invoke( piece );
				}
			};
		}

		
		#endregion

		public APlayer Player1;
		public APlayer Player2;
		#region ToSerialize

		private PlayerId _currTurn_PlayerId;
		public APlayer CurrTurn_Player {
			get
			{
				return GetPlayer( _currTurn_PlayerId );
			}
		}

		public APlayer GetPlayer(PlayerId playerId){
			return playerId == PlayerId.Player1 ? Player1 : Player2;
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
		[ReadOnly] public APlayer winner = null;

		private CancellationTokenSource gameLoopCancelToken;
		public GameHistory gameHistory = null;
		[ReadOnly] public GameSettings settings;
		private ShogiClock shogiClock;

		void Awake(){
			settings = FindObjectOfType<GameSettings>();
			shogiClock = FindObjectOfType<ShogiClock>();

			Cell.OnAnyCellClicked = new RefAction<Cell>();
			Piece.OnAnyPieceClicked = new RefAction<Piece>();
			// OnPlayer1_PieceClicked = new RefAction<Piece>();
			// OnPlayer2_PieceClicked = new RefAction<Piece>();
			// OnNewTurnBegun = new RefAction<PlayerId>();
			Debug.Log("Event init");
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

			if(shogiClock != null){
				shogiClock.timer_player1.OnTimerFinished -= Player2_HasWon;
				shogiClock.timer_player2.OnTimerFinished -= Player1_HasWon;
			}
		}

		public void BeginGame( PlayerId startingPlayer ) {
			gameLoopCancelToken?.Cancel();
			gameLoopCancelToken = new CancellationTokenSource();
			BeginGameAsync( startingPlayer ).AttachExternalCancellation( gameLoopCancelToken.Token );
		}

		private async UniTask BeginGameAsync( PlayerId startingPlayer ) {
			_currTurn_PlayerId = startingPlayer;

			RegisterTimeout_ToGameOver();

			Player1.enabled = false;
			Player2.enabled = false;
			Player1.enabled = true;
			Player2.enabled = true;

			OnNewTurnBegun.Invoke( _currTurn_PlayerId );
			while(isGameOver == false && manualOverride == false){
				Debug.Log("Awaiting Turn: "+_currTurn_PlayerId.ToString());
				IShogiAction action = await CurrTurn_Player.RequestAction().AttachExternalCancellation( gameLoopCancelToken.Token );
				
				if (action.IsMoveValid( this )) {
					Debug.Log("Valid Move: Executing");
					// gameHistory.playedMoves.LastOrDefault()?.DisableLastMoveFX();

					await action.ExecuteAction( this ).AttachExternalCancellation( gameLoopCancelToken.Token );
					// await action.EnableLastMoveFX( this.settings );

					if (action is not UndoLastAction) {
						gameHistory.RegisterNewMove( action );
					} else {
						//We don't want to register the UndoAction, and also we want to remove the last action. 
						gameHistory.playedMoves.Pop();
					}

					Debug.Log( "Finish Move Execution" );
				} else {
					Debug.Log("Invalid Action: Try again");
					continue;
				}

				AdvanceTurn();
				OnNewTurnBegun.Invoke( _currTurn_PlayerId );
			}
		}

		private void RegisterTimeout_ToGameOver() {
			var shogiClock = FindObjectOfType<ShogiClock>();
			shogiClock.timer_player1.OnTimerFinished += Player2_HasWon;
			shogiClock.timer_player2.OnTimerFinished += Player1_HasWon;
		}

		private void Player1_HasWon(){
			GameOver( GetPlayer( PlayerId.Player1 ) );
		}

		private void Player2_HasWon() {
			GameOver( GetPlayer( PlayerId.Player2 ) );
		}

		private void GameOver(APlayer winner){
			this.winner = winner;
			isGameOver = true;
			gameLoopCancelToken.Cancel();
			Debug.Log( "Game Finished" );
		}

		private void AdvanceTurn() {
			_currTurn_PlayerId = (_currTurn_PlayerId == PlayerId.Player1) ? PlayerId.Player2 : PlayerId.Player1;
		}

		public void ApplyGameState(GameState state) {
			ReassignPiecesData( state );
			RefreshMonobehavioursInScene();
		}

		public async UniTask ApplyGameHistory( GameHistory history ) {
			ApplyGameState( history.initialGameState );
			gameHistory = history;

			//Alter timescale to fast forward?
			IShogiAction prevMove = null;
			foreach (var move in history.playedMoves) {
				await move.ExecuteAction( this );
				prevMove = move;
			}

			PlayerId nextPlayerTurn = GetPlayer_WhoMovesNext( history );
			BeginGame( nextPlayerTurn );

			Debug.Log( "Finish Apply GameHistory: All moves applied" );

			PlayerId GetPlayer_WhoMovesNext( GameHistory loadedGameHistory ) {
				PlayerId nextPlayerTurn;
				if (loadedGameHistory.playedMoves.Count % 2 == 0) {
					nextPlayerTurn = loadedGameHistory.firstToMove;
				} else {
					nextPlayerTurn = loadedGameHistory.firstToMove == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;
				}

				return nextPlayerTurn;
			}
		}


		private void RefreshMonobehavioursInScene() {
			player1_sideboard.RefreshWithPiecesInScene();
			player2_sideboard.RefreshWithPiecesInScene();
			board.RefreshWithPiecesInScene();
		}

		private void ReassignPiecesData( GameState obj ) {
			Queue<Piece> piecesObjs = FindObjectsOfType<Piece>(true).ToQueue();
			foreach(PieceData piece in obj.piecesData){
				Piece currPieceObj = piecesObjs.Dequeue();
				currPieceObj.X = piece.x;
				currPieceObj.Y = piece.y;
				currPieceObj.OwnerId = piece.owner;
				currPieceObj.IsPromoted = piece.isPromoted;
				currPieceObj.IsCaptured = piece.isCaptured;

				currPieceObj.gameObject.SetActive( true );
			}
		}
	}
}
