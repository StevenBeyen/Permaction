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

    private System.Random random;
    private Vector3 parentScale, parentPosition;
    private Vector2 effectiveXBounds, effectiveZBounds;
    private float yCorrection;

    void Start()
    {
        random = new System.Random();
        GetParentData();
        RenderElements();
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
        yCorrection = gameObject.transform.parent.GetComponent<BoxCollider>().bounds.size.y;
    }

    private void RenderElements()
    {
        int nb_elements;
        if (unique)
            nb_elements = 1;
        else
            nb_elements = Mathf.RoundToInt((float) (0.9f + random.NextDouble() / 5.0f) * elementsPerSquareMeter * parentScale.x * parentScale.z);
        float scale, x_pos, z_pos;
        Vector3 position;
        for (int i = 0; i < nb_elements; ++i)
        {
            // Instantiate in prefab's local scale before stretching.
            scale = (float) random.NextDouble() * scaleRange + scaleOffset;
            x_pos = (float) (parentPosition.x + (effectiveXBounds[0] + random.NextDouble() * (effectiveXBounds[1] - effectiveXBounds[0])));
            z_pos = (float) (parentPosition.z + (effectiveZBounds[0] + random.NextDouble() * (effectiveZBounds[1] - effectiveZBounds[0])));
            position = new Vector3(x_pos, parentPosition.y, z_pos);
            position.y = MetaData.terrain.SampleHeight(position) + yCorrection;
            GameObject instantiatedElement = Instantiate(element, position, Quaternion.identity);
            instantiatedElement.transform.localScale = new Vector3(scale, scale, scale);
            instantiatedElement.transform.Rotate(0, (float) (random.NextDouble() * 360), 0);
        }
    }
}
