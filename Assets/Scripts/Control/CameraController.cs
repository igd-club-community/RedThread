using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {
	public Vector3 minPosition = new Vector3(-18f, -12f, 5f);
	public Vector3 maxPosition = new Vector3(18f, 12f, 15f);

	public float mouseZoomSensivity = 1f;
	public float mouseSensivity = 1f;

	public float moveSpeed = 5.0f;
	public float maxSpeedXY = 20.0f;

	private Vector3 _targetPosition;

	private Vector3 _oldMousePos;
	private Vector3 _startPosition;
	private Vector3 _startRotation;

	void Awake() {
		minPosition += transform.position;
		maxPosition += transform.position;

		_targetPosition = transform.position;
		_oldMousePos = Input.mousePosition;
		_startPosition = transform.position;
		_startRotation = transform.rotation.eulerAngles;

	}
	
	void Update() {
		transform.rotation = Quaternion.identity;
		Move();
		transform.rotation = Quaternion.Euler(_startRotation);
	}

	private void Move() {
		if (Input.GetButtonDown("Focus")) {
			_targetPosition = _startPosition;
		} else {
			Vector3 newMovement = GetMovement();
			if (newMovement.x != 0 || newMovement.y != 0) {
				_targetPosition.x = transform.position.x + newMovement.x;
				_targetPosition.y = transform.position.y + newMovement.y;
			} 

			if (newMovement.z != 0) {
				_targetPosition.z = transform.position.z + newMovement.z;
			}
		}
		

		transform.position = Vector3.Lerp(transform.position, _targetPosition, moveSpeed * Time.deltaTime);

		float clampedX = Mathf.Clamp(transform.position.x, minPosition.x, maxPosition.x);
		float clampedY = Mathf.Clamp(transform.position.y, minPosition.y, maxPosition.y);
		float clampedZ = Mathf.Clamp(transform.position.z, minPosition.z, maxPosition.z);
		
		transform.position = new Vector3(clampedX, clampedY, clampedZ);
	}
	private float GetScrollWheelZ() {
		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

		if (mouseWheel == 0) {
			return 0f;
		} else {
			return mouseZoomSensivity * mouseWheel;
		}
	}

	private Vector3 GetMovement() {
		Vector3 movement = Vector3.zero;

		if (Input.GetMouseButtonDown(1)) {
			_targetPosition.x = transform.position.x;
			_targetPosition.y = transform.position.y;
			_oldMousePos = Input.mousePosition;
		}
		if (Input.GetMouseButton(1)) {
			movement = mouseSensivity * (_oldMousePos - Input.mousePosition);
		}
		movement = Vector3.ClampMagnitude(movement, maxSpeedXY);

		movement.z = GetScrollWheelZ();

		return movement;
	}
}
