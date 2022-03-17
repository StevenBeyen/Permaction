using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using API;

namespace Menu {
    public class MenuElementsCreation : MonoBehaviour
    {
        public GameObject background;

        public GameObject homeButton;

        public GameObject categoriesGrid;
        private GameObject categoriesTitle;
        private GameObject renderButton;

        public GameObject categoriesTitleEN;
        public GameObject categoriesTitleFR;
        public GameObject renderButtonEN;
        public GameObject renderButtonFR;

        public GameObject categoryButton;
        
        public GameObject elementsTitle;
        public GameObject elementsGrid;
        public GameObject elementsGridAlt;
        private GameObject elementsBackButton;

        public GameObject elementsBackButtonEN;
        public GameObject elementsBackButtonFR;

        public GameObject elementToggle;
        public GameObject elementTitleContainer;
        public GameObject elementTitleLetter;

        public GameObject demoLivesGrid;
        public GameObject demoLife;
        public GameObject demoOutOfLives;

        private List<GameObject> homeButtonFadeOutList = new List<GameObject>();

        private PhysicalElements menu_elements;
        private Dictionary<string, List<Element>> categoriesAndElementsDict = new Dictionary<string, List<Element>>();
        
        private List<Sprite> fontsheet001_sprites;
        private Sprite fontsheet001_space;
        private int fontsheet001_letterOffset = -65;
        private int fontsheet001_spaceIndex = -33; // 32 (space) - 65 (offset)
        private int fontsheet001_counter0 = 26;
        private int fontsheet001_counter9 = 35;

        private Coroutine ShowElementTitleCoroutine;

        private bool autoTrigger = false;
        private Coroutine outOfLivesWarningCoroutine;

        // Start is called before the first frame update
        void Start()
        {
            fontsheet001_sprites = new List<Sprite>(Resources.LoadAll<Sprite>("2108.w032.n003.58B.p51.58(2)"));
            fontsheet001_space = Resources.Load<Sprite>("space");
            InstantiateDemoLives();
            homeButton.GetComponent<Button>().onClick.AddListener(
                () => homeButtonAction()
            );
            StartCoroutine(CreateMenuElements());
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void InstantiateDemoLives()
        {
            for(int i = 0; i < MetaData.DEMO_MAX_NB_ELEMENTS; ++i)
            {
                GameObject instantiatedDemoLife = Instantiate(demoLife);
                instantiatedDemoLife.transform.SetParent(demoLivesGrid.transform, false);
                UserData.meta_data.demo_lives[i] = instantiatedDemoLife;
            }
        }

        private void homeButtonAction()
        {
            foreach (GameObject go in homeButtonFadeOutList)
                StartCoroutine(MenuActions.FadeOut(go));
        }

        IEnumerator CreateMenuElements()
        {
            // TMP Login Demo User
            // TODO remove once login / user account creation is implemented
            UserData.user = new User();
            yield return StartCoroutine(UserData.user.PostWebRequest(MetaData.USER_LOGIN_URI, JsonUtility.ToJson(UserData.user), UserData.user.LoginCallback));

            // TODO Add switch case to generate and manage buttons and titles correctly depending on language
            if (UserData.user.id_locale == UserData.meta_data.id_locale_mapping["en"]) // English
            {
                categoriesTitle = categoriesTitleEN;
                renderButton = renderButtonEN;
                elementsBackButton = elementsBackButtonEN;
            } else if (UserData.user.id_locale == UserData.meta_data.id_locale_mapping["fr"]) // French
            {
                categoriesTitle = categoriesTitleFR;
                renderButton = renderButtonFR;
                elementsBackButton = elementsBackButtonFR;
            } else
            {
                Debug.Log("[Menu Elements Creation] No ID locale?");
                // TODO Add locale selection routine
            }

            // Getting all physical elements
            menu_elements = new PhysicalElements();
            yield return StartCoroutine(menu_elements.GetWebRequest(MetaData.PHYSICAL_ELEMENTS_URI, menu_elements.PhysicalElementsCallback, UserData.user.cookie));
            // Let's store some data for the end visual result
            UserData.meta_data.ExtractData(menu_elements.physical_elements);

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
                GameObject instantiatedCategoryButton = InstantiateCategory(category);
                GameObject instantiatedElementsTitle = InstantiateElementsTitle(category, instantiatedCategoryButton);
                GameObject instantiatedElements = InstantiateElements(category, instantiatedCategoryButton);
                GameObject instantiatedElementsBackButton = InstantiateElementsBackButton(category, instantiatedCategoryButton, instantiatedElementsTitle, instantiatedElements);
                homeButtonFadeOutList.Add(instantiatedElementsTitle);
                homeButtonFadeOutList.Add(instantiatedElements);
                homeButtonFadeOutList.Add(instantiatedElementsBackButton);
            }
        }

