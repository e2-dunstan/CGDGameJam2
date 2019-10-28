using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeJobManager : MonoBehaviour
{
    [SerializeField]
    Job job = null;

    public bool hasJob = false;

    public void Awake()
    {
        //hasJobUIElement.SetActive(false);
        //hasCompletedJobUIElement.SetActive(false);
    }
    public void Update()
    {
       //We may wish to manage the removal of jobs from a player here, if a deadline for a task is reached when in a players posession
    }

    public void SetJob(Job _job)
    {
        job = _job;
        hasJob = true;
        JobUIManager.Instance.SpawnUIElement(JobUIManager.UIElement.HAS_TASK, gameObject);
    }

    /// <summary>
    /// Will get the job off the employee, and remove the reference on the player
    /// </summary>
    public Job GetJob()
    {
        hasJob = false;
        Job tempJobPointer = job;
        job = null;
        JobUIManager.Instance.RemoveUIElementFromObject(JobUIManager.UIElement.HAS_TASK, gameObject);
        return tempJobPointer;
    }
}
