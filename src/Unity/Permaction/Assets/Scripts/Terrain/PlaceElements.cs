using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using API;
using Graphical;

public class PlaceElements : MonoBehaviour
{
    public GameObject billboard;
    public GameObject arcLink;

    // Start is called before the first frame update
    void Start()
    {
        // Cloning the terrain so that all runtime changes are cancelled on exit
        CloneTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        if (UserData.reply != null && !UserData.elements_loaded)
        {
            LoadElements();
        }
    }

    void CloneTerrain()
    {
        Terrain.activeTerrain.terrainData = TerrainDataCloner.Clone(Terrain.activeTerrain.terrainData);
        Terrain.activeTerrain.GetComponent<TerrainCollider>().terrainData = Terrain.activeTerrain.terrainData; // Don't forget to update the TerrainCollider as well
    }

    void LoadElements()
    {
        string prefab_name;
        int prefab_fixed_size_value;
        GameObject prefab;
        Vector3 base_position;
        Vector3 real_position;
        Vector3 rotation_offset;
        Vector3 scale;
        float rotation;
        foreach(Element e in UserData.reply.result)
        {
            GameObject instantiatedGOContainer = new GameObject(e.name); // TODO Check on server-side if name is sent back (probably not)
            instantiatedGOContainer.transform.SetParent(Terrain.activeTerrain.transform);
            instantiatedGOContainer.AddComponent<Graphical.GraphicalElement>();
            BoxCollider boxCollider = instantiatedGOContainer.AddComponent<BoxCollider>();
            e.Sync();
            scale = e.GetScale();
            UserData.meta_data.prefab_mapping.TryGetValue(e.id, out prefab_name);
            prefab = Resources.Load(prefab_name) as GameObject;
            base_position = e.GetPosition();
            // Postprocessing: converting base position height (0,1 value) in terrain height
            base_position = new Vector3(base_position.x, base_position.y * Terrain.activeTerrain.terrainData.size.y, base_position.z);
            // Flattening terrain on the area where the element will be placed
            Terrain.activeTerrain.terrainData.SetHeights((int) base_position.x, (int) base_position.z, e.GetHeights());
            // Switch case depending on type of prefab object: stretched or copied
            // Case 1: element that has to be copied to cover the given terrain coordinates
            if (UserData.meta_data.prefab_fixed_size_values.TryGetValue(e.id, out prefab_fixed_size_value))
            {
                // Objects
                rotation_offset = new Vector3(scale.x/2.0f, 0, scale.z/2.0f);
                //rotation_offset = new Vector3(prefab_fixed_size_value/2.0f, 0, prefab_fixed_size_value/2.0f);
                for (int x=0; (x+prefab_fixed_size_value-1)<scale.x; x+=prefab_fixed_size_value) {
                    for (int z=0; (z+prefab_fixed_size_value-1)<scale.z; z+=prefab_fixed_size_value) {
                        real_position = base_position + new Vector3(x,0,z);
                        //rotation = Random.Range(-2,2) * 90;
                        GameObject instantiatedGO = Instantiate(prefab, real_position, Quaternion.identity, instantiatedGOContainer.transform);
                        //instantiatedGO.transform.RotateAround(real_position + rotation_offset, Vector3.up, rotation);
                    }
                }
                Vector3 center = base_position + new Vector3(scale.x/2.0f, 0, scale.z/2.0f);
                // Box collider on container
                boxCollider.center = center;
                boxCollider.size = scale;
            } else // Case 2: stretchable terrain object
            {
                // Object itself
                rotation_offset = new Vector3(scale.x/2.0f, 0, scale.z/2.0f);
                rotation = Random.Range(0,2) * 180;
                GameObject instantiatedGO = Instantiate(prefab, base_position, Quaternion.identity, instantiatedGOContainer.transform);
                instantiatedGO.transform.localScale = scale;
                //instantiatedGO.transform.RotateAround(base_position + rotation_offset, Vector3.up, rotation);
                // Box collider on container
                MeshRenderer renderer = instantiatedGO.GetComponentInChildren<MeshRenderer>();
                boxCollider.center = renderer.bounds.center;
                boxCollider.size = renderer.bounds.size;
            }
            // Billboard
            GameObject instanciatedBillboard = Instantiate(billboard, base_position, Quaternion.identity, instantiatedGOContainer.transform);
            instanciatedBillboard.transform.localScale = scale;
            // Arc Links
            GameObject arcLinkContainer = new GameObject(MetaData.ARC_LINK_CONTAINER);
            arcLinkContainer.transform.SetParent(instantiatedGOContainer.transform);
            // TODO V0.1 Update this with list of actual links
            new ArcLink(Instantiate(arcLink, base_position, Quaternion.identity, arcLinkContainer.transform), base_position + rotation_offset, new Vector3(0,0,100));
            arcLinkContainer.GetComponentInChildren<LineRenderer>().enabled = false;
        }
        UserData.elements_loaded = true;
    }
}
