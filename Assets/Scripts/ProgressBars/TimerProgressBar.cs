using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    [SerializeField] private Text progressText;

    float startTime;
    float currentTime;

    bool timerDone;

    private void Start()
    {
        SetTimer(120);
    }
    public void SetTimer(float _seconds)
    {
        currentTime = _seconds;
        startTime = currentTime;

        UpdateProgress();
    }

    private void UpdateProgress()
    {
        if (currentTime <= 0 && timerDone)
        {
            timerDone = true;
        }
        else
        {
            progressImage.fillAmount = Mathf.Clamp(currentTime / startTime, 0.0f, 1.0f);
            progressText.text = FormatTime(currentTime);
        }
    }

    private string FormatTime(float _time)
    {
        int minutes = (int)_time / 60;
        int seconds = (int)_time - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private void Update()
    {
        currentTime -= Time.deltaTime;
        UpdateProgress();
    }
    public bool HasTimerEnded()
    {
        return timerDone;
    }
}
