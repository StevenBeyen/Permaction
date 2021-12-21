using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalElement
{
    public int id;
    public GameObject game_object;
    public Vector3 position;
    public Vector3 scale;
    public List<int> associated_ids;

    private Vector2 x_bounds, z_bounds;

    public PhysicalElement(int id, GameObject game_object, Vector3 position, Vector3 scale)
    {
        this.id = id;
        this.game_object = game_object;
        this.position = position;
        this.scale = scale;
        this.associated_ids = new List<int>();
        this.x_bounds = ComputeBounds(position.x, scale.x);
        this.z_bounds = ComputeBounds(position.z, scale.z);
        Debug.Log(x_bounds);
        Debug.Log(z_bounds);
    }

    private Vector2 ComputeBounds(float position, float scale)
    {
        float half_scale = scale / 2.0f;
        return new Vector2(position - half_scale, position + half_scale);
    }

    public Vector2 GetXBounds()
    {
        return x_bounds;
    }

    public Vector2 GetZBounds()
    {
        return z_bounds;
    }
}
