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
    public GameObject quitCross;
    public GameObject demoLivesGrid;
    public GameObject categoriesTitleEN;
    public GameObject categoriesTitleFR;
    public GameObject categoriesGrid;
    public GameObject renderButtonEN;
    public GameObject renderButtonFR;

    private int id_locale = -1;

    private IEnumerator Start()
    {
        // Waiting for ID locale
        yield return StartCoroutine(WaitForIDLocale());

        // Creating language-dependent menu actions
        if (id_locale == 1) // EN
        {
            playButtonEN.GetComponent<Button>().onClick.AddListener(
                () => playButtonAction(playButtonEN, quitButtonEN, categoriesTitleEN, renderButtonEN)
            );
            StartCoroutine(fadeIn(playButtonEN));
            StartCoroutine(fadeIn(quitButtonEN));
        } else if (id_locale == 2) // FR
        {
            playButtonFR.GetComponent<Button>().onClick.AddListener(
                () => playButtonAction(playButtonFR, quitButtonFR, categoriesTitleFR, renderButtonFR)
            );
            StartCoroutine(fadeIn(playButtonFR));
            StartCoroutine(fadeIn(quitButtonFR));
        } else // ERROR
        {
            Debug.Log("MENU ACTIONS EMPTY ID LOCALE");
        }

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
        quitCross.GetComponent<Button>().onClick.AddListener(
            () => quitButtonAction()
        );
    }

    private IEnumerator WaitForIDLocale()
    {
        while (UserData.user == null || UserData.user.id_locale == -1) {
            yield return new WaitForSeconds(0.1f);
        }
        id_locale = UserData.user.id_locale;
    }

    private void playButtonAction(GameObject playButton, GameObject quitButton, GameObject categoriesTitle, GameObject renderButton)
    {
        StartCoroutine(fadeOut(mainTitle));
        StartCoroutine(fadeOut(playButton));
        StartCoroutine(fadeOut(quitButton));
        StartCoroutine(fadeIn(quitCross));
        StartCoroutine(fadeIn(demoLivesGrid));
        StartCoroutine(fadeIn(categoriesTitle));
        StartCoroutine(fadeIn(categoriesGrid));
        StartCoroutine(renderButtonFadeIn(renderButton));
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
        yield return StartCoroutine(SetTerrainHeightmap());
        yield return StartCoroutine(RenderElements());
        ShowResult();
    }

    private IEnumerator SetTerrainHeightmap()
    {
        SceneManager.LoadScene(MetaData.DEMO_TERRAIN_SCENE, LoadSceneMode.Additive);
        while (!UserData.terrain_loaded)
            yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator RenderElements()
    {
        PlacementRequest placement_request = new PlacementRequest(UserData.terrain_heightmap, UserData.selectedElements.ToArray());
        yield return StartCoroutine(placement_request.PostWebRequest(placement_request.GetPlacementRequestURI(), JsonUtility.ToJson(placement_request), placement_request.APIRendererCallback, UserData.user.cookie));
        UserData.reply = placement_request.GetReply();
    }

    private void ShowResult()
    {
        // TODO V1 Change with user terrain
        SceneManager.LoadScene(MetaData.DEMO_TERRAIN_SCENE);
    }

    private IEnumerator renderButtonFadeIn(GameObject renderButton)
        {
            if (UserData.selectedElements.Count > 0)
            {
                renderButton.GetComponent<Button>().interactable = true;
                StartCoroutine(fadeIn(renderButton));
            } else {
                renderButton.GetComponent<Button>().interactable = false;
                StartCoroutine(fadeIn(renderButton, 0f, 0.5f));
            }
            yield return null;
        }

    /*
        Public static methods.
    */

    public static IEnumerator fadeIn(GameObject gameObject, float startValue = 0.5f, float stopValue = 1f)
        {
            yield return new WaitForSeconds(0.25f);
            gameObject.SetActive(true);
            for (float i = startValue; i <= stopValue; i += Time.deltaTime)
            {
                gameObject.GetComponent<CanvasGroup>().alpha = i;
                yield return null;
            }
        }

    public static IEnumerator fadeOut(GameObject gameObject, float startValue = 0.5f, float stopValue = 0.25f)
    {
        for (float i = startValue; i >= stopValue; i -= Time.deltaTime)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = i*2;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
