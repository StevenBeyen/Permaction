using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphical
{
    public class GraphicalElement : MonoBehaviour
    {
        private void OnMouseUpAsButton()
        {
            if (UserData.selected_element != null && UserData.selected_element != this)
                UserData.selected_element.unselect();
            if (UserData.selected_element != this)
                select();
        }

        public void select()
        {
            transform.Find(MetaData.GRAPHICAL_TITLE).gameObject.SetActive(true);
            foreach (Transform t in transform)
            {
                if (t.name == MetaData.ARC_LINK_CONTAINER)
                    t.gameObject.SetActive(true);
            }
            UserData.selected_element = this;
        }

        public void unselect()
        {
            UserData.selected_element = null;
            foreach (Transform t in transform)
            {
                if (t.name == MetaData.ARC_LINK_CONTAINER)
                    t.gameObject.SetActive(false);
            }
            transform.Find(MetaData.GRAPHICAL_TITLE).gameObject.SetActive(false);
        }
    }
}
