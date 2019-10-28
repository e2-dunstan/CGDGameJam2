using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    [SerializeField] private Text progressText;

    //To be made private
    [SerializeField] private float taskTime;
    [SerializeField] private float currentTime;
    [SerializeField] private int numOfEmployees;

    private bool taskDone;

    public void SetTaskTime(float _seconds)
    {
        taskTime = _seconds;
        currentTime = _seconds;

        UpdateProgress();
    }

    public void SetNumOfEmployees(int _numOfEmployees)
    {
        numOfEmployees = _numOfEmployees;
    }

    private void UpdateProgress()
    {
        float percentage = 1.0f - (currentTime / taskTime);
        percentage = Mathf.Clamp(percentage, 0.0f, 1.0f);

        if (percentage >= 1)
        {
            taskDone = true;
        }
        
        progressImage.fillAmount = percentage;
        progressText.text = (int)(percentage * 100) + "%";
    }

    public bool IsTaskDone()
    {
        return taskDone;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime * numOfEmployees;
        UpdateProgress();
    }
}
