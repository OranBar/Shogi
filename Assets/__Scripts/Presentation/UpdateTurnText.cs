using System.Collections;
using System.Collections.Generic;
using Shogi;
using TMPro;
using UnityEngine;

public class UpdateTurnText : MonoBehaviour
{
	[Auto] public TMP_Text text;

	void Start(){
		FindObjectOfType<ShogiGame>().OnNewTurnBegun += UpdateText;
	}

	void UpdateText(PlayerId currTurn_PlayerId){
		text.text = "Moving: " + currTurn_PlayerId;
	}
}
