using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;

    public void Start()
    {
        isPaused = false;
    }

    public void PausePressed()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }
    public void ContinuePressed()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }
    public void LoadScene(string scene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(scene);
    }
    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("EXIT");
    }

}
