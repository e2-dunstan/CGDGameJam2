﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

public class TaskProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    [SerializeField] private Text progressText;

    [SerializeField] private Text requiredText;
    [SerializeField] private Text numberOfPeople;
    [SerializeField] private Image personImage;


    //To be made private
    [SerializeField] private float currentTime;

    [SerializeField] float spawnDuration = 1.0f;

    private bool taskDone;

    public bool active;
    public Job job = null;


    private bool isPaused = false;
    private Color startColour;

    public Event taskHasRequirements = Event.NONE;
    public int numberOfPeopleRequired = 0;
    public bool pinkRequired = false;
    public bool blueRequired = false;
    public bool itemRequired = false;
    public Color itemRequiredTextColour;

    private bool hasBeenPlayed = false;
    public void PauseProgress()
    {
        isPaused = true;
        if (!hasBeenPlayed)
        {
            AudioManager.Instance.Play(AudioManager.SoundsType.MISC, (int)AudioManager.MiscSounds.ERROR, 0.1f);
            hasBeenPlayed = true;
        }
    }

    public void UnPauseProgress()
    {
        isPaused = false;
        hasBeenPlayed = false;
    }

    Vector3 startScale = new Vector3(0, 1, 0);
    Vector3 endScale = new Vector3(1, 1, 1);

    private void Awake()
    {
        requiredText.gameObject.SetActive(false);
        numberOfPeople.gameObject.SetActive(false);
        
        startColour = progressImage.color;
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
        progressImage.color = startColour;
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

    private void UpdateStuckProgress()
    {
        requiredText.gameObject.SetActive(true);

        switch(taskHasRequirements)
        {
            case Event.NONE:
                break;
            case Event.REQUIRE_NUMBER_OF_PEOPLE:
                numberOfPeople.gameObject.SetActive(true);
                numberOfPeople.text = numberOfPeopleRequired.ToString() + "x People";
                break;
            case Event.REQUIRE_BLUE_PERSON:
                numberOfPeople.gameObject.SetActive(true);
                numberOfPeople.text = "Blue Hair";
                break;
            case Event.REQUIRE_PINK_PERSON:
                numberOfPeople.gameObject.SetActive(true);
                numberOfPeople.text = "Pink Hair";
                break;
            case Event.REQUIRE_ITEM:
                numberOfPeople.gameObject.SetActive(true);
                numberOfPeople.color = itemRequiredTextColour;
                //Debug.Log(itemRequiredTextColour);
                numberOfPeople.text = "Item";
                break;
            default:
                break;
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
            else
            {
                UpdateStuckProgress();
                progressText.text = "Task Is Stuck";
                progressImage.color = Color.red;
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

    public void ResetProgressBarEvent()
    {
        numberOfPeople.color = Color.white;
        requiredText.gameObject.SetActive(false);
        numberOfPeople.text = "";
        numberOfPeople.gameObject.SetActive(false);
        taskHasRequirements = Event.NONE;
        numberOfPeopleRequired = 0;
        blueRequired = false;
        pinkRequired = false;
        itemRequired = false;
        hasBeenPlayed = false;
    }
}
