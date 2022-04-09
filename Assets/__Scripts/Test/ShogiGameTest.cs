using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
    public class ShogiGameTest : MonoBehaviour
    {
		private const int WIDTH = 9, HEIGHT = 9;

		public BoardTest board;
		
		void Start()
        {
			AddPiecesFromScene();
		}

		void AddPiecesFromScene(){
			foreach(var piece in FindObjectsOfType<PieceTest>()){
				board.board [piece.X, piece.Y] = piece.piece;
			}
		}

		public void PlayAction(MovePieceAction action){
			board.UpdateBoard( action );
			action.piece.PieceMovementAnimation( action );

			Piece capturedPiece = board.board [action.destinationX, action.destinationY];
			bool wasCapturingMove = capturedPiece != null;

			if(wasCapturingMove){
				action.piece.PieceDeathAnimation();
			}

			PromoteIfPossible(action);
		}

		public void PromoteIfPossible(MovePieceAction action){
			if(action.destinationY >= 6){
				action.piece.Promote();
				Debug.Log( "Promoted" );
			}
		}
    }
}
