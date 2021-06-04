using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace API
{
    [System.Serializable]
    public class TerrainLine
    {
        public int index;
        public string[] terrain_line_data;

        public TerrainLine(int index, string[] terrain_line_data)
        {
            this.index = index;
            this.terrain_line_data = terrain_line_data;
        }
    }
}