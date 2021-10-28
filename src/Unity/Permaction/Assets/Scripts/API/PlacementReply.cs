using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace API
{
    [System.Serializable]
    public class PlacementReply
    {
        public float fitness;
        public bool optimum;
        public Element[] result;

        public PlacementReply(float fitness, bool optimum, Element[] result)
        {
            this.fitness = fitness;
            this.optimum = optimum;
            this.result = result;
        }
    }
}