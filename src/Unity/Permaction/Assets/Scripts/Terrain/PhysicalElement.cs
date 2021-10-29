using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalElement
{
    public int id;
    public GameObject gameObject;
    public Vector3 position;

    public PhysicalElement(int id, GameObject gameObject, Vector3 position)
    {
        this.id = id;
        this.gameObject = gameObject;
        this.position = position;
    }
}
