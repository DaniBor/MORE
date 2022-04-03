using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonScript : MonoBehaviour
{
    public void ChangeScene(string scene)
    {
        string test = "";
        if(scene == "Main Menu")
        {
            test = "Game";
        }
        else if(scene == "Game")
        {
            test = "Main Menu";
        }

        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(test);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
    }
}
