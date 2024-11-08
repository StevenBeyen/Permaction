﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using API;
using Graphical;

public class PlaceElements : MonoBehaviour
{
    public Terrain terrain;
    public GameObject graphicalTitle;
    public GameObject greenArcLink;
    public GameObject redArcLink;
    public GameObject linkDescription;

    private List<string> prefab_names;
    private int prefab_fixed_size_width, prefab_fixed_size_length, x_step, z_step;
    private GameObject prefab;
    private Vector3 base_position, real_position, rotation_offset, scale;
    private float rotation;
    private BoxCollider boxCollider;
    private float height_correction;

    // Start is called before the first frame update
    void Start()
    {
        // Cloning the terrain so that all runtime changes are cancelled on exit
        CloneTerrain();
        height_correction = MetaData.NON_FLATTENING_HEIGHT_MARGIN * terrain.terrainData.size.y;
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
        if (element.show_interactions)
        {
            // Title
            GameObject instantiatedGraphicalTitle = Instantiate(graphicalTitle, base_position + rotation_offset + new Vector3(0, container.GetComponent<BoxCollider>().size.y, 0), Quaternion.identity, container.transform);
            instantiatedGraphicalTitle.transform.GetComponentInChildren<TextMesh>().text = element.name;
            // Saving element for arc link creation
            UserData.physical_elements.Add(new PhysicalElement(element.id, container, base_position + rotation_offset, scale));
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
            rotation_offset = new Vector3(prefab_fixed_size_width/2.0f, 0, prefab_fixed_size_length/2.0f);
            x_step = prefab_fixed_size_width;
            z_step = prefab_fixed_size_length;
        }
        // Objects
        for (int x=0; (x+x_step)<=scale.x; x+=x_step) {
            for (int z=0; (z+z_step)<=scale.z; z+=z_step) {
                prefab = Resources.Load(prefab_names[Random.Range(0,prefab_names.Count)]) as GameObject;
                real_position = base_position + rotation_offset + new Vector3(x,0,z);
                if (!element.terrain_flattening)
                    real_position.y += height_correction;
                GameObject instantiatedGO = Instantiate(prefab, real_position, Quaternion.identity, container.transform);
                instantiatedGO.transform.RotateAround(real_position, Vector3.up, rotation + Random.Range(0,2) * 180);
                try {
                    instantiatedGO.transform.GetComponent<BoxCollider>().enabled = false;
                } catch (MissingComponentException) {}
            }
        }
        // Changing rotation offset for box collider and title
        if (element.show_interactions)
        {
            rotation_offset = new Vector3(scale.x/2.0f, 0, scale.z/2.0f);
            Vector3 center = base_position + rotation_offset;
            // Box collider on container
            float y_scale;
            try {
                y_scale = prefab.transform.GetComponent<BoxCollider>().bounds.size.y;
            } catch (MissingComponentException) {
                y_scale = (scale.x + scale.z) / 2.0f;
            }
            if (y_scale == 0) {
                y_scale = MetaData.EMPTY_COLLIDER_HEIGHT_CORRECTION;
                center.y -= (y_scale/2 - MetaData.BOX_COLLIDER_ADDITIONAL_CORRECTION);
            }
            boxCollider.center = center;
            boxCollider.size = new Vector3(scale.x, y_scale, scale.z);
        }
    }

    private void InstantiateStretchableElement(Element element, GameObject container)
    {
        // Object itself
        prefab = Resources.Load(prefab_names[Random.Range(0,prefab_names.Count)]) as GameObject;
        rotation_offset = new Vector3(scale.x/2.0f, 0, scale.z/2.0f);
        rotation = Random.Range(0,2) * 180;
        real_position = base_position + rotation_offset;
        if (!element.terrain_flattening)
            real_position.y += height_correction;
        GameObject instantiatedGO = Instantiate(prefab, real_position, Quaternion.identity, container.transform);
        instantiatedGO.transform.RotateAround(real_position, Vector3.up, rotation);
        // Adding some small random rotation to the container as well
        container.transform.RotateAround(real_position, Vector3.up, Random.Range(-3.0f, 3.0f));
        if (rotation%180 == 0)
            instantiatedGO.transform.localScale = scale;
        else
            instantiatedGO.transform.localScale = new Vector3(scale.z, scale.y, scale.x);
        // Box collider on container
        if (element.show_interactions)
        {
            MeshRenderer renderer = instantiatedGO.GetComponent<MeshRenderer>();
            int childCount, index;
            childCount = instantiatedGO.transform.childCount;
            index = 0;
            while (renderer == null && index < childCount)
            {
                renderer = instantiatedGO.transform.GetChild(index).gameObject.GetComponent<MeshRenderer>();
                index += 1;
            }
            if (renderer != null)
            {
                boxCollider.center = renderer.bounds.center;
                boxCollider.size = renderer.bounds.size;
            } else // Skinned mesh renderer for deforming objects, with box collider a little higher than object itself to avoid collisions
            {
                SkinnedMeshRenderer skinnedRenderer = instantiatedGO.GetComponentInChildren<SkinnedMeshRenderer>();
                boxCollider.center = new Vector3(skinnedRenderer.bounds.center.x, skinnedRenderer.bounds.center.y + MetaData.EPSILON, skinnedRenderer.bounds.center.z);
                boxCollider.size = skinnedRenderer.bounds.size;
            }
            
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
                            CreateArcLink(firstElement, secondElement, arcLink, binaryInteraction.description);
                            CreateArcLink(secondElement, firstElement, arcLink, binaryInteraction.description);
                        }
                    }
                }
            }
        }
    }

    // TODO Add description associated with arc link
    private void CreateArcLink(PhysicalElement firstElement, PhysicalElement secondElement, GameObject arcLink, string description)
    {
        if (!firstElement.associated_ids.Contains(secondElement.id))
        {
            // Container
            GameObject arcLinkContainer = new GameObject(MetaData.ARC_LINK_CONTAINER);
            arcLinkContainer.transform.SetParent(firstElement.game_object.transform);
            arcLinkContainer.SetActive(false);
            // ArcLink
            ArcLink arc = new ArcLink(Instantiate(arcLink, firstElement.position, Quaternion.identity, arcLinkContainer.transform), firstElement.position, secondElement.position);
            firstElement.associated_ids.Add(secondElement.id);
            // Description
            GameObject instantiatedLinkDescription = Instantiate(linkDescription, arc.getTopCoordinates(), Quaternion.identity, arcLinkContainer.transform);
            // TODO Handle case with multiple descriptions between two elements. Parsing upwards in code maybe?
            // Break on multiple lines when over X characters:
            int nextSpaceIndex;
            char[] descriptionChar = description.ToCharArray();
            for (int i  = 0; i < description.Length / MetaData.LINK_DESCRIPTION_LINE_WRAP; ++i)
            {
                nextSpaceIndex = description.IndexOf(' ', MetaData.LINK_DESCRIPTION_LINE_WRAP * (i + 1));
                if (nextSpaceIndex != -1)
                    descriptionChar[nextSpaceIndex] = '\n';
            }
            description = new string(descriptionChar);
            instantiatedLinkDescription.GetComponentInChildren<TextMesh>().text = description;
        }
    }
}
