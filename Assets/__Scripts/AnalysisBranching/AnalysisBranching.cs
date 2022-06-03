using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Shogi
{
	public class AnalysisBranching : MonoBehaviour
	{
		public ScrollRect scrollRect;
		public GameObject entryPrefab;
		private ShogiGame shogiGame;

		private List<AnalysisEntry> entries = new List<AnalysisEntry>();

		void Awake(){
			shogiGame = FindObjectOfType<ShogiGame>();
		}

		void OnEnable(){
			shogiGame.OnActionExecuted += CreateEntryForLastMove;
		}

		void OnDisable(){
			shogiGame.OnActionExecuted -= CreateEntryForLastMove;
		}

		public void CreateEntryForLastMove(AShogiAction lastMove) { 
			Debug.Log("here");
			GameObject newEntryObj = Instantiate( entryPrefab, scrollRect.content );
			int currTurn = shogiGame.TurnCount;
			newEntryObj.GetComponent<AnalysisEntry>().InitEntry( currTurn, lastMove );
			entries.Add( newEntryObj.GetComponent<AnalysisEntry>() );
		}
	}

	
}
