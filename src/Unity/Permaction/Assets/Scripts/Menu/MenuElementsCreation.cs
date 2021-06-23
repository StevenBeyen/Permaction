using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using API;

namespace Menu {
    public class MenuElementsCreation : MonoBehaviour
    {
        public GameObject background;

        public GameObject categoriesGrid;
        public GameObject categoriesTitle;
        public GameObject renderButton;

        public GameObject categoryButton;
        
        public GameObject elementsTitle;
        public GameObject elementsGrid;
        public GameObject elementsBackButton;

        public GameObject elementToggle;
        public GameObject elementTitleContainer;
        public GameObject elementTitleLetter;

        public GameObject demoLivesGrid;
        public GameObject demoLife;
        public GameObject demoOutOfLives;

        private PhysicalElements menu_elements;
        private Dictionary<string, List<Element>> categoriesAndElementsDict = new Dictionary<string, List<Element>>();
        
        private List<Sprite> fontsheet001_sprites;
        private int fontsheet001_letterOffset = 367;
        private int fontsheet001_letterSpaceAdditionalOffset = 5;
        private int fontsheet001_spaceIndex;
        private int fontsheet001_counter0 = 459;
        private int fontsheet001_counter9 = 468;

        private Coroutine showElementTitleCoroutine;

        private bool autoTrigger = false;
        private Coroutine outOfLivesWarningCoroutine;

