using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class CameraControls : MonoBehaviour
	{

		[SerializeField] private float _minZoom = 5, _maxZoom = 3, _smoothTime = 0.3f, _zoomSpeed = 4;
		private float _currSize, _targetZoom, _zoomVelocity = 0;
		private Camera _cam;
		private GameManager _gameManager;

		private void Awake() 
		{
			_cam = GetComponent<Camera>();
			_currSize = _targetZoom = _cam.orthographicSize;
			_gameManager = FindObjectOfType<GameManager>();
		}

		private void Update() 
		{
			if (_gameManager.GetGameState() != GameState.Menu)
			{
				_targetZoom = Mathf.Clamp(_targetZoom - _zoomSpeed * Input.GetAxis("Mouse ScrollWheel"), _maxZoom, _minZoom);
			}
			

			if (Mathf.Abs(_targetZoom - _currSize) >= 0.01)
			{
				_currSize = Mathf.SmoothDamp(_currSize, _targetZoom, ref _zoomVelocity, _smoothTime);
				_cam.orthographicSize = _currSize;
			}
		}
	}
