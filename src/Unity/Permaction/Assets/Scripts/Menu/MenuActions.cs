using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using API;

public class MenuActions : MonoBehaviour
{
    public GameObject mainTitle;
    public GameObject playButtonEN;
    public GameObject playButtonFR;
    public GameObject quitButtonEN;
    public GameObject quitButtonFR;
    public GameObject homeButton;
    public GameObject demoLivesGrid;
    public GameObject categoriesTitleEN;
    public GameObject categoriesTitleFR;
    public GameObject categoriesGrid;
    public GameObject renderButtonEN;
    public GameObject renderButtonFR;

    private int id_locale = -1;
    private GameObject playButton;
    private GameObject quitButton;
    private GameObject categoriesTitle;
    private GameObject renderButton;

    private IEnumerator Start()
    {
        // Waiting for ID locale
        yield return StartCoroutine(WaitForIDLocale());

        // Creating language-dependent menu actions
        if (id_locale == UserData.meta_data.id_locale_mapping["en"]) // English
        {
            playButton = playButtonEN;
            quitButton = quitButtonEN;
            categoriesTitle = categoriesTitleEN;
            renderButton = renderButtonEN;
        } else if (id_locale == UserData.meta_data.id_locale_mapping["fr"]) // French
        {
            playButton = playButtonFR;
            quitButton = quitButtonFR;
            categoriesTitle = categoriesTitleFR;
            renderButton = renderButtonFR;
        } else // ERROR
        {
            Debug.Log("[Menu Actions] No ID locale?");
            // TODO Add locale selection routine
        }
        playButton.GetComponent<Button>().onClick.AddListener(
            () => playButtonAction()
        );
        StartCoroutine(FadeIn(playButton));
        StartCoroutine(FadeIn(quitButton));

        // Creating language-independent menu actions
        quitButtonEN.GetComponent<Button>().onClick.AddListener(
            () => quitButtonAction()
        );
        quitButtonFR.GetComponent<Button>().onClick.AddListener(
            () => quitButtonAction()
        );
        renderButtonEN.GetComponent<Button>().onClick.AddListener(
            () => renderButtonAction()
        );
        renderButtonFR.GetComponent<Button>().onClick.AddListener(
            () => renderButtonAction()
        );
        homeButton.GetComponent<Button>().onClick.AddListener(
            () => homeButtonAction()
        );
    }

    private IEnumerator WaitForIDLocale()
    {
        while (UserData.user == null || UserData.user.id_locale == 0) {
            yield return new WaitForSeconds(0.1f);
        }
        id_locale = UserData.user.id_locale;
    }

    private void playButtonAction()
    {
        StartCoroutine(FadeOut(mainTitle));
        StartCoroutine(FadeOut(playButton));
        StartCoroutine(FadeOut(quitButton));
        StartCoroutine(FadeIn(homeButton));
        StartCoroutine(FadeIn(demoLivesGrid));
        StartCoroutine(FadeIn(categoriesTitle));
        StartCoroutine(FadeIn(categoriesGrid));
        StartCoroutine(RenderButtonFadeIn(renderButton));
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
        StartCoroutine(FadeIn(quitButton));
    }

    private void renderButtonAction()
    {
        // TODO Add waiting animation
        StartCoroutine(RenderRoutine());
    }

    private void quitButtonAction()
    {
        Application.Quit();
    }

    private IEnumerator RenderRoutine()
    {
        yield return StartCoroutine(GetBinaryInteractions());
        yield return StartCoroutine(SetTerrainHeightmap());
        yield return StartCoroutine(RenderElements());
        ShowResult();
    }

    private IEnumerator GetBinaryInteractions()
    {
        BinaryInteractions binary_interactions = new BinaryInteractions();
        yield return StartCoroutine(binary_interactions.GetWebRequest(MetaData.BINARY_INTERACTIONS_URI, binary_interactions.BinaryInteractionsCallback, UserData.user.cookie));
        UserData.binary_interactions = binary_interactions;
    }

    private IEnumerator SetTerrainHeightmap()
    {
        SceneManager.LoadScene(MetaData.DEMO_TERRAIN_SCENE, LoadSceneMode.Additive);
        while (!UserData.terrain_loaded)
            yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator RenderElements()
    {
        PlacementRequest placement_request = new PlacementRequest(UserData.terrain_heightmap, UserData.selected_elements.ToArray());
        yield return StartCoroutine(placement_request.PostWebRequest(MetaData.PLACEMENT_REQUEST_URI, JsonUtility.ToJson(placement_request), placement_request.APIRendererCallback, UserData.user.cookie));
        UserData.reply = placement_request.GetReply();
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
