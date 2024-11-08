﻿using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour 
{
	//
	// VARIABLES
	//
	
	public Terrain terrain;
	public float turnSpeed = 4.0f;		// Speed of camera turning when mouse moves in along an axis
	public float panSpeed = 3.0f;		// Speed of the camera when being panned
	public float zoomSpeed = 3.0f;		// Speed of the camera going back and forth
	public float responsiveness = 10.0f;	// Responsiveness for the smoothing applied to the camera height
	public float cameraHeight = 20.0f;	// Height of the camera depending on terrain height
	public float cameraLimit = 15.0f;
	
	private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts
	private bool isPanning;		// Is the camera being panned?
	private bool isRotating;	// Is the camera being rotated?
	private bool isZooming;		// Is the camera zooming?

	private float xMinLimit;
	private float xMaxLimit;
	private float zMinLimit;
	private float zMaxLimit;
	
	void Start()
	{
		Vector3 terrainSize = terrain.terrainData.size;
		xMinLimit = -cameraLimit;
		xMaxLimit = terrainSize.x + cameraLimit;
		zMinLimit = -cameraLimit;
		zMaxLimit = terrainSize.z + cameraLimit;
	}

	void Update () 
	{	
		// Get the left mouse button
		if(Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isPanning = true;
			isRotating = false;
		}

		// Get the right mouse button
		if(Input.GetMouseButtonDown(1))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isRotating = true;
			isPanning = false;
		}
		
		// Disable movements on button release
		if (!Input.GetMouseButton(0))
		{
			isPanning = false;
		}
		if (!Input.GetMouseButton(1))
		{
			isRotating = false;
		}
		
		// Move the camera on its XZ plane
		if (isPanning)
		{
	        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

			// Camera drag on XZ plane
			Vector3 moveX = - Vector3.right * pos.x * panSpeed;
			transform.Translate(moveX, Space.Self);
	        Vector3 moveZ = - Vector3.forward * pos.y * panSpeed;
			transform.Translate(moveZ, Space.Self);

			// Camera height depending on terrain height
			Vector3 camPos2 = transform.position;
			float groundLevel = terrain.SampleHeight(camPos2);
			camPos2.y = groundLevel + cameraHeight;
			transform.position = camPos2;
		}

		// Rotate camera along Y axis
		if (isRotating)
		{
	        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

			transform.Rotate(0, - pos.x * turnSpeed, 0, Space.World);
			transform.Rotate(pos.y * turnSpeed, 0, 0, Space.Self);
		}

		// Movement limits
		Vector3 camPos = transform.position;
		if (camPos.x < xMinLimit)
		{
			camPos.x = xMinLimit;
			transform.position = camPos;
		}
		if (camPos.x > xMaxLimit)
		{
			camPos.x = xMaxLimit;
			transform.position = camPos;
		}
		if (camPos.z < zMinLimit)
		{
			camPos.z = zMinLimit;
			transform.position = camPos;
		}
		if (camPos.z > zMaxLimit)
		{
			camPos.z = zMaxLimit;
			transform.position = camPos;
		}
	}
}