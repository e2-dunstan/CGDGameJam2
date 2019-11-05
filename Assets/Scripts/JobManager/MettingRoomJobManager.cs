using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MettingRoomJobManager : MonoBehaviour
{
    public float timeBetweenJobs = 5.0f;

    public float easyJobSpawnTimer = 5.0f;
    public float mediumJobSpawnTimer = 3.0f;
    public float hardJobSpawnTimer = 2.0f;

    private float dt = 10.0f;

    public List<Job> jobs = null;

    public int maxNumberOfJobsAtOnce = 1;

    private int numberOfEmployeesInRoom = 0;

    private GameObject JobUIElement = null;
    private GameObject AlertUIElement = null;

    public static AudioManager audio;

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
        int numberOfEmployeesInRoomWithJob = employeesInRoom.Count(x => x.GetComponent<EmployeeJobManager>().hasJob);
        int numberOfActiveEmployeesInRoom = employeesInRoom.Count(x => x.activeSelf);

        if (jobs.Count < maxNumberOfJobsAtOnce)
        {
            dt += Time.deltaTime;
        }
        else
        {
            dt = 0.0f;
        }

        //Spawn Jobs
        if (dt > timeBetweenJobs && jobs.Count < maxNumberOfJobsAtOnce)
        {
            //Instantiate UI element at gamePos
            //Give UI element this job
            Job tempJob = null;

            switch (JobManager.Instance.currentGameDifficulty)
            {
                case JobManager.CurrentGameDifficulty.SUPER_EASY:
                    tempJob = JobManager.Instance.GetRandomInactiveJobAndAddToQueue(Difficulty.EASY);
                    timeBetweenJobs = easyJobSpawnTimer;
                    break;
                case JobManager.CurrentGameDifficulty.EASY:
                    tempJob = JobManager.Instance.GetRandomInactiveJobAndAddToQueue(Difficulty.EASY);
                    timeBetweenJobs = easyJobSpawnTimer;
                    break;
                case JobManager.CurrentGameDifficulty.MEDIUM:
                    tempJob = JobManager.Instance.GetRandomInactiveJobAndAddToQueue(Difficulty.MEDIUM);
                    timeBetweenJobs = mediumJobSpawnTimer;
                    break;
                case JobManager.CurrentGameDifficulty.HARD:
                    tempJob = JobManager.Instance.GetRandomInactiveJobAndAddToQueue(Difficulty.HARD);
                    timeBetweenJobs = hardJobSpawnTimer;
                    break;
                default:
                    Debug.Log("Job difficulty isn't set properly");
                    break;
            }
            if (tempJob != null)
            {
                JobUIElement = JobUIManager.Instance.SpawnUIElement(JobUIManager.UIElement.JOB_DESCRIPTION, gameObject);
                JobUIElement.GetComponent<JobOfferBox>().SetUpJobUI(tempJob);
                jobs.Add(tempJob);
                AudioManager.Instance.Play(AudioManager.SoundsType.TASK, (int)AudioManager.TaskSounds.CREATED, 0.1f);
                dt = 0.0f;
            }
            else
            {
                Debug.LogError("No More Jobs In JSON File, Ask Ben");
            }

        }

        //Spawn UI relative to someone being in the room
        if(numberOfEmployeesInRoom > 0 && JobUIElement != null && numberOfEmployeesInRoomWithJob == 0 && numberOfActiveEmployeesInRoom > 0)
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

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Employee"))
    //    {
    //        employeesInRoom = new List<GameObject>();
    //        employeesInRoom.Add(other.gameObject);

    //        employeesInRoom.Count();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Employee"))
        {
            Debug.Log("Employee entered meeting room");
            numberOfEmployeesInRoom++;

            employeesInRoom.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Employee"))
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
        GameObject employeeWithoutJob = employeesInRoom.Where(x => x.GetComponent<EmployeeJobManager>().hasJob != true && x.activeSelf).FirstOrDefault();

        if(employeeWithoutJob != null)
        {
            int randomJob = Random.Range(0, jobs.Count - 1);
            employeeWithoutJob.GetComponent<EmployeeJobManager>().SetJob(jobs[randomJob], JobUIManager.UIElement.HAS_TASK);
            RemoveJobFromList(jobs[randomJob]);
            JobUIElement.GetComponent<JobOfferBox>().CloseJobOfferBox();
            AudioManager.Instance.Play(AudioManager.SoundsType.TASK, (int)AudioManager.TaskSounds.ACCEPTED, 1.0f);
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
        AudioManager.Instance.Play(AudioManager.SoundsType.TASK, (int)AudioManager.TaskSounds.REJECT, 1.0f);
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
