using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MettingRoomJobManager : MonoBehaviour
{

    public float timeBetweenJobs = 5.0f;

    public float dt = 0.0f;

    public List<Job> jobs = null;

    public int maxNumberOfJobsAtOnce = 1;

    private int numberOfEmployeesInRoom = 0;

    private GameObject UIElement = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        dt += Time.deltaTime;

        if (dt > timeBetweenJobs && jobs.Count < maxNumberOfJobsAtOnce)
        {
            //Instantiate UI element at gamePos
            //Give UI element this job
            UIElement = JobUIManager.Instance.SpawnUIElement(JobUIManager.UIElement.JOB_DESCRIPTION, gameObject);
            Job tempJob = JobManager.Instance.GetRandomInactiveJobAndAddToQueue();
            UIElement.GetComponent<JobOfferBox>().SetUpJobUI(tempJob);
            jobs.Add(tempJob);
            dt = 0.0f;
        }
    }

    private void RemoveJobFromList(Job _job)
    {
        List<Job> newJobsList = jobs.Where(x => x.taskID != _job.taskID).ToList();
        jobs = newJobsList;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Employee"))
        {
            Debug.Log("Employee entered meeting room");
            numberOfEmployeesInRoom++;

            if (jobs != null)
            {
                int randomJob = Random.Range(0, jobs.Count - 1);
                other.gameObject.GetComponent<EmployeeJobManager>().SetJob(jobs[randomJob]);
                RemoveJobFromList(jobs[randomJob]);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Employee"))
        {
            numberOfEmployeesInRoom--;
        }
    }
}
