using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalElement
{
    public int id;
    public GameObject game_object;
    public Vector3 position;
    public List<int> associated_ids;

    public PhysicalElement(int id, GameObject game_object, Vector3 position)
    {
        this.id = id;
        this.game_object = game_object;
        this.position = position;
        this.associated_ids = new List<int>();
    }
}
