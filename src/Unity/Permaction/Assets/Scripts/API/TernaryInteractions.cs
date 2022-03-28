using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace API
{
    [System.Serializable]
    public class TernaryInteractions : API, IEnumerable
    {
        public TernaryInteraction[] ternary_interactions;

        public TernaryInteractions()
        {

        }

        public void TernaryInteractionsCallback(UnityWebRequest webRequest)
        {
            this.ternary_interactions = JsonUtility.FromJson<TernaryInteractions>(webRequest.downloadHandler.text).ternary_interactions;
        }

        public IEnumerator GetEnumerator()
        {
            return ternary_interactions.GetEnumerator();
        }

        public override string ToString()
        {
            string reply = "[";
            foreach (TernaryInteraction ternary_interaction in ternary_interactions)
                reply += ternary_interaction.ToString() + " ; \n";
            reply += "]";
            return reply;
        }
    }
}
