using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public void PausePressed()
    {
        Time.timeScale = 0;
    }
    public void ContinuePressed()
    {
        Time.timeScale = 1;
    }
    public void MenuPressed()
    {
        SceneManager.LoadScene("Menu");
    }
    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("EXIT");
    }

}
