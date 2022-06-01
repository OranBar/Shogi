using TMPro;
using UnityEngine;

public class AnalysisEntry : MonoBehaviour
{
	public TMP_Text numberText;
	public TMP_Text moveText;
	
	public void SetEntryText( int moveNumber, string moveRepresentation ) {
		numberText.text = moveNumber.ToString();
		moveText.text = moveRepresentation;
	}
}
