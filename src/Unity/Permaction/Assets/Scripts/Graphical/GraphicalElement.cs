using System;
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
            foreach (LineRenderer lr in transform.GetComponentsInChildren<LineRenderer>())
            {
                lr.enabled = true;
            }
            try {
                transform.Find(MetaData.ARC_LINK_CONTAINER).Find(MetaData.LINK_DESCRIPTION).gameObject.SetActive(true);
            } catch (NullReferenceException) {}
            UserData.selected_element = this;
        }

        public void unselect()
        {
            UserData.selected_element = null;
            try {
                transform.Find(MetaData.ARC_LINK_CONTAINER).Find(MetaData.LINK_DESCRIPTION).gameObject.SetActive(false);
            } catch (NullReferenceException) {}
            foreach (LineRenderer lr in transform.GetComponentsInChildren<LineRenderer>())
            {
                lr.enabled = false;
            }
            transform.Find(MetaData.GRAPHICAL_TITLE).gameObject.SetActive(false);
        }
    }
}
