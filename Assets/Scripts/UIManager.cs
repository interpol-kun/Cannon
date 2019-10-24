using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public bool isPaused;

    void Start()
    {
        isPaused = false;
    }

    public void PausePressed()
    {
        isPaused = true;
    }
    public void ContinuePressed()
    {
        isPaused = false;
    }
    public void LoadScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }
    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("EXIT");
    }

}
