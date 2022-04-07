using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabEnhancer : MonoBehaviour
{
    public GameObject element;
    public bool unique = false;
    public float elementsPerSquareMeter = 1.0f;
    public float scaleRange = 1.25f;
    public float scaleOffset = 0.5f;
    public Vector2 xBounds = new Vector2(-0.45f, 0.45f);
    public Vector2 zBounds = new Vector2(-0.45f, 0.45f);
    public bool freeRotation = true;
    public bool smallVerticalNoise = true;
    public bool zRotation = false;

    private Vector3 parentScale, parentPosition;
    private Vector2 effectiveXBounds, effectiveZBounds;
    private float yCorrection;
    private double lastRandom;
    private int nb_elements;
    private GameObject parentGO;

    void Start()
    {
        GetParentData();
        RenderElements();
        /*if (nb_elements > 1)
            CombineMesh();*/
    }

    private void GetParentData()
    {
        parentScale = gameObject.transform.localScale;
        parentPosition = gameObject.transform.position;
        if (gameObject.transform.localEulerAngles.y % 180 == 0)
        {
            effectiveXBounds = xBounds * parentScale.x;
            effectiveZBounds = zBounds * parentScale.z;
        } else
        {
            effectiveXBounds = xBounds * parentScale.z;
            effectiveZBounds = zBounds * parentScale.x;
        }
        try {
            yCorrection = gameObject.transform.GetComponent<BoxCollider>().bounds.size.y * gameObject.transform.localScale.y;
        } catch(MissingComponentException) {
            yCorrection = gameObject.transform.parent.GetComponent<BoxCollider>().bounds.size.y;
        }
    }

    private void RenderElements()
    {
        if (unique)
            nb_elements = 1;
        else
            nb_elements = Mathf.RoundToInt((float) (0.9f + Random.Range(0f, 1f) / 5.0f) * elementsPerSquareMeter * parentScale.x * parentScale.z);
        float scale, x_pos, z_pos;
        Vector3 position;
        // Init first GO to be parent of all the other ones
        if (nb_elements > 0)
        {
            // Instantiate in prefab's local scale before stretching.
            scale = (float) Random.Range(0f, 1f) * scaleRange + scaleOffset;
            x_pos = (float) (parentPosition.x + (effectiveXBounds[0] + Random.Range(0f, 1f) * (effectiveXBounds[1] - effectiveXBounds[0])));
            z_pos = (float) (parentPosition.z + (effectiveZBounds[0] + Random.Range(0f, 1f) * (effectiveZBounds[1] - effectiveZBounds[0])));
            position = new Vector3(x_pos, parentPosition.y, z_pos);
            position.y = UserData.meta_data.terrain.SampleHeight(position) + yCorrection;
            parentGO = Instantiate(element, position, Quaternion.identity);
            parentGO.transform.localScale = new Vector3(scale, scale, scale);
            if (freeRotation)
                parentGO.transform.Rotate(0, (float) (Random.Range(0f, 1f) * 360), 0);
            else
                parentGO.transform.Rotate(0, Random.Range(-5.0f, 5.0f), 0);
            if (smallVerticalNoise)
                parentGO.transform.Rotate(Random.Range(-5.0f, 5.0f), 0, 0);
            if (zRotation)
                parentGO.transform.Rotate(0, 0, (float) (Random.Range(0f, 1f) * 360));
            parentGO.transform.parent = this.transform;
        }
        for (int i = 1; i < nb_elements; ++i)
        {
            // Instantiate in prefab's local scale before stretching.
            scale = (float) Random.Range(0f, 1f) * scaleRange + scaleOffset;
            x_pos = (float) (parentPosition.x + (effectiveXBounds[0] + Random.Range(0f, 1f) * (effectiveXBounds[1] - effectiveXBounds[0])));
            z_pos = (float) (parentPosition.z + (effectiveZBounds[0] + Random.Range(0f, 1f) * (effectiveZBounds[1] - effectiveZBounds[0])));
            position = new Vector3(x_pos, parentPosition.y, z_pos);
            position.y = UserData.meta_data.terrain.SampleHeight(position) + yCorrection;
            GameObject instantiatedElement = Instantiate(element, position, Quaternion.identity);
            instantiatedElement.transform.localScale = new Vector3(scale, scale, scale);
            if (freeRotation)
                instantiatedElement.transform.Rotate(0, (float) (Random.Range(0f, 1f) * 360), 0);
            else
                instantiatedElement.transform.Rotate(0, Random.Range(-5.0f, 5.0f), 0);
            if (smallVerticalNoise)
                instantiatedElement.transform.Rotate(Random.Range(-5.0f, 5.0f), 0, 0);
            if (zRotation)
                instantiatedElement.transform.Rotate(0, 0, (float) (Random.Range(0f, 1f) * 360));
            instantiatedElement.transform.parent = parentGO.transform;
        }
    }

    private void CombineMesh()
    {
        MeshFilter[] meshFilters = parentGO.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            Quaternion rotation = Quaternion.Euler(meshFilters[i].transform.localEulerAngles.x, meshFilters[i].transform.localEulerAngles.y, meshFilters[i].transform.localEulerAngles.z);
            combine[i].transform = Matrix4x4.TRS(meshFilters[i].transform.localPosition, rotation, meshFilters[i].transform.localScale);
            //combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        parentGO.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        parentGO.transform.GetComponent<MeshFilter>().mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        parentGO.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        parentGO.transform.gameObject.SetActive(true);
    }
}
