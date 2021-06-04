using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHeightmap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!UserData.terrain_loaded)
        {
            // Set up dot instead of comma for float => ToString
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            string[][] heightmap;
            int heightmapResolution = Terrain.activeTerrain.terrainData.heightmapResolution;
            float[,] rawHeightmap = Terrain.activeTerrain.terrainData.GetHeights(0,0,heightmapResolution,heightmapResolution); // y,x to convert to x,z !
            heightmap = new string[heightmapResolution][];
            for (int x=0; x<heightmapResolution; ++x)
            {
                heightmap[x] = new string[heightmapResolution];
                for (int z=0; z<heightmapResolution; ++z)
                {
                    heightmap[x][z] = rawHeightmap[z,x].ToString();
                }
            }
            UserData.terrain_heightmap = heightmap;
            UserData.terrain_loaded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
