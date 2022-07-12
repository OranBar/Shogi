using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi{


public class CrackFX : MonoBehaviour
{
		public TraumaInducer traumaInducer;
		void Start()
		{
			
		}

		// Update is called once per frame
		void Update()
		{
			
		}

		public void Activate() {
			gameObject.GetComponentInChildren<ParticleSystem>().Stop();
			gameObject.GetComponentInChildren<ParticleSystem>().Play();
			traumaInducer.enabled = false;
			traumaInducer.enabled = true;
		}
	}
}
