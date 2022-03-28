using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using API;

public class TerrainMenuActions : MonoBehaviour
{
    public GameObject root;
    public GameObject menuHandle;
    public GameObject homeButton;
    public GameObject volumeButton;
    public GameObject noVolumeButton;

    public GameObject audio;

    private bool menuHandleOpen = false;

    private void Start()
    {
        menuHandle.GetComponent<Button>().onClick.AddListener(
            () => MenuHandleAction()
        );
        homeButton.GetComponent<Button>().onClick.AddListener(
            () => HomeButtonAction()
        );
        volumeButton.GetComponent<Button>().onClick.AddListener(
            () => VolumeButtonAction()
        );
        noVolumeButton.GetComponent<Button>().onClick.AddListener(
            () => NoVolumeButtonAction()
        );
    }

    private void MenuHandleAction()
    {
        if (menuHandleOpen)
        {
            StartCoroutine(RotateClose(menuHandle));
            StartCoroutine(SlideOut(homeButton));
            StartCoroutine(SlideOut(volumeButton));
            StartCoroutine(SlideOut(noVolumeButton));
        } else
        {
            StartCoroutine(RotateOpen(menuHandle));
            StartCoroutine(SlideIn(homeButton));
            StartCoroutine(SlideIn(volumeButton));
            StartCoroutine(SlideIn(noVolumeButton));
        }
        menuHandleOpen = !menuHandleOpen;
    }

    private void HomeButtonAction()
    {
        /*UserData.DEMO_MENU_ASYNC_SCENE = SceneManager.LoadSceneAsync(MetaData.DEMO_MENU_SCENE, LoadSceneMode.Additive);
        root.SetActive(false);
        UserData.DEMO_MENU_ASYNC_SCENE.allowSceneActivation = true;*/
        SceneManager.LoadScene(MetaData.DEMO_MENU_SCENE);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(MetaData.DEMO_MENU_SCENE));
    }

    private void VolumeButtonAction()
    {
        audio.SetActive(true);
        volumeButton.SetActive(false);
        noVolumeButton.SetActive(true);
    }

    private void NoVolumeButtonAction()
    {
        audio.SetActive(false);
        volumeButton.SetActive(true);
        noVolumeButton.SetActive(false);
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

    public static IEnumerator RotateOpen(GameObject gameObject, float startValue = 0f, float stopValue = -0.5f)
    {
        gameObject.GetComponent<Button>().interactable = false;
        for (float i = startValue; i >= stopValue; i -= Time.deltaTime)
        {
            gameObject.transform.localEulerAngles = new Vector3(
                gameObject.transform.localEulerAngles.x,
                gameObject.transform.localEulerAngles.y,
                i * 720
            );
            yield return null;
        }
        // Final value set
        gameObject.transform.localEulerAngles = new Vector3(
            gameObject.transform.localEulerAngles.x,
            gameObject.transform.localEulerAngles.y,
            stopValue * 720
        );
        gameObject.GetComponent<Button>().interactable = true;
    }

    public static IEnumerator RotateClose(GameObject gameObject, float startValue = -0.5f, float stopValue = 0f)
    {
        gameObject.GetComponent<Button>().interactable = false;
        for (float i = startValue; i <= stopValue; i += Time.deltaTime)
        {
            gameObject.transform.localEulerAngles = new Vector3(
                gameObject.transform.localEulerAngles.x,
                gameObject.transform.localEulerAngles.y,
                i * 720
            );
            yield return null;
        }
        // Final value set
        gameObject.transform.localEulerAngles = new Vector3(
            gameObject.transform.localEulerAngles.x,
            gameObject.transform.localEulerAngles.y,
            stopValue * 720
        );
        gameObject.GetComponent<Button>().interactable = true;
    }

    public static IEnumerator SlideIn(GameObject gameObject, float startValue = 0f, float stopValue = 0.5f)
    {
        float x = gameObject.transform.localPosition.x;
        for (float i = startValue; i <= stopValue; i += Time.deltaTime)
        {
            gameObject.transform.localPosition = new Vector3(
                x + i * 300,
                gameObject.transform.localPosition.y,
                gameObject.transform.localPosition.z
            );
            yield return null;
        }
        // Final value set
        gameObject.transform.localPosition = new Vector3(
            x + stopValue * 300,
            gameObject.transform.localPosition.y,
            gameObject.transform.localPosition.z
        );
    }

    public static IEnumerator SlideOut(GameObject gameObject, float startValue = 0f, float stopValue = 0.5f)
    {
        float x = gameObject.transform.localPosition.x;
        for (float i = startValue; i <= stopValue; i += Time.deltaTime)
        {
            gameObject.transform.localPosition = new Vector3(
                x - i * 300,
                gameObject.transform.localPosition.y,
                gameObject.transform.localPosition.z
            );
            yield return null;
        }
        // Final value set
        gameObject.transform.localPosition = new Vector3(
            x - stopValue * 300,
            gameObject.transform.localPosition.y,
            gameObject.transform.localPosition.z
        );
    }

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
