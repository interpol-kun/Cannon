using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void PausePressed()
    {
        Time.timeScale = 0f;
    }
    public void ContinuePressed()
    {
        Time.timeScale = 1f;
    }
    public void MenuPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("EXIT");
    }

}
