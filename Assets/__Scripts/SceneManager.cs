using System.Collections;
using System.Collections.Generic;
using BarbarO.Utils;
using UnityEngine;

public enum ShogiSceneStates{
	None,
	Initialization,
	PlayingGame
	
}

public enum PlayerActions{
	ReloadGame_AtGameState,

}

namespace Shogi
{
	[ScriptTiming(500)]
    public class SceneManager : MonoBehaviour
    {
		public RefAction OnSceneInitialized;
		public RefAction OnGameStarted;
		private ShogiGame shogiGame;
		private AnalysisBranchingManager analysisBranchingManager;

		void Awake()
        {
			shogiGame = FindObjectOfType<ShogiGame>();
			analysisBranchingManager = FindObjectOfType<AnalysisBranchingManager>();
			
			Debug.Assert(analysisBranchingManager != null);
		}

		protected virtual void Start(){
			StartCoroutine( OnInitialization_Enter() );
		}

		protected IEnumerator OnInitialization_Enter(){

			yield return new WaitUntil( () => shogiGame.Player1 != null && shogiGame.Player2 != null );
			analysisBranchingManager.enabled = true;
		}

    }
}
