using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public bool isPaused;
    [SerializeField]
    private GameObject waveTextPrefab;
    [SerializeField]
    private GameObject currentCanvas;

    [SerializeField]
    public static UIManager instance = null;

    void Awake()
    {
        isPaused = false;
        if(currentCanvas == null)
        {
            currentCanvas = GameObject.Find("Canvas");
        }

        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void WaveNumber(int waveNumber, float speed)
    {
        var waveText = Instantiate(waveTextPrefab, currentCanvas.transform);
        waveText.GetComponent<WaveNumberController>().Play(speed, true, waveNumber);
    }
}
