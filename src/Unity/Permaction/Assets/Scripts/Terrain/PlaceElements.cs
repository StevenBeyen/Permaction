using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using API;
using Graphical;

public class PlaceElements : MonoBehaviour
{
    public Terrain terrain;
    public GameObject billboard;
    public GameObject greenArcLink;
    public GameObject redArcLink;

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
        terrain.terrainData = TerrainDataCloner.Clone(terrain.terrainData);
        terrain.GetComponent<TerrainCollider>().terrainData = terrain.terrainData; // Don't forget to update the TerrainCollider as well
    }

    void LoadElements()
    {
        List<string> prefab_names;
        int prefab_fixed_size_width, prefab_fixed_size_length, x_step, z_step;
        GameObject prefab;
        Vector3 base_position, real_position, rotation_offset, scale;
        Vector3 billboard_scale = new Vector3(7, 7, 7);
        float rotation;
        foreach(Element e in UserData.reply.result)
        {
            GameObject instantiatedGOContainer = new GameObject(e.id.ToString());
            instantiatedGOContainer.transform.SetParent(terrain.transform);
            instantiatedGOContainer.AddComponent<Graphical.GraphicalElement>();
            BoxCollider boxCollider = instantiatedGOContainer.AddComponent<BoxCollider>();
            e.Sync();
            scale = e.GetScale();
            UserData.meta_data.prefab_mapping.TryGetValue(e.id, out prefab_names);
            base_position = e.GetPosition();
            // Postprocessing: converting base position height (0,1 value) in terrain height
            base_position = new Vector3(base_position.x, base_position.y * terrain.terrainData.size.y, base_position.z);
            // Flattening terrain on the area where the element will be placed
            terrain.terrainData.SetHeights((int) base_position.x - 1, (int) base_position.z - 1, e.GetHeights());
            // Switch case depending on type of prefab object: stretched or copied
            // Case 1: element that has to be copied to cover the given terrain coordinates
            if (UserData.meta_data.prefab_fixed_size_widths.TryGetValue(e.id, out prefab_fixed_size_width))
            {
                // Getting element length, or default if it has none
                if (!UserData.meta_data.prefab_fixed_size_lengths.TryGetValue(e.id, out prefab_fixed_size_length))
                    prefab_fixed_size_length = MetaData.PREFAB_FIXED_SIZE_DEFAULT_LENGTH;
                // Rotation, rotation offset & steps
                if (prefab_fixed_size_width == scale.z) // Horizontal (x ranged) element
                {
                    rotation = 0;
                    rotation_offset = new Vector3(prefab_fixed_size_length/2.0f, 0, prefab_fixed_size_width/2.0f);
                    x_step = prefab_fixed_size_length;
                    z_step = prefab_fixed_size_width;
                } else if (prefab_fixed_size_width == scale.x) // Vertical (z ranged) element
                {
                    rotation = 90;
                    rotation_offset = new Vector3(prefab_fixed_size_width/2.0f, 0, prefab_fixed_size_length/2.0f);
                    x_step = prefab_fixed_size_width;
                    z_step = prefab_fixed_size_length;
                } else // Non linear element
                {
                    rotation = 0;
                    rotation_offset = new Vector3(prefab_fixed_size_width/2.0f, 0, 0);
                    x_step = prefab_fixed_size_width;
                    z_step = prefab_fixed_size_length;
                }
                // Objects
                for (int x=0; (x+x_step)<=scale.x; x+=x_step) {
                    for (int z=0; (z+z_step)<=scale.z; z+=z_step) {
                        prefab = Resources.Load(prefab_names[Random.Range(0,prefab_names.Count)]) as GameObject;
                        real_position = base_position + rotation_offset + new Vector3(x,0,z);
                        GameObject instantiatedGO = Instantiate(prefab, real_position, Quaternion.identity, instantiatedGOContainer.transform);
                        instantiatedGO.transform.RotateAround(real_position, Vector3.up, rotation);
                    }
                }
                // Changing rotation offset for box collider and billboard
                rotation_offset = new Vector3(scale.x/2.0f, 0, scale.z/2.0f);
                Vector3 center = base_position + rotation_offset;
                // Box collider on container
                boxCollider.center = center;
                boxCollider.size = new Vector3(scale.x, 1, scale.z);
            } else // Case 2: stretchable terrain object
            {
                // Object itself
                prefab = Resources.Load(prefab_names[Random.Range(0,prefab_names.Count)]) as GameObject;
                rotation_offset = new Vector3(scale.x/2.0f, 0, scale.z/2.0f);
                rotation = Random.Range(0,4) * 90;
                GameObject instantiatedGO = Instantiate(prefab, base_position + rotation_offset, Quaternion.identity, instantiatedGOContainer.transform);
                instantiatedGO.transform.RotateAround(base_position + rotation_offset, Vector3.up, rotation + Random.Range(-10, 10));
                if(rotation%180 == 0)
                    instantiatedGO.transform.localScale = scale;
                else
                    instantiatedGO.transform.localScale = new Vector3(scale.z, scale.y, scale.x);
                // Box collider on container
                MeshRenderer renderer = instantiatedGO.GetComponentInChildren<MeshRenderer>();
                boxCollider.center = renderer.bounds.center;
                boxCollider.size = renderer.bounds.size;
            }
            // Billboard
            GameObject instantiatedBillboard = Instantiate(billboard, base_position + rotation_offset, Quaternion.identity, instantiatedGOContainer.transform);
            instantiatedBillboard.transform.localScale = billboard_scale;
            // Saving elements for arc link creation
            UserData.physicalElements.Add(new PhysicalElement(e.id, instantiatedGOContainer, base_position + rotation_offset));
        }
        // And now creating the arc links
        GameObject arcLink;
        foreach(BinaryInteraction binaryInteraction in UserData.binaryInteractions)
        {
            foreach(PhysicalElement firstElement in UserData.physicalElements)
            {
                if (binaryInteraction.element1_id == firstElement.id)
                {
                    foreach(PhysicalElement secondElement in UserData.physicalElements)
                    {
                        if (binaryInteraction.element2_id == secondElement.id)
                        {
                            if (binaryInteraction.interaction_level > 0)
                            {
                                arcLink = greenArcLink;
                            } else if (binaryInteraction.interaction_level < 0)
                            {
                                arcLink = redArcLink;
                            } else // Neutral interaction... should not happen because should not exist in the database!
                            {
                                Debug.Log("[Place Elements] Neutral interaction??");
                                arcLink = null;
                            }
                            // Create arc link in two directions since each arc is linked to one element only (they can only have one parent!).
                            createArcLink(firstElement, secondElement, arcLink);
                            createArcLink(secondElement, firstElement, arcLink);
                        }
                    }
                }
            }
        }
        UserData.elements_loaded = true;
    }

    // TODO Add description associated with arc link
    void createArcLink(PhysicalElement firstElement, PhysicalElement secondElement, GameObject arcLink)
    {
        if (!firstElement.associated_ids.Contains(secondElement.id))
        {
            GameObject arcLinkContainer = new GameObject(MetaData.ARC_LINK_CONTAINER);
            arcLinkContainer.transform.SetParent(firstElement.gameObject.transform);
            new ArcLink(Instantiate(arcLink, firstElement.position, Quaternion.identity, arcLinkContainer.transform), firstElement.position, secondElement.position);
            arcLinkContainer.GetComponentInChildren<LineRenderer>().enabled = false;
            firstElement.associated_ids.Add(secondElement.id);
        }
    }
}
