using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class WaveNumberController : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve popUp;

    private float speed = 1;
    private bool play;
    private float localTime;

    private TMP_Text text;
    // Start is called before the first frame update
    void Awake()
    {
        play = false;
        localTime = 0.001f;
        //Make sure there will be no 1-frame flickering on the start of each wave
        transform.localScale = Vector2.zero;

        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            var scale = popUp.Evaluate(localTime / speed);
            transform.localScale = new Vector2(scale, scale);
            localTime += Time.deltaTime;
        }
    }

    public void Play(float animTime, bool isPlaying, int waveNumber)
    {
        speed = animTime;
        play = isPlaying;
        text.text = "WAVE " + waveNumber;
        Destroy(gameObject, animTime);
    }
}
