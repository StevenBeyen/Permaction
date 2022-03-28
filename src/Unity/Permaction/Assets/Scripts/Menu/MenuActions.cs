using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using API;

public class MenuActions : MonoBehaviour
{
    public GameObject root;
    public GameObject mainTitleContainer;
    public GameObject languageSelectionContainer;
    public GameObject languageButtonEN;
    public GameObject languageButtonFR;
    public GameObject infoPanelEN;
    public GameObject infoPanelCloseEN;
    public GameObject infoPanelFR;
    public GameObject infoPanelCloseFR;
    public GameObject menuContainer;
    public GameObject mainTitle;
    public GameObject playButtonEN;
    public GameObject playButtonFR;
    public GameObject previousGenerationButtonEN;
    public GameObject previousGenerationButtonFR;
    public GameObject opinionButtonEN;
    public GameObject opinionButtonFR;
    public GameObject homeButton;
    public GameObject infoButton;
    public GameObject demoLivesGrid;
    public GameObject categoriesTitleEN;
    public GameObject categoriesTitleFR;
    public GameObject categoriesGrid;
    public GameObject renderButtonEN;
    public GameObject renderButtonFR;
    public GameObject loadingAnimation;
    public GameObject loadingTipsTitleEN;
    public GameObject loadingTipsTitleFR;
    public GameObject loadingTips;

    private int id_locale = -1;
    private GameObject infoPanel;
    private GameObject infoPanelClose;
    private GameObject playButton;
    private GameObject previousGenerationButton;
    private GameObject opinionButton;
    private GameObject categoriesTitle;
    private GameObject renderButton;
    private GameObject loadingTipsTitle;

    private void Start()
    {
        // Language selection buttons
        languageButtonEN.GetComponent<Button>().onClick.AddListener(
            () => StartCoroutine(languageButtonENAction())
        );
        languageButtonFR.GetComponent<Button>().onClick.AddListener(
            () => StartCoroutine(languageButtonFRAction())
        );

        if (UserData.user != null)
            id_locale = UserData.user.id_locale;

        // Language selection routine
        if (id_locale == -1)
            StartCoroutine(FadeIn(languageSelectionContainer));
        else
            createMenu();
    }

    private void createMenu()
    {
        // Workaround for 2+ generation static variable reset...
        UserData.meta_data.terrain = null;
        UserData.terrain_loaded = false;
        UserData.reply = null;
        UserData.elements_loaded = false;
        UserData.tips = new List<string>();
        UserData.physical_elements = new List<PhysicalElement>();

        // Creating language-dependent menu variables
        if (id_locale == UserData.meta_data.id_locale_mapping["en"]) // English
        {
            infoPanel = infoPanelEN;
            infoPanelClose = infoPanelCloseEN;
            playButton = playButtonEN;
            previousGenerationButton = previousGenerationButtonEN;
            opinionButton = opinionButtonEN;
            categoriesTitle = categoriesTitleEN;
            renderButton = renderButtonEN;
            loadingTipsTitle = loadingTipsTitleEN;
        } else if (id_locale == UserData.meta_data.id_locale_mapping["fr"]) // French
        {
            infoPanel = infoPanelFR;
            infoPanelClose = infoPanelCloseFR;
            playButton = playButtonFR;
            previousGenerationButton = previousGenerationButtonFR;
            opinionButton = opinionButtonFR;
            categoriesTitle = categoriesTitleFR;
            renderButton = renderButtonFR;
            loadingTipsTitle = loadingTipsTitleFR;
        } else // ERROR
        {
            Debug.Log("[Menu Actions] No ID locale?");
            // TODO Add locale selection routine
        }
        infoPanelClose.GetComponent<Button>().onClick.AddListener(
            () => infoPanelCloseAction()
        );
        playButton.GetComponent<Button>().onClick.AddListener(
            () => playButtonAction()
        );

        StartCoroutine(FadeOut(languageSelectionContainer));
        //StartCoroutine(FadeIn(menuContainer));
        StartCoroutine(FadeIn(playButton));
        //StartCoroutine(FadeIn(previousGenerationButton));
        StartCoroutine(FadeIn(opinionButton));

        // Creating language-independent menu actions
        previousGenerationButton.GetComponent<Button>().onClick.AddListener(
            () => previousGenerationAction()
        );
        /*previousGenerationButtonFR.GetComponent<Button>().AddListener(
            () => previousGenerationAction()
        );*/
        renderButton.GetComponent<Button>().onClick.AddListener(
            () => renderButtonAction()
        );
        /*renderButtonFR.GetComponent<Button>().onClick.AddListener(
            () => renderButtonAction()
        );*/
        homeButton.GetComponent<Button>().onClick.AddListener(
            () => homeButtonAction()
        );
        infoButton.GetComponent<Button>().onClick.AddListener(
            () => infoButtonAction()
        );
    }

    private IEnumerator languageButtonENAction()
    {
        id_locale = UserData.meta_data.id_locale_mapping["en"];
        yield return StartCoroutine(demoUserLogin());
        createMenu();
        StartCoroutine(FadeOut(mainTitleContainer));
        StartCoroutine(FadeIn(infoPanel));
    }

    private IEnumerator languageButtonFRAction()
    {
        id_locale = UserData.meta_data.id_locale_mapping["fr"];
        yield return StartCoroutine(demoUserLogin());
        createMenu();
        StartCoroutine(FadeOut(mainTitleContainer));
        StartCoroutine(FadeIn(infoPanel));
    }

    private IEnumerator demoUserLogin()
    {
        UserData.user = new User(null, null, id_locale, null);
        yield return StartCoroutine(UserData.user.PostWebRequest(MetaData.USER_LOGIN_URI, JsonUtility.ToJson(UserData.user), UserData.user.LoginCallback));
    }

