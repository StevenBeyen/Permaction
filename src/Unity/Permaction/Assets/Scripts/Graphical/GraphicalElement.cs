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
            transform.GetComponentInChildren<SpriteRenderer>().enabled = true;
            foreach (LineRenderer lr in transform.GetComponentsInChildren<LineRenderer>())
            {
                lr.enabled = true;
            }
            UserData.selected_element = this;
        }

        public void unselect()
        {
            UserData.selected_element = null;
            foreach (LineRenderer lr in transform.GetComponentsInChildren<LineRenderer>())
            {
                lr.enabled = false;
            }
            transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }
}
