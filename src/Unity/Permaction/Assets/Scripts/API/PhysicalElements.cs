using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace API
{
    [System.Serializable]
    public class PhysicalElements : API
    {
        public Element[] physical_elements;

        public PhysicalElements()
        {

        }

        public void PhysicalElementsCallback(UnityWebRequest webRequest)
        {
            this.physical_elements = JsonUtility.FromJson<PhysicalElements>(webRequest.downloadHandler.text).physical_elements;
            this.Sort();
        }

        public void Sort()
        {
            System.Array.Sort(physical_elements);
        }

        public override string ToString()
        {
            string reply = "[";
            foreach (Element element in physical_elements)
                reply += element.ToString() + " ; \n";
            reply += "]";
            return reply;
        }
    }
}