    private void infoPanelCloseAction()
    {
        StartCoroutine(FadeOut(infoPanel));
        StartCoroutine(FadeIn(mainTitleContainer));
        StartCoroutine(FadeIn(menuContainer));
        infoPanel.GetComponentInChildren<Scrollbar>().value = 1;
    }

    private void playButtonAction()
    {
        StartCoroutine(FadeOut(mainTitle));
        StartCoroutine(FadeOut(playButton));
        //StartCoroutine(FadeOut(previousGenerationButton));
        StartCoroutine(FadeOut(opinionButton));
        StartCoroutine(FadeIn(homeButton));
        StartCoroutine(FadeIn(demoLivesGrid));
        StartCoroutine(FadeIn(categoriesTitle));
        StartCoroutine(FadeIn(categoriesGrid));
        StartCoroutine(RenderButtonFadeIn(renderButton));
    }

    private void previousGenerationAction()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(MetaData.DEMO_TERRAIN_SCENE));
    }

    private void homeButtonAction()
    {
        StartCoroutine(FadeOut(homeButton));
        StartCoroutine(FadeOut(demoLivesGrid));
        StartCoroutine(FadeOut(categoriesTitle));
        StartCoroutine(FadeOut(categoriesGrid));
        StartCoroutine(FadeOut(renderButton));
        StartCoroutine(FadeIn(mainTitle));
        StartCoroutine(FadeIn(playButton));
        //StartCoroutine(FadeIn(previousGenerationButton));
        StartCoroutine(FadeIn(opinionButton));
    }

    private void infoButtonAction()
    {
        StartCoroutine(FadeOut(mainTitleContainer));
        StartCoroutine(FadeOut(menuContainer));
        StartCoroutine(FadeIn(infoPanel));
    }

    private void renderButtonAction()
    {
        StartCoroutine(FadeOut(categoriesTitle));
        StartCoroutine(FadeOut(categoriesGrid));
        StartCoroutine(RenderRoutine());
    }

    /*private void quitButtonAction()
    {
        Application.Quit();
    }*/

    private IEnumerator RenderRoutine()
    {
        FilterTips();
        StartCoroutine(FadeOut(renderButton));
        StartCoroutine(FadeIn(loadingAnimation));
        StartCoroutine(FadeIn(loadingTipsTitle));
        StartCoroutine(renderTips());
        yield return StartCoroutine(SetTerrainHeightmap());
        yield return StartCoroutine(RenderElements());
        ShowResult();
    }

    private void FilterTips()
    {
        List<int> element_ids = new List<int>();
        foreach (Element element in UserData.selected_elements)
            element_ids.Add(element.id);
        foreach (BinaryInteraction bi in UserData.binary_interactions)
        {
            if (element_ids.Contains(bi.element1_id) || element_ids.Contains(bi.element2_id))
                UserData.tips.Add(bi.description);
        }
    }

    private IEnumerator renderTips()
    {
        int i;
        while(true)
        {
            i = Random.Range(0, UserData.tips.Count);
            loadingTips.GetComponent<Text>().text = UserData.tips[i];
            yield return StartCoroutine(FadeIn(loadingTips));
            yield return new WaitForSeconds(10.0f);
            yield return StartCoroutine(FadeOut(loadingTips));
        }
    }

    private IEnumerator SetTerrainHeightmap()
    {
        //yield return new WaitForSeconds(0.0f);
        SceneManager.LoadScene(MetaData.DEMO_TERRAIN_SCENE, LoadSceneMode.Additive);
        while (!UserData.terrain_loaded)
            yield return new WaitForSeconds(0.01f);
    }

    private IEnumerator RenderElements()
    {
        PlacementRequest placement_request = new PlacementRequest(UserData.terrain_heightmap, UserData.selected_elements.ToArray());
        yield return StartCoroutine(placement_request.PostWebRequest(MetaData.PLACEMENT_REQUEST_URI, JsonUtility.ToJson(placement_request), placement_request.APIRendererCallback, UserData.user.cookie));
        UserData.reply = placement_request.GetReply();
        // Resetting user selection
        UserData.selected_elements = new List<Element>();
        UserData.meta_data.current_active_demo_lives = MetaData.DEMO_MAX_NB_ELEMENTS;
    }

    private void ShowResult()
    {
        // TODO V1 Change with user terrain
        SceneManager.LoadScene(MetaData.DEMO_TERRAIN_SCENE);
    }

    private IEnumerator RenderButtonFadeIn(GameObject renderButton)
        {
            if (UserData.selected_elements.Count > 0)
            {
                renderButton.GetComponent<Button>().interactable = true;
                StartCoroutine(FadeIn(renderButton));
            } else {
                renderButton.GetComponent<Button>().interactable = false;
                StartCoroutine(FadeIn(renderButton, 0f, 0.5f));
            }
            yield return null;
        }

    /*
        Public static methods.
    */

    public static IEnumerator FadeIn(GameObject gameObject, float startValue = 0.5f, float stopValue = 1f)
        {
            yield return new WaitForSeconds(0.25f);
            gameObject.SetActive(true);
            for (float i = startValue; i <= stopValue; i += Time.deltaTime)
            {
                gameObject.GetComponent<CanvasGroup>().alpha = i;
                yield return null;
            }
        }

    public static IEnumerator FadeOut(GameObject gameObject, float startValue = 0.5f, float stopValue = 0.25f)
    {
        for (float i = startValue; i >= stopValue; i -= Time.deltaTime)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = i*2;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