        private void UpdateCategoryCounter(GameObject categoryButton, bool increment)
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

        private void UpdateSelectedElements(Element element, bool value)
        {
            if (value)
            {
                UserData.selected_elements.Add(element);
            } else {
                UserData.selected_elements.Remove(element);
            }
        }

        private GameObject InstantiateCategory(string category)
        {
            // Creating the button
            string icon;
            GameObject instantiatedButton = Instantiate(categoryButton);
            instantiatedButton.transform.SetParent(categoriesGrid.transform, false);
            UserData.meta_data.icon_mapping.TryGetValue(category, out icon);
            instantiatedButton.transform.Find(MetaData.ICON_TAG).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(icon);
            // Adding the onclick behaviour (deactivate current menu elements)
            instantiatedButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.FadeOut(categoriesGrid))
            );
            instantiatedButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.FadeOut(categoriesTitle))
            );
            instantiatedButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(RenderButtonFadeOut())
            );
            return instantiatedButton;
        }

        private GameObject InstantiateElementsTitle(string category, GameObject instantiatedCategoryButton)
        {
            GameObject instantiatedTitle = Instantiate(elementsTitle);
            instantiatedTitle.transform.SetParent(background.transform, false);
            instantiatedTitle.GetComponentInChildren<Text>().text = category;
            instantiatedCategoryButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.FadeIn(instantiatedTitle))
            );
            return instantiatedTitle;
        }

        private GameObject InstantiateElements(string category, GameObject instantiatedCategoryButton)
        {
            GameObject grid;
            List<Element> elements;
            string icon;
            categoriesAndElementsDict.TryGetValue(category, out elements);
            if (elements.Count > 6)
                grid = elementsGridAlt;
            else
                grid = elementsGrid;
            GameObject instantiatedGrid = Instantiate(grid);
            instantiatedGrid.transform.SetParent(background.transform, false);
            instantiatedCategoryButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.FadeIn(instantiatedGrid))
            );
            foreach(Element element in elements)
            {
                GameObject instantiatedElement = Instantiate(elementToggle);
                instantiatedElement.transform.SetParent(instantiatedGrid.transform, false);
                UserData.meta_data.icon_mapping.TryGetValue(element.name, out icon);
                instantiatedElement.transform.Find(MetaData.BG_TAG + '/' + MetaData.ICON_TAG).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(icon);
                instantiatedElement.GetComponent<Toggle>().onValueChanged.AddListener(
                    (value) => ShowElementTitleRoutine(element.name, value)
                );
                instantiatedElement.GetComponent<Toggle>().onValueChanged.AddListener(
                    (value) => UpdateCategoryCounter(instantiatedCategoryButton, value)
                );
                instantiatedElement.GetComponent<Toggle>().onValueChanged.AddListener(
                    (value) => StartCoroutine(UpdateDemoLives(value, instantiatedElement.GetComponent<Toggle>()))
                );
                instantiatedElement.GetComponent<Toggle>().onValueChanged.AddListener(
                    (value) => UpdateSelectedElements(element, value)
                );
            }
            return instantiatedGrid;
        }

        private void ShowElementTitleRoutine(string title, bool value)
        {
            if (value)
            {
                if (ShowElementTitleCoroutine != null)
                {
                    StopCoroutine(ShowElementTitleCoroutine);
                }
                ShowElementTitleCoroutine = StartCoroutine(ShowElementTitle(title));
            }
        }

        private void HideElementTitle()
        {
            if (ShowElementTitleCoroutine != null)
            {
                StopCoroutine(ShowElementTitleCoroutine);
            }
            elementTitleContainer.GetComponent<CanvasGroup>().alpha = 0f;
        }

        private IEnumerator ShowElementTitle(string title)
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
                Sprite sprite;
                int fontsheet001_index = char.ToUpper(letter) + fontsheet001_letterOffset;
                if (fontsheet001_index == fontsheet001_spaceIndex)
                {
                    sprite = fontsheet001_space;
                } else
                {
                    sprite = fontsheet001_sprites[fontsheet001_index];
                }
                GameObject instantiatedLetter = Instantiate(elementTitleLetter);
                instantiatedLetter.GetComponentInChildren<Image>().sprite = sprite;
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

        private GameObject InstantiateElementsBackButton(string category, GameObject instantiatedCategoryButton, GameObject instantiatedElementsTitle, GameObject instantiatedElements)
        {
            GameObject instantiatedBackButton = Instantiate(elementsBackButton);
            instantiatedBackButton.transform.SetParent(background.transform, false);
            instantiatedCategoryButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.FadeIn(instantiatedBackButton))
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.FadeOut(instantiatedElementsTitle))
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => HideElementTitle()
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.FadeOut(instantiatedElements))
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.FadeOut(instantiatedBackButton))
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.FadeIn(categoriesGrid))
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(MenuActions.FadeIn(categoriesTitle))
            );
            instantiatedBackButton.GetComponent<Button>().onClick.AddListener(
                () => StartCoroutine(RenderButtonFadeIn())
            );
            return instantiatedBackButton;
        }

        private IEnumerator RenderButtonFadeIn()
        {
            if (UserData.selected_elements.Count > 0)
            {
                renderButton.GetComponent<Button>().interactable = true;
                StartCoroutine(MenuActions.FadeIn(renderButton));
            } else {
                renderButton.GetComponent<Button>().interactable = false;
                StartCoroutine(MenuActions.FadeIn(renderButton, 0f, 0.5f));
            }
            yield return null;
        }

        private IEnumerator RenderButtonFadeOut()
        {
            if (UserData.selected_elements.Count > 0)
            {
                renderButton.GetComponent<Button>().interactable = true;
                StartCoroutine(MenuActions.FadeOut(renderButton));
            } else
            {
                renderButton.GetComponent<Button>().interactable = false;
                StartCoroutine(MenuActions.FadeOut(renderButton, 0.25f, 0f));
            }
            yield return null;
        }

        private IEnumerator UpdateDemoLives(bool removeLife, Toggle elementToggle)
        {
            if (autoTrigger) // Ugly way of bypassing the listener autocall on toggle.isOn value change.
            {
                autoTrigger = false;
            } else
            {
                if (removeLife)
                {
                    if (UserData.meta_data.current_active_demo_lives == 0) // All lives have been used...
                    {
                        autoTrigger = true;
                        if (outOfLivesWarningCoroutine != null)
                        {
                            StopCoroutine(outOfLivesWarningCoroutine);
                        }
                        outOfLivesWarningCoroutine = StartCoroutine(OutOfDemoLivesWarning());
                        elementToggle.isOn = false;
                    } else
                    {
                        --UserData.meta_data.current_active_demo_lives;
                        GameObject currentLife = UserData.meta_data.demo_lives[UserData.meta_data.current_active_demo_lives];
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
                    if (UserData.meta_data.current_active_demo_lives == MetaData.DEMO_MAX_NB_ELEMENTS) // Should not happen !
                    {
                        Debug.LogError("Cannot add a life since they are all there already.");
                    } else
                    {
                        GameObject currentLife = UserData.meta_data.demo_lives[UserData.meta_data.current_active_demo_lives];
                        ++UserData.meta_data.current_active_demo_lives;
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

        private IEnumerator OutOfDemoLivesWarning()
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
