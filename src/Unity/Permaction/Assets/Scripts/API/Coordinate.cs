using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace API
{
    [System.Serializable]
    public class Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}