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
				IShogiAction lastMove = shogiGame.gameHistory.playedMoves.Last();
				GameObject newEntryObj = Instantiate( entryPrefab, scrollRect.content );
				newEntryObj.GetComponent<AnalysisEntry>().SetEntryText(shogiGame.gameHistory.playedMoves.Count, lastMove.ToString());
			}
		}

	}
}
