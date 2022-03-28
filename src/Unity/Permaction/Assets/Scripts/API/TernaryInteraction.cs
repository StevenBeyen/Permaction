using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace API
{
    [System.Serializable]
    public class TernaryInteraction
    {
        public int id;
        public int id_locale;
        public int element1_id;
        public int interaction_type_id;
        public int element2_id;
        public string description;
        
        public TernaryInteraction()
        {
            
        }

        public override string ToString()
        {
            return "id: " + id + ", element1_id: " + element1_id + ", element2_id: " + element2_id + ", description: " + description; 
        }
    }
}
