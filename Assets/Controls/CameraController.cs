using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class that represents camera and its behaviour in play mode
 */

public class CameraController : MonoBehaviour {

	public float horizontalSpeed = 40;
	public float verticalSpeed = 40;
	public float cameraRotateSpeed = 80;
	public float cameraDistance = 30;
	public float zoomSpeed =  80;

	float curDistance;

	// Update is called once per frame
	void Update() {
		float horizontal = Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime;
		float vertical = Input.GetAxis("Vertical") * verticalSpeed * Time.deltaTime;
		float rotation = Input.GetAxis("Rotation");

		transform.Translate(Vector3.forward * vertical);
		transform.Translate(Vector3.right * horizontal);

		if (rotation != 0) {
			transform.Rotate(Vector3.up, rotation * cameraRotateSpeed * Time.deltaTime);
		}
		if (Input.GetMouseButton (2)) {
			transform.Rotate(Vector3.up, Input.GetAxis("Mouse X")  * cameraRotateSpeed * Time.deltaTime);
		}

		if (Input.GetMouseButton (1) && Input.GetMouseButton(0)) {
			float h = Input.GetAxis("Mouse X") * horizontalSpeed * Time.deltaTime;
			float v = Input.GetAxis("Mouse Y") * verticalSpeed * Time.deltaTime;	
			transform.Translate(Vector3.back * v);
			transform.Translate(Vector3.left * h);
		}
		if (Input.mouseScrollDelta.magnitude > 0) {
			float z = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
			transform.Translate (Vector3.up * -z);
		}
	}
}

