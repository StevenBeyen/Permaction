using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphical {
    public class ArcLink
    {
        private float angle = 60;
        private int resolution = 100;

        private float gravity = Mathf.Abs(Physics.gravity.y);

        private LineRenderer lr;
        private float radianAngle;
        private float distance;
        private float velocity;
        private float xRatio;
        private float zRatio;

        private float x0;
        private float y0;
        private float z0;
        private float y1;
        private float xDirection = 1;
        private float zDirection = 1;

        public ArcLink(GameObject arcLink, Vector3 source, Vector3 destination)
        {
            lr = arcLink.GetComponent<LineRenderer>();
            radianAngle = Mathf.Deg2Rad * angle;
            float xDistance = Mathf.Abs(source.x - destination.x);
            float zDistance = Mathf.Abs(source.z - destination.z);
            distance = xDistance + zDistance;
            velocity = Mathf.Sqrt(distance * gravity / Mathf.Sin(2 * radianAngle));
            xRatio = xDistance / (xDistance + zDistance);
            zRatio = zDistance / (xDistance + zDistance);
            x0 = source.x;
            y0 = source.y;
            z0 = source.z;
            if (source.x > destination.x)
                xDirection = -1;
            if  (source.z > destination.z)
                zDirection = -1;
            y1 = destination.y;
            RenderArc();
        }

        private void RenderArc()
        {
            lr.positionCount = resolution + 1;
            lr.SetPositions(ArcArray());
        }

        private Vector3[] ArcArray()
        {
            Vector3[] arcArray = new Vector3[resolution + 1];
            float t;
            for (int i = 0; i <= resolution; ++i)
            {
                t = (float)i / (float)resolution;
                arcArray[i] = ArcPoint(t);
            }
            return arcArray;
        }

        private Vector3 ArcPoint(float t)
        {
            float currentDistance = t * distance;
            float x = x0 + xDirection * currentDistance * xRatio;
            float y = y0 * (1 - t) + y1 * t + currentDistance * Mathf.Tan(radianAngle) - ((gravity * currentDistance * currentDistance)/(2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
            float z = z0 + zDirection * currentDistance * zRatio;
            return new Vector3(x, y, z);
        }
    }
}
