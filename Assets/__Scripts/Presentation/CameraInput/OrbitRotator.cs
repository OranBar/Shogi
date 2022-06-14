using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Shogi
{
	public class OrbitRotator : MonoBehaviour
	{
		public float sensitivity = 0.4f;
		private Vector3 mouseReference;
		private Vector3 mouseOffset;
		private Vector3 rotation;
		private bool isRotating;
		private Tween rotationMagnetAnim;
		private Quaternion defaultRotationValue;
		public float magnetThreshold;
		public float magnetAnimSpeed = 1;

		void Start() {
			rotation = Vector3.zero;
			defaultRotationValue = this.transform.rotation;
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
			rotation.y = -( mouseOffset.x + mouseOffset.y ) * sensitivity;
			transform.Rotate( rotation );

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
				rotationMagnetAnim = this.transform.DORotateQuaternion( defaultRotationValue, magnetAnimSpeed );
			}
		}

		private bool IsCloseToDefaultRotation() {
			bool result = Mathf.Abs(Quaternion.Angle(this.transform.rotation, defaultRotationValue)) < magnetThreshold;
			return result;
		}
	}
}
