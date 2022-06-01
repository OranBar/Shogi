using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi
{
	public class AnalysisBranching : MonoBehaviour
	{
		public ScrollRect scrollRect;
		public GameObject entryPrefab;
		private ShogiGame shogiGame;


		void Start(){
			shogiGame = FindObjectOfType<ShogiGame>();
			shogiGame.OnNewTurnBegun += CreateEntryForLastMove;
		}

		public void CreateEntryForLastMove(PlayerId id) { 
			if(shogiGame.gameHistory.playedMoves.Count != 0){
				GameObject newEntryObj = Instantiate( entryPrefab, scrollRect.content );
				AShogiAction lastMove = null;
				int currTurn = shogiGame.TurnCount;
				lastMove = shogiGame.gameHistory.playedMoves.Last();
				newEntryObj.GetComponent<AnalysisEntry>().InitEntry( currTurn, lastMove );
			} 
		}
	}
}
