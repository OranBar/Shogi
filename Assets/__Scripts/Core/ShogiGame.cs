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
		public RefAction<Piece> OnPlayer1_PieceClicked = new RefAction<Piece>();
		public RefAction<Piece> OnPlayer2_PieceClicked = new RefAction<Piece>();
		public RefAction<PlayerId> OnNewTurnBegun = new RefAction<PlayerId>();
		public RefAction<AShogiAction> OnActionExecuted = new RefAction<AShogiAction>();
		public RefAction<AShogiAction> OnBeforeActionExecuted = new RefAction<AShogiAction>();

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

		[SerializeField] private PlayerId _currTurn_PlayerId;
		public APlayer CurrTurn_Player {
			get
			{
				return GetPlayer( _currTurn_PlayerId );
			}
		}

		public APlayer GetPlayer(PlayerId playerId){
			return playerId == PlayerId.Player1 ? Player1 : Player2;
		}

		public ABoard board;
		#endregion
		public SideBoard player1_sideboard;
		public SideBoard player2_sideboard;
		public SideBoard GetSideBoard( PlayerId owner ) {
			return owner == PlayerId.Player1 ? player1_sideboard : player2_sideboard;
		}

		public Piece[] AllPieces => FindObjectsOfType<Piece>();

		public bool beginGameOnStart = true;

		public bool manualOverride;
		private bool isGameOver;
		[ReadOnly] public APlayer winner = null;

		public CancellationTokenSource gameLoopCancelToken;
		public GameHistory gameHistory = null;
		[ReadOnly] public ShogiGameSettings settings;
		[ReadOnly] public ShogiClock shogiClock;
		public PlayerId startingPlayer;

		public int TurnCount => gameHistory.playedMoves.Count + 1;

		void Awake(){
			settings = FindObjectOfType<ShogiGameSettings>();
			shogiClock = FindObjectOfType<ShogiClock>();
			startingPlayer = PlayerId.Player1;
			gameHistory = new GameHistory( new GameState( this ), startingPlayer, this );

			RegisterPieceClickedEvents_Invocation();
		}

		void Start() {
			if(beginGameOnStart){
				BeginGame( startingPlayer );
			}
		}

		void OnDisable(){
			isGameOver = true;

			if(shogiClock != null){
				shogiClock.timer_player1.OnTimerFinished -= Player2_HasWon;
				shogiClock.timer_player2.OnTimerFinished -= Player1_HasWon;
			}
		}

		public void BeginGame( PlayerId startingPlayer ) {
			Debug.Log("Beginning Shogi Game "+startingPlayer.ToString());
			gameLoopCancelToken?.Cancel();
			gameLoopCancelToken = new CancellationTokenSource();
			BeginGameAsync( startingPlayer ).AttachExternalCancellation( gameLoopCancelToken.Token );
		}

		private async UniTask BeginGameAsync( PlayerId startingPlayer ) {
			_currTurn_PlayerId = startingPlayer;
			RegisterGameOver_OnClockTimeout();

			RefreshMonobehavioursInScene();

			Player1.enabled = false;
			Player2.enabled = false;
			Player1.enabled = true;
			Player2.enabled = true;

			OnNewTurnBegun.Invoke( _currTurn_PlayerId );
			while(isGameOver == false && manualOverride == false){
				Debug.Log($"Turn {TurnCount}. Awaiting Move from : "+_currTurn_PlayerId.ToString());
				AShogiAction action = await CurrTurn_Player.RequestAction().AttachExternalCancellation( gameLoopCancelToken.Token );
				
				if (action.IsMoveValid( this )) {
					Debug.Log("Valid Move: Executing");
					await ExecuteAction_AndCallEvents( action ).AttachExternalCancellation( gameLoopCancelToken.Token );

					gameHistory.RegisterNewMove( action );
					
					Debug.Log( "Finish Move Execution" );
				} else {
					Debug.Log("Invalid Action: Try again");
					continue;
				}

				AdvanceTurn();
				OnNewTurnBegun.Invoke( _currTurn_PlayerId );
			}
		}

		public async UniTask ExecuteAction_AndCallEvents( AShogiAction action ){
			OnBeforeActionExecuted.Invoke( action );
			await action.ExecuteAction( this ).AttachExternalCancellation( gameLoopCancelToken.Token );
			OnActionExecuted.Invoke( action );
		}

		private void RegisterGameOver_OnClockTimeout() {
			var shogiClock = FindObjectOfType<ShogiClock>();
			shogiClock.timer_player1.OnTimerFinished -= Player2_HasWon;
			shogiClock.timer_player2.OnTimerFinished -= Player1_HasWon;
			
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

			//Alter timescale to fast forward?
			foreach (var move in history.playedMoves) {
				await move.ExecuteAction( this );
				gameHistory.RegisterNewMove( move );
			}

			Restore_ClockTimers_Values( history );

			PlayerId nextPlayerTurn = history.GetPlayer_WhoMovesNext();
			BeginGame( nextPlayerTurn );

			Debug.Log( "Finish Apply GameHistory: All moves applied" );

			#region ApplyGameHistory Local Methods
			void Restore_ClockTimers_Values( GameHistory history ) {
				(float player1_time, float player2_time) timers_clockValues = history.timersHistory.Last();
				shogiClock.timer_player1.clockTime = timers_clockValues.player1_time;
				shogiClock.timer_player2.clockTime = timers_clockValues.player2_time;
			}

			#endregion
		}


		[Button]
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
