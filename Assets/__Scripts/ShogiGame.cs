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
		#region ToSerialize
		public Player gio;
		public Player oran;
		private PlayerId _currPlayer_turn;
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
			foreach(var piece in FindObjectsOfType<Piece>()) {
				PlacePiece( piece, piece.X, piece.Y );
			}
		}

		private void PlacePiece( Piece piece, int x, int y ) {
			board.board [x,y] = piece;
			piece.PlacePieceOnCell_Immediate( x, y );
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

		[ContextMenu("Save")]
		public void SaveGameState(){
			GameState gameState = new GameState();
			string json = JsonUtility.ToJson( gameState );
			Debug.Log(json);

			string path = Application.persistentDataPath + "/shogi.bin";
			gameState.SerializeToBinaryFile(path );
			// IFormatter formatter = new BinaryFormatter();
			// Stream stream = new FileStream( path, FileMode.Create, FileAccess.Write, FileShare.None );
			// formatter.Serialize( stream, gameState );
			// stream.Close();
			// Debug.Log( "File serialized at " + path );
			// string gameState_raw = gameState.GenerateBoardRepresentation();
			//Save to a file or somewhere 
		}

		[ContextMenu("Load bin")]
		public void LoadGameState(){
			string path = Application.persistentDataPath + "/shogi.bin";
			GameState obj = GameState.DeserializeFromBinaryFile( path );
			string json = JsonUtility.ToJson( obj );
			Debug.Log( json );
		}
		
		public void LoadGameState( string gameState_raw ) {
		}

		public void LoadGameState(GameState gameState){

		}

		
	}
}
