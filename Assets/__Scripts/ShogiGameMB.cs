using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Shogi
{
    public class ShogiGameMB : MonoBehaviour
    {
		#region ToSerialize
		public Player gio;
		public Player oran;
		public PlayerId _currPlayer_turn;
		public Player CurrPlayer_turn {
			get{
				return _currPlayer_turn == PlayerId.Player1 ? gio : oran;
			}
		}
		//We will want to serialize all pieces. Pieces is all the data we need
		#endregion
		public Board board;


		void Start()
        {
			Debug.Assert( FindObjectsOfType<Player>().Length <= 2 );
			AddPiecesFromScene();
		}

		void AddPiecesFromScene(){
			foreach(var piece in FindObjectsOfType<Piece>()){
				board.board [piece.X, piece.Y] = piece;
			}
		}

		public void PlayAction(MovePieceAction action){
			board.UpdateBoard( action );
			action.piece.PieceMovementAnimation( action );

			Piece capturedPiece = board.board [action.destinationX, action.destinationY];
			bool wasCapturingMove = capturedPiece != null;

			if(wasCapturingMove){
				// capturedPiece.PieceDeathAnimation();
			}

			PromoteIfPossible(action);
		}

		public void PromoteIfPossible(MovePieceAction action){
			if(action.destinationY >= 6){
				action.piece.Promote();
				Debug.Log( "Promoted" );
			}
		}

		public void SaveGameState(){
			GameState gameState = new GameState(FindObjectsOfType<Piece>());
			// string gameState_raw = gameState.GenerateBoardRepresentation();
			//Save to a file or somewhere 
		}

		public void LoadGameState( string gameState_raw ) {
			// GameState gameState = new GameState( gameState_raw );
			// LoadGameState( gameState );
		}

		public void LoadGameState(GameState gameState){

		}

		
	}
}
