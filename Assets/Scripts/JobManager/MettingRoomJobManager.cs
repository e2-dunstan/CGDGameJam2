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

    private GameObject JobUIElement = null;
    private GameObject AlertUIElement = null;

    List<GameObject> employeesInRoom = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        AlertUIElement = JobUIManager.Instance.SpawnUIElement(JobUIManager.UIElement.JOB_ALERT, gameObject);
        AlertUIElement.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (jobs.Count < maxNumberOfJobsAtOnce)
        {
            dt += Time.deltaTime;
        }
        else
        {
            dt = 0.0f;
        }

        if (dt > timeBetweenJobs && jobs.Count < maxNumberOfJobsAtOnce && employeesInRoom.Count(x => x.GetComponent<EmployeeJobManager>().hasJob) == 0)
        {
            //Instantiate UI element at gamePos
            //Give UI element this job
            Job tempJob = JobManager.Instance.GetRandomInactiveJobAndAddToQueue();
    
            if (tempJob != null)
            {

                JobUIElement = JobUIManager.Instance.SpawnUIElement(JobUIManager.UIElement.JOB_DESCRIPTION, gameObject);
                JobUIElement.GetComponent<JobOfferBox>().SetUpJobUI(tempJob);
                jobs.Add(tempJob);
                dt = 0.0f;
            }
            else
            {
                Debug.LogError("No More Jobs In JSON File, Ask Ben");
            }

        }

        //Spawn UI relative to someone being in the room
        if(numberOfEmployeesInRoom > 0 && JobUIElement != null)
        {
            JobUIElement.SetActive(true);
            AlertUIElement.SetActive(false);
        }
        else
        {
            if (JobUIElement != null)
            {
                JobUIElement.SetActive(false);
            }
            if (jobs.Count >= 1)
            {
                AlertUIElement.SetActive(true);
            }
        }
    }

    private void RemoveJobFromList(Job _job)
    {
        List<Job> newJobsList = jobs.Where(x => x.taskID != _job.taskID).ToList();
        jobs = newJobsList;
    }

    private void RemoveEmployeeFromList(GameObject _employee)
    {
        List<GameObject> newEmployeeList = employeesInRoom.Where(x => x.gameObject.GetInstanceID() != _employee.gameObject.GetInstanceID()).ToList();
        employeesInRoom = newEmployeeList;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Employee"))
        {
            Debug.Log("Employee entered meeting room");
            numberOfEmployeesInRoom++;

            employeesInRoom.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Employee"))
        {
            numberOfEmployeesInRoom--;

            if (employeesInRoom.Where(x => x.gameObject.GetInstanceID() == other.gameObject.GetInstanceID()).FirstOrDefault() != null)
            {
                RemoveEmployeeFromList(other.gameObject);
            }
        }
    }

    public bool AcceptJobAndAssignToEmployee()
    {
        GameObject employeeWithoutJob = employeesInRoom.Where(x => x.GetComponent<EmployeeJobManager>().hasJob != true).FirstOrDefault();

        if(employeeWithoutJob != null)
        {
            int randomJob = Random.Range(0, jobs.Count - 1);
            employeeWithoutJob.GetComponent<EmployeeJobManager>().SetJob(jobs[randomJob], JobUIManager.UIElement.HAS_TASK);
            RemoveJobFromList(jobs[randomJob]);
            JobUIElement.GetComponent<JobOfferBox>().CloseJobOfferBox();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool DeclineJobAndAssignToEmployee()
    {
        int randomJob = Random.Range(0, jobs.Count - 1);
        JobManager.Instance.CompleteJob(jobs[randomJob].taskID);
        RemoveJobFromList(jobs[randomJob]);
        JobUIElement.GetComponent<JobOfferBox>().CloseJobOfferBox();

        return true;
        //GameObject employeeWithoutJob = employeesInRoom.Where(x => x.GetComponent<EmployeeJobManager>().hasJob != true).FirstOrDefault();

        //if (employeeWithoutJob != null)
        //{
        //    int randomJob = Random.Range(0, jobs.Count - 1);
        //    employeeWithoutJob.GetComponent<EmployeeJobManager>().SetJob(jobs[randomJob], JobUIManager.UIElement.HAS_UNWANTED_TASK);
        //    RemoveJobFromList(jobs[randomJob]);
        //    JobUIElement.GetComponent<JobOfferBox>().CloseJobOfferBox();
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }
}
