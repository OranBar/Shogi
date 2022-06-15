using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Shogi
{
	public class OrbitRotator : MonoBehaviour
	{
		public float xSensitivity = 0.4f;
		public float ySensitivity = 0.2f;
		public int yMaxAngleOffset = 20;
		private Vector3 mouseReference;
		private Vector3 mouseOffset;
		private Vector3 rotation;
		private bool isRotating;
		private Tween rotationMagnetAnim;
		private Quaternion defaultRotation_quaternion;
		private Vector3 defaultRotation;
		public float magnetThreshold;
		public float magnetAnimSpeed = 1;

		void Start() {
			rotation = Vector3.zero;
			defaultRotation_quaternion = this.transform.rotation;
			defaultRotation = this.transform.eulerAngles;
		}

		void OnDisable() {
			isRotating = false;
		}

		void Update() {
			if (Input.GetMouseButtonDown( 0 )) {
				BeginDragRotation();
			} else if (Input.GetMouseButtonUp( 0 )) {
				EndDragRotation();
			} else if (isRotating) {
				DragRotate();
			}
		}

		private void DragRotate() {
			mouseOffset = ( Input.mousePosition - mouseReference );
			rotation.y = ( mouseOffset.x ) * xSensitivity;
			rotation.x = ( mouseOffset.y ) * ySensitivity;
			transform.Rotate( rotation );
			var tmp = this.transform.eulerAngles;
			tmp.z = 0;

			//We want to measue x from -180 to 180
			if(tmp.x > 180){
				tmp.x = tmp.x - 360;
			}
			tmp.x = Mathf.Clamp( tmp.x, defaultRotation.x - yMaxAngleOffset, defaultRotation.x + yMaxAngleOffset );

			//TODO limit rotation x 

			transform.eulerAngles = tmp;

			mouseReference = Input.mousePosition;

		}

		void BeginDragRotation() {
			isRotating = true;
			mouseReference = Input.mousePosition;
			rotationMagnetAnim.Kill();
		}

		void EndDragRotation() {
			isRotating = false;
			if(IsCloseToDefaultRotation()){
				rotationMagnetAnim = this.transform.DORotateQuaternion( defaultRotation_quaternion, magnetAnimSpeed );
			}
		}

		private bool IsCloseToDefaultRotation() {
			bool result = Mathf.Abs(Quaternion.Angle(this.transform.rotation, defaultRotation_quaternion)) < magnetThreshold;
			return result;
		}
	}
}
