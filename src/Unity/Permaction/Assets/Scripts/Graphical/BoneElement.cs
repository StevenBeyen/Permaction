using UnityEngine;

//complete list of unity inpector attributes https://docs.unity3d.com/ScriptReference/AddComponentMenu.html?_ga=2.45747431.2107391006.1601167752-1733939537.1520033247
//inspector attributes https://unity3d.college/2017/05/22/unity-attributes/
//Nvidia Flex video https://youtu.be/TNAKv1dkYyQ

public class BoneElement : MonoBehaviour
{
    /*
        00 - 01 - 02
        |  \ | /  |
        10   11   12
        |  / | \  |
        20 - 21 - 22
    */
    [Header("Bones")]
    public GameObject XY00 = null;
    public GameObject XY01 = null;
    public GameObject XY02 = null;
    public GameObject XY10 = null;
    public GameObject XY11 = null;
    public GameObject XY12 = null;
    public GameObject XY20 = null;
    public GameObject XY21 = null;
    public GameObject XY22 = null;
    [Header("Spring Joint Settings")]
    [Tooltip("Strength of spring")]
    public float Spring = 1000f;
    [Tooltip("Higher the value the faster the spring oscillation stops")]
    public float Damper = 0.5f;
    [Header("Other Settings")]
    public Softbody.ColliderShape Shape = Softbody.ColliderShape.Box;
    public float ColliderSize = 0.004f;
    public float RigidbodyMass = 1e+11f; 
    //public LineRenderer PrefabLine = null;
    //public bool ViewLines = true;

    private void Start()
    {
        Softbody.Init(Shape, ColliderSize, RigidbodyMass, Spring, Damper, RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);

        Softbody.AddCollider(ref XY00);
        Softbody.AddCollider(ref XY01);
        Softbody.AddCollider(ref XY02);
        Softbody.AddCollider(ref XY10);
        Softbody.AddCollider(ref XY11);
        Softbody.AddCollider(ref XY12);
        Softbody.AddCollider(ref XY20);
        Softbody.AddCollider(ref XY21);
        Softbody.AddCollider(ref XY22);

        // Horizontal
        Softbody.AddSpring(ref XY00, ref XY01);
        Softbody.AddSpring(ref XY01, ref XY02);
        Softbody.AddSpring(ref XY10, ref XY11);
        Softbody.AddSpring(ref XY11, ref XY12);
        Softbody.AddSpring(ref XY20, ref XY21);
        Softbody.AddSpring(ref XY21, ref XY22);

        // Vertical
        Softbody.AddSpring(ref XY00, ref XY10);
        Softbody.AddSpring(ref XY10, ref XY20);
        Softbody.AddSpring(ref XY01, ref XY11);
        Softbody.AddSpring(ref XY11, ref XY21);
        Softbody.AddSpring(ref XY02, ref XY12);
        Softbody.AddSpring(ref XY12, ref XY22);

        // Diagonal
        Softbody.AddSpring(ref XY00, ref XY11);
        Softbody.AddSpring(ref XY02, ref XY11);
        Softbody.AddSpring(ref XY20, ref XY11);
        Softbody.AddSpring(ref XY22, ref XY11);
    }
}
