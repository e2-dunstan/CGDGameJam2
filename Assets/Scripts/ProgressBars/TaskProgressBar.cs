using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

public class TaskProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    [SerializeField] private Text progressText;

    //To be made private
    [SerializeField] private float currentTime;

    [SerializeField] float spawnDuration = 1.0f;

    private bool taskDone;

    public bool active;
    public Job job = null;


    private bool isPaused = false;

    public void PauseProgress()
    {
        isPaused = true;
    }

    public void UnPauseProgress()
    {
        isPaused = false;
    }

    Vector3 startScale = new Vector3(0, 1, 0);
    Vector3 endScale = new Vector3(1, 1, 1);

    private void Awake()
    {
        startScale = new Vector3(0, this.transform.localScale.y, 1);
        endScale = this.transform.localScale;
        active = true;
    }

    private void OnEnable()
    {
        float delay = 0.0f;
        Tween.LocalScale(this.transform, startScale, endScale, spawnDuration, delay, Tween.EaseInOut, Tween.LoopType.None);
        progressImage.color = Color.yellow;
    }

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

        progressImage.fillAmount = percentage;
        progressText.text = (int)(percentage * 100) + "%";

        if (percentage >= 1)
        {
            job.isTaskCompleted = true;
            progressText.text = "Done";
            progressImage.color = Color.green;
        }
    }

    public bool IsTaskDone()
    {
        return job.isTaskCompleted;
    }

    private void Update()
    {
        if (job != null)
        {
            job.completionTime += Time.deltaTime;

            if (!isPaused)
            {
                currentTime += (Time.deltaTime / job.recommendedUnitCount) * (job.currentPlayersAssigned);
                UpdateProgress();
            }
        }
    }

    public void CloseProgressBar()
    {
        active = false;
        float delay = 0.0f;
        Tween.LocalScale(this.transform, endScale, startScale, 0.75f, delay, Tween.EaseInOut, Tween.LoopType.None, null, DestroyBar);
    }

    private void DestroyBar()
    {
        Destroy(this.gameObject);
    }
}
