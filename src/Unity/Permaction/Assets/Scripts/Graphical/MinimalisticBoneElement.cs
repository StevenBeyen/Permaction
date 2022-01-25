using UnityEngine;

//complete list of unity inpector attributes https://docs.unity3d.com/ScriptReference/AddComponentMenu.html?_ga=2.45747431.2107391006.1601167752-1733939537.1520033247
//inspector attributes https://unity3d.college/2017/05/22/unity-attributes/
//Nvidia Flex video https://youtu.be/TNAKv1dkYyQ

public class MinimalisticBoneElement : MonoBehaviour
{
    /*
        Y0 - Y1 - Y2
    */
    [Header("Bones")]
    public GameObject Y0 = null;
    public GameObject Y1 = null;
    public GameObject Y2 = null;
    [Header("Spring Joint Settings")]
    [Tooltip("Strength of spring")]
    public float Spring = 1000f;
    [Tooltip("Higher the value the faster the spring oscillation stops")]
    public float Damper = 0.5f;
    [Header("Other Settings")]
    public Softbody.ColliderShape Shape = Softbody.ColliderShape.Box;
    public float ColliderSize = 0.025f;
    public float RigidbodyMass = 1e+11f; 
    //public LineRenderer PrefabLine = null;
    //public bool ViewLines = true;

    private void Start()
    {
        Softbody.Init(Shape, ColliderSize, RigidbodyMass, Spring, Damper, RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);

        Softbody.AddCollider(ref Y0);
        Softbody.AddCollider(ref Y1);
        Softbody.AddCollider(ref Y2);

        // Horizontal
        Softbody.AddSpring(ref Y0, ref Y1);
        Softbody.AddSpring(ref Y1, ref Y2);
    }
}
