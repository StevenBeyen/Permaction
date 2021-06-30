using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InitTerrainElements : MonoBehaviour
{
    public Terrain terrain;
    public float elementsPerSquareMeter = 0.05f;
    public float randomScaleOffset = 0.25f;
    public float randomScaleRange = 1.5f;
    public double grassToRocksRatio = 0.7;
    public int borderProtection = 5;

    private float heightCorrection = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Terrain terrain = Terrain.activeTerrain;
        Vector3 terrain_size = terrain.terrainData.size;
        int width = (int)terrain_size.x; // terrain width
        int length = (int)terrain_size.z; // terrain length
        renderGrassAndRocks(terrain, width, length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void renderGrassAndRocks(Terrain terrain, int width, int length)
    {
        System.Random random = new System.Random();
        string randomPrefabName;
        GameObject prefab;
        int correctedWidth, correctedLength;
        correctedWidth = width - 2 * borderProtection;
        correctedLength = length - 2 * borderProtection;
        for (int i = 0; i < elementsPerSquareMeter * width * length; ++i)
        {
            float randomWidth, randomLength, height;
            float randomScale;
            Vector3 position;
            randomWidth = borderProtection + (float) random.NextDouble() * width;
            randomLength = borderProtection + (float) random.NextDouble() * length;
            height = terrain.SampleHeight(new Vector3(randomWidth, 0, randomLength)) + heightCorrection;
            position = new Vector3(randomWidth, height, randomLength);
            randomScale = (float) random.NextDouble() * randomScaleRange + randomScaleOffset;
            if (random.NextDouble() < grassToRocksRatio) // Grass
            {
                randomPrefabName = UserData.meta_data.prefab_grass[random.Next(UserData.meta_data.prefab_grass.Count)];
            } else // Rocks
            {
                randomPrefabName = UserData.meta_data.prefab_rocks[random.Next(UserData.meta_data.prefab_rocks.Count)];
            }
            prefab = Resources.Load(randomPrefabName) as GameObject;
            GameObject instantiatedPrefab = Instantiate(prefab, position, Quaternion.identity, Terrain.activeTerrain.transform);
            instantiatedPrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            instantiatedPrefab.transform.RotateAround(position, Vector3.up, (float)random.NextDouble() * 360);
        }
    }
}
