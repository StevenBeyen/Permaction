using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace API
{
    [System.Serializable]
    public class PlacementRequest : API
    {
        public TerrainLine[] terrain_data;
        public Element[] elements_data;
        private PlacementReply reply;

        public PlacementRequest(string[][] terrain_data, Element[] elements_data)
        {
            this.terrain_data = new TerrainLine[terrain_data.Length];
            for (int i=0; i<terrain_data.Length;++i)
            {
                this.terrain_data[i] = new TerrainLine(i, terrain_data[i]);
            }
            this.elements_data = elements_data;
        }

        public void APIRendererCallback(UnityWebRequest webRequest)
        {
            // TODO Add management of error messages
            this.reply = JsonUtility.FromJson<PlacementReply>(webRequest.downloadHandler.text);
            Debug.Log(webRequest.downloadHandler.text);
        }

        public PlacementReply GetReply()
        {
            return reply;
        }
    }
}