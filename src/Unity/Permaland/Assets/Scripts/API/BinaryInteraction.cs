using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace API
{
    [System.Serializable]
    public class BinaryInteraction
    {
        public int id;
        public int element1_id;
        public int element2_id;
        public int interaction_level;
        public string description;
        
        public BinaryInteraction()
        {
            
        }

        public override string ToString()
        {
            return "id: " + id + ", element1_id: " + element1_id + ", element2_id: " + element2_id + ", interaction_level: " + interaction_level; 
        }
    }
}
