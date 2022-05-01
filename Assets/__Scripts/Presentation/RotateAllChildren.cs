using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi{
	public class RotateAllChildren : MonoBehaviour
	{
		public Vector3 targetLocalRotation;

		void OnTransformChildrenChanged(){
			foreach(Transform child in transform){
				child.localEulerAngles = targetLocalRotation;
			}
		}
	}
}
