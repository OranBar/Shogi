using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi
{
    public class Main : MonoBehaviour
    {
        IEnumerator Start()
        {
			var shogiGame = FindObjectOfType<ShogiGame>(true);
			while(shogiGame.Player1 == null || shogiGame.Player2 == null){
				yield return new WaitForSeconds( 0.5f );
			}
			shogiGame.BeginGame(PlayerId.Player1);
		}
    }
}