        // Start is called before the first frame update
        void Start()
        {
            fontsheet001_sprites = new List<Sprite>(Resources.LoadAll<Sprite>("fontsheet001"));
            fontsheet001_spaceIndex = char.ToUpper(' ') + fontsheet001_letterOffset;
            instantiateDemoLives();
            StartCoroutine(createMenuElements());
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void instantiateDemoLives()
        {
            for(int i = 0; i < MetaData.DEMO_MAX_NB_ELEMENTS; ++i)
            {
                GameObject instantiatedDemoLife = Instantiate(demoLife);
                instantiatedDemoLife.transform.SetParent(demoLivesGrid.transform, false);
                UserData.meta_data.demoLives[i] = instantiatedDemoLife;
            }
        }

        IEnumerator createMenuElements()
        {
            // TMP Login Demo User
            // TODO remove once login / user account creation is implemented
            UserData.user = new User();
            yield return StartCoroutine(UserData.user.PostWebRequest(UserData.user.GetUserLoginURI(), JsonUtility.ToJson(UserData.user), UserData.user.LoginCallback));
            
            // TODO Add switch case to generate and manage buttons and titles correctly depending on language

            // Getting all physical elements
            menu_elements = new PhysicalElements();
            yield return StartCoroutine(menu_elements.GetWebRequest(menu_elements.GetPhysicalElementsURI(), menu_elements.PhysicalElementsCallback, UserData.user.cookie));

            // First we store the elements and categories differently for menu creation purposes
            List<Element> elements;
            foreach(Element element in menu_elements.physical_elements)
            {
                element.counter = 1;
                if(categoriesAndElementsDict.TryGetValue(element.category, out elements))
                {
                    elements.Add(element);
                } else
                {
                    elements = new List<Element>();
                    elements.Add(element);
                    categoriesAndElementsDict.Add(element.category, elements);
                }
            }

            // Now we instantiate the categories and the elements
            foreach(string category in categoriesAndElementsDict.Keys)
            {
                GameObject instantiatedCategoryButton = instantiateCategory(category);
                GameObject instantiatedElementsTitle = instantiateElementsTitle(category, instantiatedCategoryButton);
                GameObject instantiatedElements = instantiateElements(category, instantiatedCategoryButton);
                GameObject instantiatedElementsBackButton = instantiateElementsBackButton(category, instantiatedCategoryButton, instantiatedElementsTitle, instantiatedElements);
            }
        }

        private void updateCategoryCounter(GameObject categoryButton, bool increment)
        {
            int currentSpriteIndex;
            if (increment)
            {
                // Cases with two digits
                if (categoryButton.transform.Find(MetaData.SECOND_COUNTER_TAG).GetComponent<Image>().enabled)
                {
                    currentSpriteIndex = fontsheet001_sprites.IndexOf(categoryButton.transform.Find(MetaData.SECOND_COUNTER_TAG).GetComponentInChildren<Image>().sprite);
                    if (currentSpriteIndex == fontsheet001_counter9) // Going from 19 to 20, 29 to 30, etc.
                    {
                        currentSpriteIndex = fontsheet001_sprites.IndexOf(categoryButton.transform.Find(MetaData.COUNTER_TAG).GetComponentInChildren<Image>().sprite);
                        categoryButton.transform.Find(MetaData.COUNTER_TAG).GetComponentInChildren<Image>().sprite = fontsheet001_sprites[currentSpriteIndex+1];
                        categoryButton.transform.Find(MetaData.SECOND_COUNTER_TAG).GetComponentInChildren<Image>().sprite = fontsheet001_sprites[fontsheet001_counter0];
                    }
                    else // Easy case: from x1 to x9
                    {
                        categoryButton.transform.Find(MetaData.SECOND_COUNTER_TAG).GetComponentInChildren<Image>().sprite = fontsheet001_sprites[currentSpriteIndex+1];
                    }
                } else // Cases with currently one digit
                {
                    currentSpriteIndex = fontsheet001_sprites.IndexOf(categoryButton.transform.Find(MetaData.COUNTER_TAG).GetComponentInChildren<Image>().sprite);
                    if (currentSpriteIndex == fontsheet001_counter9) // Going from 9 to 10
                    {
                        categoryButton.transform.Find(MetaData.COUNTER_TAG).GetComponentInChildren<Image>().sprite = fontsheet001_sprites[fontsheet001_counter0+1];
                        categoryButton.transform.Find(MetaData.SECOND_COUNTER_TAG).GetComponent<Image>().enabled = true;
                        categoryButton.transform.Find(MetaData.SECOND_COUNTER_TAG).GetComponentInChildren<Image>().sprite = fontsheet001_sprites[fontsheet001_counter0];
                    } else // General case: from 0 to 9
                    {
                        categoryButton.transform.Find(MetaData.COUNTER_TAG).GetComponentInChildren<Image>().sprite = fontsheet001_sprites[currentSpriteIndex+1];
                    }
                }
            } else // Decrement
            {
                // Cases with two digits
                if (categoryButton.transform.Find(MetaData.SECOND_COUNTER_TAG).GetComponent<Image>().enabled)
                {
                    currentSpriteIndex = fontsheet001_sprites.IndexOf(categoryButton.transform.Find(MetaData.SECOND_COUNTER_TAG).GetComponentInChildren<Image>().sprite);
                    if (currentSpriteIndex == fontsheet001_counter0) // Changing two digits
                    {
                        categoryButton.transform.Find(MetaData.SECOND_COUNTER_TAG).GetComponentInChildren<Image>().sprite = fontsheet001_sprites[currentSpriteIndex-1];
                        currentSpriteIndex = fontsheet001_sprites.IndexOf(categoryButton.transform.Find(MetaData.COUNTER_TAG).GetComponentInChildren<Image>().sprite);
                        if (currentSpriteIndex == fontsheet001_counter0+1) // Going from 10 to 9
                        {
                            categoryButton.transform.Find(MetaData.SECOND_COUNTER_TAG).GetComponent<Image>().enabled = false;
                            categoryButton.transform.Find(MetaData.COUNTER_TAG).GetComponentInChildren<Image>().sprite = fontsheet001_sprites[fontsheet001_counter9];
                        } else // More general case: going from 20 to 19, 30 to 29, etc.
                        {
                            categoryButton.transform.Find(MetaData.COUNTER_TAG).GetComponentInChildren<Image>().sprite = fontsheet001_sprites[currentSpriteIndex-1];
                        }
                    } else { // Changing only second digit
                        categoryButton.transform.Find(MetaData.SECOND_COUNTER_TAG).GetComponentInChildren<Image>().sprite = fontsheet001_sprites[currentSpriteIndex-1];
                    }
                } else // Cases with one digit
                {
                    currentSpriteIndex = fontsheet001_sprites.IndexOf(categoryButton.transform.Find(MetaData.COUNTER_TAG).GetComponentInChildren<Image>().sprite);
                    if (currentSpriteIndex == fontsheet001_counter0) // Should not happen !
                    {
                        Debug.LogError("Decrementing 0... wait, what?");
                    } else // General case: from 9 to 0
                    {
                        categoryButton.transform.Find(MetaData.COUNTER_TAG).GetComponentInChildren<Image>().sprite = fontsheet001_sprites[currentSpriteIndex-1];
                    }
                }
            }
        }

        private void updateSelectedElements(Element element, bool value)
        {
            if (value)
            {
                UserData.selectedElements.Add(element);
            } else {
                UserData.selectedElements.Remove(element);
            }
        }

        private GameObject instantiateCategory(string category)
        {
            // Creating the button
            string icon;
            GameObject instantiatedButton = Instantiate(categoryButton);
            instantiatedButton.transform.SetParent(categoriesGrid.transform, false);
            UserData.meta_data.icon_mapping.TryGetValue(category, out icon);
            instantiatedButton.transform.Find(MetaData.ICON_TAG).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(icon);
            // Adding the onclick behaviour (deactivate current menu elements)
            instantiatedButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.fadeOut(categoriesGrid))
            );
            instantiatedButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.fadeOut(categoriesTitle))
            );
            instantiatedButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(renderButtonFadeOut())
            );
            return instantiatedButton;
        }

        private GameObject instantiateElementsTitle(string category, GameObject instantiatedCategoryButton)
        {
            GameObject instantiatedTitle = Instantiate(elementsTitle);
            instantiatedTitle.transform.SetParent(background.transform, false);
            instantiatedTitle.GetComponentInChildren<Text>().text = category;
            instantiatedCategoryButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.fadeIn(instantiatedTitle))
            );
            return instantiatedTitle;
        }

        private GameObject instantiateElements(string category, GameObject instantiatedCategoryButton)
        {
            GameObject instantiatedGrid = Instantiate(elementsGrid);
            instantiatedGrid.transform.SetParent(background.transform, false);
            instantiatedCategoryButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.fadeIn(instantiatedGrid))
            );
            List<Element> elements;
            string icon;
            categoriesAndElementsDict.TryGetValue(category, out elements);
            foreach(Element element in elements)
            {
                GameObject instantiatedElement = Instantiate(elementToggle);
                instantiatedElement.transform.SetParent(instantiatedGrid.transform, false);
                UserData.meta_data.icon_mapping.TryGetValue(element.name, out icon);
                instantiatedElement.transform.Find(MetaData.BG_TAG + '/' + MetaData.ICON_TAG).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(icon);
                instantiatedElement.GetComponent<Toggle>().onValueChanged.AddListener(
                    (value) => showElementTitleRoutine(element.name, value)
                );
                instantiatedElement.GetComponent<Toggle>().onValueChanged.AddListener(
                    (value) => updateCategoryCounter(instantiatedCategoryButton, value)
                );
                instantiatedElement.GetComponent<Toggle>().onValueChanged.AddListener(
                    (value) => StartCoroutine(updateDemoLives(value, instantiatedElement.GetComponent<Toggle>()))
                );
                instantiatedElement.GetComponent<Toggle>().onValueChanged.AddListener(
                    (value) => updateSelectedElements(element, value)
                );
            }
            return instantiatedGrid;
        }

        private void showElementTitleRoutine(string title, bool value)
        {
            if (value)
            {
                if (showElementTitleCoroutine != null)
                {
                    StopCoroutine(showElementTitleCoroutine);
                }
                showElementTitleCoroutine = StartCoroutine(showElementTitle(title));
            }
        }

        private void hideElementTitle()
        {
            if (showElementTitleCoroutine != null)
            {
                StopCoroutine(showElementTitleCoroutine);
            }
            elementTitleContainer.GetComponent<CanvasGroup>().alpha = 0f;
        }

        private IEnumerator showElementTitle(string title)
        {
            elementTitleContainer.GetComponent<CanvasGroup>().alpha = 0f;
            // First we erase all existing text
            foreach(Transform child in elementTitleContainer.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            // Then we add every letter of selected element
            foreach(char letter in title)
            {
                int fontsheet001_index = char.ToUpper(letter) + fontsheet001_letterOffset;
                if (fontsheet001_index == fontsheet001_spaceIndex)
                {
                    fontsheet001_index += fontsheet001_letterSpaceAdditionalOffset;
                }
                GameObject instantiatedLetter = Instantiate(elementTitleLetter);
                instantiatedLetter.GetComponentInChildren<Image>().sprite = fontsheet001_sprites[fontsheet001_index];
                instantiatedLetter.transform.SetParent(elementTitleContainer.transform, false);
            }
            // Let's fade in the whole thing over half a second
            for (float i = 0; i <= 0.5; i += Time.deltaTime)
            {
                elementTitleContainer.GetComponent<CanvasGroup>().alpha = i*2;
                yield return null;
            }
            // Wait for two seconds
            yield return new WaitForSeconds(2);
            // And now fade out in half a second as well
            for (float i = 0.5f; i >= 0; i -= Time.deltaTime)
            {
                elementTitleContainer.GetComponent<CanvasGroup>().alpha = i*2;
                yield return null;
            }
        }

        private GameObject instantiateElementsBackButton(string category, GameObject instantiatedCategoryButton, GameObject instantiatedElementsTitle, GameObject instantiatedElements)
        {
            GameObject instantiatedBackButton = Instantiate(elementsBackButton);
            instantiatedBackButton.transform.SetParent(background.transform, false);
            instantiatedCategoryButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.fadeIn(instantiatedBackButton))
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.fadeOut(instantiatedElementsTitle))
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => hideElementTitle()
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.fadeOut(instantiatedElements))
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.fadeOut(instantiatedBackButton))
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.fadeIn(categoriesGrid))
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.fadeIn(categoriesTitle))
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(renderButtonFadeIn())
            );
            return instantiatedBackButton;
        }

        private IEnumerator renderButtonFadeIn()
        {
            if (UserData.selectedElements.Count > 0)
            {
                renderButton.GetComponent<Button>().interactable = true;
                StartCoroutine(MenuActions.fadeIn(renderButton));
            } else {
                renderButton.GetComponent<Button>().interactable = false;
                StartCoroutine(MenuActions.fadeIn(renderButton, 0f, 0.5f));
            }
            yield return null;
        }

        private IEnumerator renderButtonFadeOut()
        {
            if (UserData.selectedElements.Count > 0)
            {
                renderButton.GetComponent<Button>().interactable = true;
                StartCoroutine(MenuActions.fadeOut(renderButton));
            } else
            {
                renderButton.GetComponent<Button>().interactable = false;
                StartCoroutine(MenuActions.fadeOut(renderButton, 0.25f, 0f));
            }
            yield return null;
        }

        private IEnumerator updateDemoLives(bool removeLife, Toggle elementToggle)
        {
            if (autoTrigger) // Ugly way of bypassing the listener autocall on toggle.isOn value change.
            {
                autoTrigger = false;
            } else
            {
                if (removeLife)
                {
                    if (UserData.meta_data.currentActiveDemoLives == 0) // All lives have been used...
                    {
                        autoTrigger = true;
                        if (outOfLivesWarningCoroutine != null)
                        {
                            StopCoroutine(outOfLivesWarningCoroutine);
                        }
                        outOfLivesWarningCoroutine = StartCoroutine(outOfDemoLivesWarning());
                        elementToggle.isOn = false;
                    } else
                    {
                        --UserData.meta_data.currentActiveDemoLives;
                        GameObject currentLife = UserData.meta_data.demoLives[UserData.meta_data.currentActiveDemoLives];
                        Image lifeImage = currentLife.GetComponent<Image>();
                        var color = lifeImage.color;
                        for (float i = 0.5f; i >= 0; i -= Time.deltaTime)
                        {
                            color.a = i*2;
                            lifeImage.color = color;
                            yield return null;
                        }
                        currentLife.SetActive(false);
                    }
                } else // Adding a life (deselecting an element)
                {
                    if (UserData.meta_data.currentActiveDemoLives == MetaData.DEMO_MAX_NB_ELEMENTS) // Should not happen !
                    {
                        Debug.LogError("Cannot add a life since they are all there already.");
                    } else
                    {
                        GameObject currentLife = UserData.meta_data.demoLives[UserData.meta_data.currentActiveDemoLives];
                        ++UserData.meta_data.currentActiveDemoLives;
                        currentLife.SetActive(true);
                        Image lifeImage = currentLife.GetComponent<Image>();
                        var color = lifeImage.color;
                        for (float i = 0; i <= 0.5; i += Time.deltaTime)
                        {
                            color.a = i*2;
                            lifeImage.color = color;
                            yield return null;
                        }
                    }
                }
            }
        }

        private IEnumerator outOfDemoLivesWarning()
        {
            demoOutOfLives.GetComponent<Image>().enabled = true;
            for (float i = 0.5f; i >= 0.25f; i -= Time.deltaTime)
            {
                demoOutOfLives.transform.localScale = new Vector3(i*2, i*2, 1);
                yield return null;
            }
            for (float i = 0.5f; i >= 0; i -= Time.deltaTime)
            {
                demoOutOfLives.transform.localScale = new Vector3(i*2, i*2, 1);
                yield return null;
            }
            demoOutOfLives.GetComponent<Image>().enabled = false;
        }
    }
}
