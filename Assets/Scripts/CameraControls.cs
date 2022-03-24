using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class CameraControls : MonoBehaviour
	{

		[SerializeField] float MINSIZE = 5, MAXSIZE = 3, SMOOTHTIME = 0.3f, zoomSpeed = 4;
		float currSize, targetSize, zoomVelocity = 0;
		Camera cam;

		void Awake() 
		{
			cam = GetComponent<Camera>();
			currSize = targetSize = cam.orthographicSize;
		}

		private void Update() 
		{
			targetSize = Mathf.Clamp(targetSize - zoomSpeed * Input.GetAxis("Mouse ScrollWheel"), MAXSIZE, MINSIZE);

			if (Mathf.Abs(targetSize - currSize) < 0.01) return;

			currSize = Mathf.SmoothDamp(currSize, targetSize, ref zoomVelocity, SMOOTHTIME);
			cam.orthographicSize = currSize;
		}
	}
