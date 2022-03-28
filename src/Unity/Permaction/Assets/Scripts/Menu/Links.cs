using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Links : MonoBehaviour
{
    public void openFreepikLink()
    {
        Application.OpenURL("https://www.flaticon.com/authors/freepik");
    }

    public void openGitHubLink()
    {
        Application.OpenURL("https://github.com/StevenBeyen/Permaction");
    }

    public void openPatreonLink()
    {
        Application.OpenURL("https://patreon.com/Permaction");
    }

    public void openUpklyakLink()
    {
        Application.OpenURL("https://www.freepik.com/upklyak");
    }

    public void openOpinionFormEN()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLScL0OfsDrY0ph1HrDwMW_UEmHWJ1ddY58DGbYuPzQuV3rY86Q/viewform?usp=pp_url");
    }

    public void openOpinionFormFR()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLScvswBLTh5RJqJ90m0HJJk4uSzP9lpSU1eFQpPMDdbRJVGyaw/viewform?usp=pp_url");
    }
}
