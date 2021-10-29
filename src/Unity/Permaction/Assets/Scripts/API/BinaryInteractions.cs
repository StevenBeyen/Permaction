using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace API
{
    [System.Serializable]
    public class BinaryInteractions : API, IEnumerable
    {
        private const string BINARY_INTERACTIONS_URI = API_URI + "/binary_interactions";

        public BinaryInteraction[] binary_interactions;

        public BinaryInteractions()
        {

        }

        public void BinaryInteractionsCallback(UnityWebRequest webRequest)
        {
            this.binary_interactions = JsonUtility.FromJson<BinaryInteractions>(webRequest.downloadHandler.text).binary_interactions;
        }

        public string GetBinaryInteractionsURI()
        {
            return BINARY_INTERACTIONS_URI;
        }

        public IEnumerator GetEnumerator()
        {
            return binary_interactions.GetEnumerator();
        }

        public override string ToString()
        {
            string reply = "[";
            foreach (BinaryInteraction binary_interaction in binary_interactions)
                reply += binary_interaction.ToString() + " ; \n";
            reply += "]";
            return reply;
        }
    }
}
