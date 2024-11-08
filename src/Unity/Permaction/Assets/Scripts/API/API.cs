﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace API {

    public class API
    {   
        public IEnumerator GetWebRequest(string url, Action<UnityWebRequest> callback = null, string cookie = null, Action<UnityWebRequest> errorCallback = null)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            webRequest.timeout = 3;
            if  (cookie != null)
                webRequest.SetRequestHeader("Cookie", cookie);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError && errorCallback != null)
                errorCallback(webRequest);
            else if (callback != null)
                callback(webRequest);
        }

        public IEnumerator PostWebRequest(string url, string data, Action<UnityWebRequest> callback = null, string cookie = null)
        {
            UnityWebRequest webRequest = UnityWebRequest.Post(url, data);
            UploadHandler customUploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(data));
            customUploadHandler.contentType = "application/json";
            webRequest.uploadHandler = customUploadHandler;
            if  (cookie != null)
                webRequest.SetRequestHeader("Cookie", cookie);
            yield return webRequest.SendWebRequest();
            
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                Debug.LogError(webRequest.error);
            else if (callback != null)
                callback(webRequest);
        }
    }
}
