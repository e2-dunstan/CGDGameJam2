using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeJobManager : MonoBehaviour
{
    [SerializeField]
    Job job = null;

    public bool hasJob = false;

    public void Update()
    {
       //We may wish to manage the removal of jobs from a player here, if a deadline for a task is reached when in a players posession
    }

    public void SetJob(Job _job)
    {
        job = _job;
        hasJob = true;
    }

    /// <summary>
    /// Will get the job off the employee, and remove the reference on the player
    /// </summary>
    public Job GetJob()
    {
        hasJob = false;
        Job tempJobPointer = job;
        job = null;
        return tempJobPointer;
    }
}
