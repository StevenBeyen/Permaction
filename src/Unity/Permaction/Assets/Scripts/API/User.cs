using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace API
{
    [System.Serializable]
    public class User : API
    {
        private const string DEMO_USER_EN = "demouser_en";
        private const string DEMO_USER_FR = "demouser_fr";

        public string username;
        public string password;
        public int id_locale;
        public string cookie;

        public User(string username = null, string password = null, int id_locale = 0, string cookie = null)
        {
            // TODO Remove once login/signup is implemented
            if((username == null) && (password == null))
            {
                if (id_locale == UserData.meta_data.id_locale_mapping["en"])
                {
                    username = DEMO_USER_EN;
                    password = DEMO_USER_EN;
                } else if (id_locale == UserData.meta_data.id_locale_mapping["fr"])
                {
                    username = DEMO_USER_FR;
                    password = DEMO_USER_FR;
                }
            }
            else if((username == null) || (password == null))
            {
                // TODO Change this for a pop-up
                Debug.LogError("Cannot specify username without password.");
            }
            this.username = username;
            this.password = password;      
            this.id_locale = id_locale;
            this.cookie = cookie;
        }

        public void LoginCallback(UnityWebRequest webRequest)
        {
            this.cookie = webRequest.GetResponseHeader("Set-Cookie");
            this.id_locale = JsonUtility.FromJson<User>(webRequest.downloadHandler.text).id_locale;
        }
    }
}
