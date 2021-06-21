using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace API
{
    [System.Serializable]
    public class User : API
    {
        private const string TEST_USERNAME = "testuser_fr";
        private const string TEST_PASSWORD = "testuser_fr";

        private const string USER_LOGIN_URI = API_URI + "/login";
        private const string USER_SIGNUP_URI = API_URI + "/signup";

        public string username;
        public string password;
        public int id_locale;
        public string cookie;

        public User(string username = null, string password = null, int id_locale = -1, string cookie = null)
        {
            // TODO Remove once login/signup is implemented
            if((username == null) && (password == null))
            {
                this.username = TEST_USERNAME;
                this.password = TEST_PASSWORD;
            }
            else if((username == null) || (password == null))
            {
                // TODO Change this for a pop-up
                Debug.LogError("Cannot specify username without password.");
            }
            else
            {
                this.username = username;
                this.password = password;      
                this.id_locale = id_locale;
                this.cookie = cookie;
            }
        }

        public void LoginCallback(UnityWebRequest webRequest)
        {
            cookie = webRequest.GetResponseHeader("Set-Cookie");
            id_locale = JsonUtility.FromJson<User>(webRequest.downloadHandler.text).id_locale;
        }

        public string GetUserLoginURI()
        {
            return USER_LOGIN_URI;
        }
    }
}
