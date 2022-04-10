using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shogi{
	public class RotateAllChildren : MonoBehaviour
	{
		void OnTransformChildrenChanged(){
			foreach(Transform child in transform){
				child.SetLocalRotationZ(180);;
			}
		}
	}
}
