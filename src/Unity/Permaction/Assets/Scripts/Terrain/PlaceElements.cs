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

    private List<string> prefab_names;
    private int prefab_fixed_size_width, prefab_fixed_size_length, x_step, z_step;
    private GameObject prefab;
    private Vector3 base_position, real_position, rotation_offset, scale;
    private float rotation;
    private BoxCollider boxCollider;

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
            LoadElements();
    }

    void CloneTerrain()
    {
        terrain.terrainData = TerrainDataCloner.Clone(terrain.terrainData);
        terrain.GetComponent<TerrainCollider>().terrainData = terrain.terrainData; // Don't forget to update the TerrainCollider as well
    }

    void LoadElements()
    {
        foreach(Element element in UserData.reply.result)
        {
            InstantiateElement(element);
        }
        CreateArcLinks();
        UserData.elements_loaded = true;
    }

    private void InstantiateElement(Element element)
    {
        // First of all, let's extract the information we stored and sync position data for placement
        element.Enhance();
        element.Sync();
        // Let's create a container for this element.
        GameObject container = new GameObject(element.name);
        container.transform.SetParent(terrain.transform);
        container.AddComponent<Graphical.GraphicalElement>();
        if (element.show_interactions) // Only adding box collider if the element needs to be selectable.
            boxCollider = container.AddComponent<BoxCollider>();
        scale = element.GetScale();
        UserData.meta_data.prefab_mapping.TryGetValue(element.id, out prefab_names);
        base_position = element.GetPosition();
        // Converting base position height (0,1 value) in terrain height
        base_position = new Vector3(base_position.x, base_position.y * terrain.terrainData.size.y, base_position.z);
        // Flattening terrain on the area where the element will be placed
        if (element.terrain_flattening)
            terrain.terrainData.SetHeights((int) base_position.x, (int) base_position.z, element.GetHeights());
        // Switch case depending on type of prefab object: fixed size or stretchable
        if (UserData.meta_data.prefab_fixed_size_widths.TryGetValue(element.id, out prefab_fixed_size_width))
        {
            InstantiateFixedSizeElement(element, container);
        } else
        {
            InstantiateStretchableElement(element, container);
        }
        // Billboard
        if (element.show_interactions)
        {
            GameObject instantiatedBillboard = Instantiate(billboard, base_position + rotation_offset, Quaternion.identity, container.transform);
            // Saving elements for arc link creation
            //UserData.physical_elements.Add(new PhysicalElement(e.id, container, base_position + rotation_offset));
        }
    }

    private void InstantiateFixedSizeElement(Element element, GameObject container)
    {
        // Getting element length, or default if it has none
        if (!UserData.meta_data.prefab_fixed_size_lengths.TryGetValue(element.id, out prefab_fixed_size_length))
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
                GameObject instantiatedGO = Instantiate(prefab, real_position, Quaternion.identity, container.transform);
                instantiatedGO.transform.RotateAround(real_position, Vector3.up, rotation + Random.Range(0,2) * 180);
            }
        }
        // Changing rotation offset for box collider and billboard
        if (element.show_interactions)
        {
            rotation_offset = new Vector3(scale.x/2.0f, 0, scale.z/2.0f);
            Vector3 center = base_position + rotation_offset;
            // Box collider on container
            boxCollider.center = center;
            boxCollider.size = new Vector3(scale.x, 1, scale.z);
        }
    }

    private void InstantiateStretchableElement(Element element, GameObject container)
    {
        // Object itself
        prefab = Resources.Load(prefab_names[Random.Range(0,prefab_names.Count)]) as GameObject;
        rotation_offset = new Vector3(scale.x/2.0f, 0, scale.z/2.0f);
        rotation = Random.Range(0,4) * 90;
        GameObject instantiatedGO = Instantiate(prefab, base_position + rotation_offset, Quaternion.identity, container.transform);
        instantiatedGO.transform.RotateAround(base_position + rotation_offset, Vector3.up, rotation);
        // Adding some small random rotation to the container as well
        container.transform.RotateAround(base_position + rotation_offset, Vector3.up, Random.Range(-5.0f, 5.0f));
        if (rotation%180 == 0)
            instantiatedGO.transform.localScale = scale;
        else
            instantiatedGO.transform.localScale = new Vector3(scale.z, scale.y, scale.x);
        // Box collider on container
        if (element.show_interactions)
        {
            MeshRenderer renderer = instantiatedGO.GetComponentInChildren<MeshRenderer>();
            boxCollider.center = renderer.bounds.center;
            boxCollider.size = renderer.bounds.size;
        }
    }

    private void CreateArcLinks()
    {
        GameObject arcLink;
        foreach(BinaryInteraction binaryInteraction in UserData.binary_interactions)
        {
            foreach(PhysicalElement firstElement in UserData.physical_elements)
            {
                if (binaryInteraction.element1_id == firstElement.id)
                {
                    foreach(PhysicalElement secondElement in UserData.physical_elements)
                    {
                        if (binaryInteraction.element2_id == secondElement.id)
                        {
                            // Positive and neutral interactions should be green (neutral interactions just don't have an impact on element placement).
                            if (binaryInteraction.interaction_level >= 0)
                            {
                                arcLink = greenArcLink;
                            } else // (binaryInteraction.interaction_level < 0)
                            {
                                arcLink = redArcLink;
                            }
                            // Create arc link in two directions since each arc is linked to one element only (they can only have one parent!).
                            CreateArcLink(firstElement, secondElement, arcLink);
                            CreateArcLink(secondElement, firstElement, arcLink);
                        }
                    }
                }
            }
        }
    }

    // TODO Add description associated with arc link
    private void CreateArcLink(PhysicalElement firstElement, PhysicalElement secondElement, GameObject arcLink)
    {
        if (!firstElement.associated_ids.Contains(secondElement.id))
        {
            GameObject arcLinkContainer = new GameObject(MetaData.ARC_LINK_CONTAINER);
            arcLinkContainer.transform.SetParent(firstElement.game_object.transform);
            new ArcLink(Instantiate(arcLink, firstElement.position, Quaternion.identity, arcLinkContainer.transform), firstElement.position, secondElement.position);
            arcLinkContainer.GetComponentInChildren<LineRenderer>().enabled = false;
            firstElement.associated_ids.Add(secondElement.id);
        }
    }
}
