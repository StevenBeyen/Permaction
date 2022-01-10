using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VisualImprovements : MonoBehaviour
{
    public Terrain terrain;
    public float grassRocksPerSquareMeter = 0.05f;
    public float grassRocksScaleOffset = 0.125f;
    public float grassRocksScaleRange = 0.75f;
    public double grassToRocksRatio = 0.7;
    public float treesPerSquareMeter = 0.0001f;
    public float treesScaleOffset = 0.75f;
    public float treesScaleRange = 1.0f;
    public int borderProtection = 3;

    private float heightCorrection = 1.5f;
    private int width, length, correctedWidth, correctedLength;
    private System.Random random;
    private string randomPrefabName;
    private GameObject prefab;
    private bool visual_improvements = false;

    // Start is called before the first frame update
    void Start()
    {
        MetaData.terrain = terrain;
        Vector3 terrain_size = terrain.terrainData.size;
        width = (int)terrain_size.x; // terrain width
        length = (int)terrain_size.z; // terrain length
        correctedWidth = width - 2 * borderProtection;
        correctedLength = length - 2 * borderProtection;
        random = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        if(UserData.elements_loaded && !visual_improvements)
        {
            RenderGrassAndRocks();
            RenderTrees();
            visual_improvements = true;
        }
    }

    void RenderGrassAndRocks()
    {
        for (int i = 0; i < grassRocksPerSquareMeter * width * length; ++i)
        {
            float randomWidth, randomLength, height;
            float randomScale;
            Vector3 position;
            randomWidth = borderProtection + (float) random.NextDouble() * width;
            randomLength = borderProtection + (float) random.NextDouble() * length;
            if (FreeCoordinates(randomWidth, randomLength))
            {
                height = terrain.SampleHeight(new Vector3(randomWidth, 0, randomLength)) + heightCorrection;
                position = new Vector3(randomWidth, height, randomLength);
                randomScale = (float) random.NextDouble() * grassRocksScaleRange + grassRocksScaleOffset;
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
                instantiatedPrefab.transform.RotateAround(position, Vector3.up, (float) random.NextDouble() * 360);
            }
        }
    }

    void RenderTrees()
    {
        for (int i = 0; i < treesPerSquareMeter * width * length; ++i)
        {
            float randomWidth, randomLength, height;
            float randomScale;
            Vector3 position;
            randomWidth = borderProtection + (float) random.NextDouble() * width;
            randomLength = borderProtection + (float) random.NextDouble() * length;
            if (FreeCoordinates(randomWidth, randomLength))
            {
                height = terrain.SampleHeight(new Vector3(randomWidth, 0, randomLength));
                position = new Vector3(randomWidth, height, randomLength);
                randomScale = (float) random.NextDouble() * treesScaleRange + treesScaleOffset;
                randomPrefabName = UserData.meta_data.prefab_trees[random.Next(UserData.meta_data.prefab_trees.Count)];
                prefab = Resources.Load(randomPrefabName) as GameObject;
                GameObject instantiatedPrefab = Instantiate(prefab, position, Quaternion.identity, Terrain.activeTerrain.transform);
                instantiatedPrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                instantiatedPrefab.transform.RotateAround(position, Vector3.up, (float) random.NextDouble() * 360);
            }
        }
    }

    private bool FreeCoordinates(float x, float z)
    {
        bool free = true;
        int index = 0;
        Vector2 x_bounds, z_bounds;
        while (free && index < UserData.physical_elements.Count)
        {
            x_bounds = UserData.physical_elements[index].GetXBounds();
            z_bounds = UserData.physical_elements[index].GetZBounds();
            if (x + borderProtection > x_bounds[0] && x - borderProtection < x_bounds[1])
                free = (z + borderProtection <= z_bounds[0] || z - borderProtection >= z_bounds[1]);
            else if (z + borderProtection > z_bounds[0] && z - borderProtection < z_bounds[1])
                free = (x + borderProtection <= x_bounds[0] || x - borderProtection >= x_bounds[1]);
            else
                free = ((x + borderProtection <= x_bounds[0] || x - borderProtection >= x_bounds[1]) && (z + borderProtection <= z_bounds[0] || z - borderProtection >= z_bounds[1]));
            ++index;
        }
        return free;
    }
}
