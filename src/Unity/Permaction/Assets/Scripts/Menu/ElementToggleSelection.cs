using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class ElementToggleSelection : MonoBehaviour
    {
        public Toggle toggle;
        
        private List<Image> checkmarkImages = new List<Image>();
        private string[] checkmarkImageNames = new string[] {"Checkmark"};
        //private string[] checkmarkImageNames = new string[] {"CheckmarkUpperLeft", "CheckmarkUpperRight", "CheckmarkLowerLeft", "CheckmarkLowerRight"};

        // Start is called before the first frame update
        void Start()
        {
            toggle = GetComponent<Toggle>();
            // First we get the Images that need to be (un)selected on toggle.
            foreach (string checkmarkImageName in checkmarkImageNames)
            {
                checkmarkImages.Add(GetChildComponentByName<Image>(checkmarkImageName));
            }
            // Now we can add the listener
            toggle.onValueChanged.AddListener(
                (value) => toggleSelection(toggle.isOn)
            );
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private T GetChildComponentByName<T>(string name) where T : Component
        {
            foreach (T component in toggle.GetComponentsInChildren<T>(true))
            {
                if (component.gameObject.name == name)
                {
                    return component;
                }
            }
            return null;
        }

        private void toggleSelection(bool value)
        {
            foreach (Image checkmarkImage in checkmarkImages)
            {
                checkmarkImage.gameObject.SetActive(value);
            }
        }
    }
}
