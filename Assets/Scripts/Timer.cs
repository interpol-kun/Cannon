using UnityEngine;

public class Timer : MonoBehaviour
{
    public delegate void OnTimerEvent(float extraTime);
    public delegate void OnTimerEventGet(Timer timer);
    public static event OnTimerEvent onTimerExpired;
    public static event OnTimerEventGet onTimerSet;

    public float timer;
    private bool isSet;


    private void Awake()
    {
        isSet = false;
    }
    void Update()
    {
        if (isSet)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                onTimerExpired?.Invoke(0f);
                isSet = false;
            }
        }
    }

    public void SetTimer(float time)
    {
        this.timer = time;
        isSet = true;
        onTimerSet?.Invoke(this);
    }

    public Timer GetTimer()
    {
        return this;
    }
    public bool TimerIsSet()
    {
        return isSet;
    }

    //Stop timer and return time
    public void Stop()
    {
        float extraTime = timer;
        timer = 0f;
        isSet = false;
        onTimerExpired?.Invoke(extraTime);
    }
}
