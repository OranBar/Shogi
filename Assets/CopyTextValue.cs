using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CopyTextValue : MonoBehaviour
{
	public TMP_Text target;
	[Auto] private TMP_Text myText;

	void Update()
    {
		if(myText.text != target.text){
			myText.text = target.text;
		}
    }
}
