using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchAndZoom : MonoBehaviour
{
    float MouseZoomSpeed = 2.5f;
    float TouchZoomSpeed = 0.1f;
    float ZoomMinBound = 20f;
    float ZoomMaxBound = 75f;
    Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchSupported)
        {
            // Pinch to zoom
            if (Input.touchCount == 2)
            {

                // get current touch positions
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);
                // get touch position from the previous frame
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance (tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance (tZero.position, tOne.position);

                // get offset value
                float deltaDistance = oldTouchDistance - currentTouchDistance;
                Zoom (deltaDistance, TouchZoomSpeed);
            }
        }
        else
        {
            float scroll = Input.mouseScrollDelta.y;
            Zoom(scroll, MouseZoomSpeed);
        }



         if(cam.fieldOfView < ZoomMinBound) 
         {
             cam.fieldOfView = 0.1f;
         }
         else
         if(cam.fieldOfView > ZoomMaxBound ) 
         {
             cam.fieldOfView = 179.9f;
         }
    }

    void Zoom(float deltaMagnitudeDiff, float speed)
    {

        cam.fieldOfView += deltaMagnitudeDiff * speed;
        // set min and max value of Clamp function upon your requirement
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, ZoomMinBound, ZoomMaxBound);
    }
}
