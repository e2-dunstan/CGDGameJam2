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

    [SerializeField] Color startColour;
    [SerializeField] Color endColour;

    float startTime;
    float currentTime;

    int previousTime;

    public void UpdateTimer(float _startTime, float _currentTime)
    {
        float timerPercentage = Mathf.Clamp(_currentTime / _startTime, 0.0f, 1.0f);
        progressImage.fillAmount = timerPercentage;
        progressImage.color = Color.Lerp(startColour, endColour, 1.0f - timerPercentage);
        progressText.text = FormatTime(_currentTime);

        if((int)_currentTime != previousTime)
        {
            if ((int)_currentTime == 60)
            {
                PulseText();
            }
            else if ((int)_currentTime == 30)
            {
                PulseText();
            }
            else if ((int)_currentTime <= 10)
            {
                PulseText();
            }

            previousTime = (int)_currentTime;
        }
    }

    private void PulseText()
    {
        Vector3 startScale = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 endScale = new Vector3(1.1f, 1.1f, 1.1f);

        Tween.Cancel(progressText.GetInstanceID());
        Tween.LocalScale(progressText.transform, startScale, endScale, 0.75f, 0, pulseCurve, Tween.LoopType.None);
        Tween.Color(progressText, Color.black, Color.red, 0.75f, 0f, pulseCurve, Tween.LoopType.None);
    }

    private string FormatTime(float _time)
    {
        int minutes = (int)_time / 60;
        int seconds = (int)_time - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
