using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeJobManager : MonoBehaviour
{
    [SerializeField]
    Job job = null;

    public bool hasJob = false;

    GameObject hasJobUIElement = null;
    GameObject hasJobToBeDestroyedUIElement = null;


    public void Awake()
    {
        //hasJobUIElement.SetActive(false);
        //hasCompletedJobUIElement.SetActive(false);
    }
    public void Update()
    {
       //We may wish to manage the removal of jobs from a player here, if a deadline for a task is reached when in a players posession
    }

    public void SetJob(Job _job, JobUIManager.UIElement uIElement)
    {
        job = _job;
        hasJob = true;

        
        hasJobUIElement = JobUIManager.Instance.SpawnUIElement(uIElement, gameObject);

    }

    public Job GetJob()
    {
        return job;
    }

    /// <summary>
    /// Will get the job off the employee, and remove the reference on the player
    /// </summary>
    public Job GetJobAndRemoveUIElement()
    {
        hasJob = false;
        Job tempJobPointer = job;
        job = null;
        Destroy(hasJobUIElement);
        return tempJobPointer;
    }
}
