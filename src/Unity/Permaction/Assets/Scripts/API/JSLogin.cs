using UnityEngine;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

public static class JSLogin
{
    
    [DllImport("__Internal")]
    public static extern string login(string url, string formdata);

    /*[DllImport("__Internal")]
    public static extern string getCookies();*/

    [DllImport("__Internal")]
    public static extern string getCookie(string name);
    
}
