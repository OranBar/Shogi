using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CopyColor : MonoBehaviour
{
	public Image target;
	[Auto] private TMP_Text mine;
	
    // Update is called once per frame
    void Update()
    {
		if(target.color != mine.color){
			mine.color = target.color;
		}
    }
}
