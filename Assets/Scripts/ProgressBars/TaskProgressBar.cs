using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    [SerializeField] private Text progressText;

    //To be made private
    [SerializeField] private float currentTime;

    private bool taskDone;

    public Job job = null;

   
    public void SetNumOfEmployees(int _numOfEmployees)
    {
        job.currentPlayersAssigned = _numOfEmployees;
    }

    public void SetJob(Job _job)
    {
        job = _job;
    }

    private void UpdateProgress()
    {
        float percentage = (currentTime / job.taskTime);
        percentage = Mathf.Clamp(percentage, 0.0f, 1.0f);

        if (percentage >= 1)
        {
            job.isTaskCompleted = true;
        }
        
        progressImage.fillAmount = percentage;
        progressText.text = (int)(percentage * 100) + "%";
    }

    public bool IsTaskDone()
    {
        return job.isTaskCompleted;
    }

    private void Update()
    {
        if (job != null)
        {
            currentTime += Time.deltaTime * job.currentPlayersAssigned;
            UpdateProgress();
        }
    }
}
