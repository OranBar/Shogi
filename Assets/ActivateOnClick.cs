using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ActivateOnClick : MonoBehaviour
{
	[Auto] CinemachineFreeLook cinemachine;
	// Start is called before the first frame update
	void Start()
    {
		cinemachine.enabled = false;
	}

	void Update(){
		if(Input.GetButtonDown("Fire1")){
			cinemachine.enabled = true;
		}
		else if( Input.GetButtonUp( "Fire1" ) ) {
			cinemachine.enabled = false;
		}
	}
}
