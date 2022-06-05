using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitApplication : MonoBehaviour
{
	[Auto] private Button button;

	public void OnEnable(){
		button.onClick.AddListener( QuitApplication );
	}

	public void OnDisable(){
		button.onClick.RemoveListener( QuitApplication );
	}
	private void QuitApplication() {
		Application.Quit();
	}
}
