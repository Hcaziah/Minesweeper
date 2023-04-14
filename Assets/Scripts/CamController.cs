using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {
	private float zoomLevel = 6f;
	private float moveSpeed = 5f;
	[SerializeField] private float dragSpeed = 1f;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		// If the middle mouse button is held down, drag the camera
		if (Input.GetMouseButton(2)) {
			// Get the mouse input
			float mouseX = Input.GetAxis("Mouse X");
			float mouseY = Input.GetAxis("Mouse Y");

			// Move the camera in the opposite direction of the mouse input
			transform.position -= new Vector3(mouseX, mouseY, 0f) * dragSpeed * Time.deltaTime;
		}
		// Get the scroll input from the mouse wheel
		float scroll = Input.GetAxis("Mouse ScrollWheel");

		// If the scroll input is not zero, adjust the zoom level accordingly
		if (scroll != 0f) {
			zoomLevel -= scroll * 500f * Time.deltaTime; // Adjust the zoom level based on the mouse scroll input
			zoomLevel = Mathf.Clamp(zoomLevel, 1f, 6f); // Clamp the zoom level to a minimum of 1 and a maximum of 6
		}

		// Set the camera's orthographic size based on the zoom level
		Camera.main.orthographicSize = zoomLevel;

		// Get the horizontal and vertical input from the keyboard
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		// Move the camera in the horizontal and vertical directions based on the input and moveSpeed
		transform.position += new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;
	}

}
