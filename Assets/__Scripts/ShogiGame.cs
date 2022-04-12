using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Shogi
{
	public class ShogiGame : MonoBehaviour
	{
		#region Events
		public static Action<Cell> OnAnyCellClicked = ( _ ) => { };
		public static Action<Piece> OnAnyPieceClicked = ( _ ) => { };
		#endregion

		#region ToSerialize
		public Player oran; //Player 1
		public Player gio; //Player 2
		private PlayerId _currPlayer_turn;
		public Player CurrPlayer_turn {
			get
			{
				return _currPlayer_turn == PlayerId.Player1 ? gio : oran;
			}
		}
		#endregion
		public Board board;


		void Start() {
			board.InitWithPiecesInScene();
			//TODO: black starts first
			_currPlayer_turn = PlayerId.Player1;
			BeginGame( _currPlayer_turn );
		}

		void BeginGame( PlayerId startingPlayer ) {
			_currPlayer_turn = startingPlayer;
			//Wait for player to call PlayAction
		}

		}

		public void PlayAction( IShogiAction action ) {
			if(action.GetPlayer(this) != CurrPlayer_turn){
				Debug.LogWarning("It's not your turn!");
				return;
			}

			if (action.IsMoveValid( this )) {
				action.ExecuteAction( this );
				//memento action
			}
		}

		// public void PlayAction(MovePieceAction action){
		// 	board.UpdateBoard( action );

		// 	Piece actingPiece = board [action.startX, action.startY];
		// 	actingPiece.PieceMovementAnimation( action );

		// 	Piece capturedPiece = board.board [action.destinationX, action.destinationY];
		// 	bool wasCapturingMove = capturedPiece != null && capturedPiece.owner != actingPiece.owner;

		// 	if(wasCapturingMove){
		// 		//A piece was killed. Such cruelty. 
		// 		capturedPiece.CapturePiece();
		// 	}
		// }


		// public void PromoteIfPossible(MovePieceAction action){
		// 	if(action.destinationY >= 6){
		// 		board[action.startX, action.startY].Promote();
		// 		Debug.Log( "Promoted" );
		// 	}
		// }

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
			//TODO: reassign data
		}

	}
}
