using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphical
{
    public class GraphicalElement : MonoBehaviour
    {
        //Shader initialShader;

        private void OnMouseUpAsButton()
        {
            if (UserData.selectedElement != null && UserData.selectedElement != this)
                UserData.selectedElement.unselect();
            if (UserData.selectedElement != this)
                select();
        }

        public void select()
        {
            //getChildGameObject(gameObject, MetaData.ARC_LINK_CONTAINER).SetActive(true);
            transform.parent.parent.GetComponentInChildren<SpriteRenderer>().enabled = true;
            foreach (LineRenderer lr in transform.parent.parent.GetComponentsInChildren<LineRenderer>())
            {
                lr.enabled = true;
            }
            UserData.selectedElement = this;
        }

        public void unselect()
        {
            UserData.selectedElement = null;
            foreach (LineRenderer lr in transform.parent.parent.GetComponentsInChildren<LineRenderer>())
            {
                lr.enabled = false;
            }
            transform.parent.parent.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }

        static public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
            Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
            return null;
        }
    }
}
