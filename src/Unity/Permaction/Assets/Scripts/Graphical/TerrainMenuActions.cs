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
    public GameObject zoomInButton;
    public GameObject zoomOutButton;

    public GameObject audio;
    public Camera camera;

    private bool menuHandleOpen = false;

    private float zoomSpeed = 2.5f;
    private float zoomMinBound = 20f;
    private float zoomMaxBound = 75f;

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
        zoomInButton.GetComponent<Button>().onClick.AddListener(
            () => ZoomInButtonAction()
        );
        zoomOutButton.GetComponent<Button>().onClick.AddListener(
            () => ZoomOutButtonAction()
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

    private void ZoomInButtonAction()
    {
        camera.fieldOfView -= zoomSpeed;
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, zoomMinBound, zoomMaxBound);
        if (camera.fieldOfView == zoomMinBound)
            zoomInButton.GetComponent<Button>().interactable = false;
        zoomOutButton.GetComponent<Button>().interactable = true;
    }

    private void ZoomOutButtonAction()
    {
        camera.fieldOfView += zoomSpeed;
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, zoomMinBound, zoomMaxBound);
        if (camera.fieldOfView == zoomMaxBound)
            zoomOutButton.GetComponent<Button>().interactable = false;
        zoomInButton.GetComponent<Button>().interactable = true;
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
}
