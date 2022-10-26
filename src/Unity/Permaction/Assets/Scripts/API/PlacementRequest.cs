using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace API
{
    [System.Serializable]
    public class PlacementRequest : API
    {
        public int id_locale;
        public TerrainLine[] terrain_data;
        public Element[] elements_data;
        private PlacementReply reply;

        public PlacementRequest(int id_locale, string[][] terrain_data, Element[] elements_data)
        {
            this.id_locale = id_locale;
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
            Debug.Log(webRequest.downloadHandler.text);
            this.reply = JsonUtility.FromJson<PlacementReply>(webRequest.downloadHandler.text);
        }

        public PlacementReply GetReply()
        {
            return reply;
        }
    }
}