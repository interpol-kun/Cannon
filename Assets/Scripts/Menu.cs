using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartPressed()
    {
        SceneManager.LoadScene("Main");
    }
    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("EXIT");
    }

}
