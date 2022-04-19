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
		public static RefAction<Piece> OnPlayer1_PieceClicked;
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
		#endregion
		public Board board;
		public SideBoard player1_sideboard;
		public SideBoard player2_sideboard;
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

		void OnDestroyed(){
			isGameOver = true;
		}

		async UniTask BeginGame( PlayerId startingPlayer ) {
			Awake();
			board.InitWithPiecesInScene();
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

		public async UniTask PlayAction( IShogiAction action ) {
			// if(action.GetPlayer(this) != CurrPlayer_turn){
			// 	Debug.LogWarning( "It's not your turn!" );
			// 	return;
			// }

			if (action.IsMoveValid( this )) {
				await action.ExecuteAction( this );
				//memento action
			}
		}

		[ContextMenu( "Save" )]
		public void SaveGameState() {
			GameState gameState = new GameState();
			string json = JsonUtility.ToJson( gameState );
			Debug.Log( json );

			string path = Application.persistentDataPath + "/shogi.bin";
			gameState.SerializeToBinaryFile( path );
		}

		[ContextMenu( "Load bin" )]
		public void LoadGameState() {
			string path = Application.persistentDataPath + "/shogi.bin";
			GameState obj = GameState.DeserializeFromBinaryFile( path );
			string json = JsonUtility.ToJson( obj );
			Debug.Log( json );
			
			ReassignPiecesPositions(obj);
			_currPlayer_turn = obj.currPlayerTurn;
			BeginGame( obj.currPlayerTurn );
		}

		private void ReassignPiecesPositions( GameState obj ) {
			Queue<Piece> piecesObjs = FindObjectsOfType<Piece>().ToQueue();
			foreach(PieceData piece in obj.pieces){
				Piece currPieceObj = piecesObjs.Dequeue();
				currPieceObj.X = piece.x;
				currPieceObj.Y = piece.y;
				currPieceObj.OwnerId = piece.owner;
				currPieceObj.IsCaptured = piece.isCaptured;
				currPieceObj.IsPromoted = piece.isPromoted;
			}
		}
	}
}
