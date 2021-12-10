using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphical {
    public class GraphicalTerrain : MonoBehaviour
    {
        private Vector3 mousePosition;

        void OnMouseDown()
        {
            mousePosition = Input.mousePosition;
        }

        void OnMouseUpAsButton()
        {
            // Little trick to avoid unselect on camera drag
            if (Input.mousePosition == mousePosition && UserData.selected_element != null)
                UserData.selected_element.unselect();
        }
    }
}
