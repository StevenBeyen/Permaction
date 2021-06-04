using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Menu;
using API;

public class RenderManager : MonoBehaviour
{
    private PlacementRequest placement_request;
    private const string DEMO_TERRAIN_SCENE = "DemoTerrain";
    private const string ACTIVE_TERRAIN_SCENE = DEMO_TERRAIN_SCENE;

    public IEnumerator Start()
    {
        yield return StartCoroutine(SetTerrainHeightmap());
        yield return StartCoroutine(RenderElements());
        UserData.reply = placement_request.GetReply();
        ShowResult();
    }

    public IEnumerator SetTerrainHeightmap()
    {
        SceneManager.LoadScene(ACTIVE_TERRAIN_SCENE, LoadSceneMode.Additive);
        while (!UserData.terrain_loaded)
            yield return new WaitForSeconds(1);
    }

    public IEnumerator RenderElements()
    {
        placement_request = new PlacementRequest(UserData.terrain_heightmap, UserData.selectedElements.ToArray());
        yield return StartCoroutine(placement_request.PostWebRequest(placement_request.GetPlacementRequestURI(), JsonUtility.ToJson(placement_request), placement_request.APIRendererCallback, UserData.user.cookie));
    }

    public void ShowResult()
    {
        SceneManager.LoadScene(ACTIVE_TERRAIN_SCENE);
    }
}
