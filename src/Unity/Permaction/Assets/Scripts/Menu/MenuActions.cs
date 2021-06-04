using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using API;

public class MenuActions : MonoBehaviour
{
    public GameObject mainTitle;
    public GameObject playButton;
    public GameObject quitButton;
    public GameObject quitCross;
    public GameObject demoLivesGrid;
    public GameObject categoriesTitle;
    public GameObject categoriesGrid;
    public GameObject renderButton;

    private void Start()
    {
        playButton.GetComponent<Button>().onClick.AddListener(
            () => playButtonAction()
        );
        renderButton.GetComponent<Button>().onClick.AddListener(
            () => renderButtonAction()
        );
        quitButton.GetComponent<Button>().onClick.AddListener(
            () => quitButtonAction()
        );
        quitCross.GetComponent<Button>().onClick.AddListener(
            () => quitButtonAction()
        );
    }

    private void playButtonAction()
    {
        StartCoroutine(fadeOut(mainTitle));
        StartCoroutine(fadeOut(playButton));
        StartCoroutine(fadeOut(quitButton));
        StartCoroutine(fadeIn(quitCross));
        StartCoroutine(fadeIn(demoLivesGrid));
        StartCoroutine(fadeIn(categoriesTitle));
        StartCoroutine(fadeIn(categoriesGrid));
        StartCoroutine(renderButtonFadeIn());
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

    private IEnumerator renderButtonFadeIn()
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
