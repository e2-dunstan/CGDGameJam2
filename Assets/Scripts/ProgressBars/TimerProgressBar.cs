using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

public class TimerProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    [SerializeField] private Text progressText;

    [SerializeField] AnimationCurve pulseCurve;

    float startTime;
    float currentTime;

    int previousTime;

    bool timerDone;

    private void Start()
    {
        SetTimer(120);
    }
    public void SetTimer(float _seconds)
    {
        currentTime = _seconds;
        startTime = currentTime;
        previousTime = 0;

        UpdateProgress();
    }

    private void UpdateProgress()
    {
        progressImage.fillAmount = Mathf.Clamp(currentTime / startTime, 0.0f, 1.0f);
        progressText.text = FormatTime(currentTime);

        if((int)currentTime != previousTime)
        {
            if ((int)currentTime == 60)
            {
                PulseText();
            }
            else if ((int)currentTime == 30)
            {
                PulseText();
            }
            else if ((int)currentTime <= 10)
            {
                PulseText();
            }

            previousTime = (int)currentTime;
        }
    }

    private void PulseText()
    {
        Tween.Cancel(progressText.GetInstanceID());
        Tween.LocalScale(progressText.rectTransform, progressText.transform.localScale * 1.1f, 0.75f, 0, pulseCurve, Tween.LoopType.None);
        Tween.Color(progressText, Color.black, Color.red, 0.75f, 0f, pulseCurve, Tween.LoopType.None);
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

        if (currentTime <= 0 && !timerDone)
        {
            timerDone = true;
        }
        else
        {
            UpdateProgress();
        }
    }
    public bool HasTimerEnded()
    {
        return timerDone;
    }
}
