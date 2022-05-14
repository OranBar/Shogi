using System.Collections;
using System.Collections.Generic;
using BarbarO.ExtensionMethods;
using NaughtyAttributes;
using UnityEngine;

namespace Shogi
{
	public class GameHistoryDebugger : MonoBehaviour
	{
		[Button]
		public void LogGameHistory_Moves() {
			var gameHistory = FindObjectOfType<ShogiGame>().gameHistory;
			Debug.Log( gameHistory.playedMoves.ToStringPretty() );
		}
	}
}