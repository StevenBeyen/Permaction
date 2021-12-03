using UnityEngine;

//complete list of unity inpector attributes https://docs.unity3d.com/ScriptReference/AddComponentMenu.html?_ga=2.45747431.2107391006.1601167752-1733939537.1520033247
//inspector attributes https://unity3d.college/2017/05/22/unity-attributes/
//Nvidia Flex video https://youtu.be/TNAKv1dkYyQ

public class BoneRoadPath : MonoBehaviour
{
    /*
        X0 --- X1 --- X2 --- X3 --- X4
        | \  / | \  / | \  / | \  / |
        |      |      R      |      |
        | /  \ | /  \ | /  \ | /  \ |
        Y0 --- Y1 --- Y2 --- Y3 --- Y4
    */
    [Header("Bones")]
    public GameObject Root = null;
    public GameObject X0 = null;
    public GameObject X1 = null;
    public GameObject X2 = null;
    public GameObject X3 = null;
    public GameObject X4 = null;
    public GameObject Y0 = null;
    public GameObject Y1 = null;
    public GameObject Y2 = null;
    public GameObject Y3 = null;
    public GameObject Y4 = null;
    [Header("Spring Joint Settings")]
    [Tooltip("Strength of spring")]
    public float Spring = 100f;
    [Tooltip("Higher the value the faster the spring oscillation stops")]
    public float Damper = 0.2f;
    [Header("Other Settings")]
    public Softbody.ColliderShape Shape = Softbody.ColliderShape.Box;
    public float ColliderSize = 0.002f;
    public float RigidbodyMass = 1f; 
    //public LineRenderer PrefabLine = null;
    //public bool ViewLines = true;

    private void Start()
    {
        Softbody.Init(Shape, ColliderSize, RigidbodyMass, Spring, Damper, RigidbodyConstraints.None);

        Softbody.AddCollider(ref Root);
        Softbody.AddCollider(ref X0);
        Softbody.AddCollider(ref X1);
        Softbody.AddCollider(ref X2);
        Softbody.AddCollider(ref X3);
        Softbody.AddCollider(ref X4);
        Softbody.AddCollider(ref Y0);
        Softbody.AddCollider(ref Y1);
        Softbody.AddCollider(ref Y2);
        Softbody.AddCollider(ref Y3);
        Softbody.AddCollider(ref Y4);

        // Root
        Softbody.AddSpring(ref X0, ref Root);
        Softbody.AddSpring(ref X1, ref Root);
        Softbody.AddSpring(ref X2, ref Root);
        Softbody.AddSpring(ref X3, ref Root);
        Softbody.AddSpring(ref X4, ref Root);
        Softbody.AddSpring(ref Y0, ref Root);
        Softbody.AddSpring(ref Y1, ref Root);
        Softbody.AddSpring(ref Y2, ref Root);
        Softbody.AddSpring(ref Y3, ref Root);
        Softbody.AddSpring(ref Y4, ref Root);

        // Horizontal
        Softbody.AddSpring(ref X0, ref X1);
        Softbody.AddSpring(ref X1, ref X2);
        Softbody.AddSpring(ref X2, ref X3);
        Softbody.AddSpring(ref X3, ref X4);
        Softbody.AddSpring(ref Y0, ref Y1);
        Softbody.AddSpring(ref Y1, ref Y2);
        Softbody.AddSpring(ref Y2, ref Y3);
        Softbody.AddSpring(ref Y3, ref Y4);

        // Vertical
        Softbody.AddSpring(ref X0, ref Y0);
        Softbody.AddSpring(ref X1, ref Y1);
        Softbody.AddSpring(ref X2, ref Y2);
        Softbody.AddSpring(ref X3, ref Y3);
        Softbody.AddSpring(ref X4, ref Y4);

        // Diagonal
        Softbody.AddSpring(ref X0, ref Y1);
        Softbody.AddSpring(ref Y0, ref X1);
        Softbody.AddSpring(ref X1, ref Y2);
        Softbody.AddSpring(ref Y1, ref X2);
        Softbody.AddSpring(ref X2, ref Y3);
        Softbody.AddSpring(ref Y2, ref X3);
        Softbody.AddSpring(ref X3, ref Y4);
        Softbody.AddSpring(ref Y3, ref X4);
    }
}
