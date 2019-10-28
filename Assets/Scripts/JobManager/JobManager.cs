using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JobManager : MonoBehaviour
{
    public enum  CurrentGameDifficulty
    {
        SUPER_EASY = 0,
        EASY = 1,
        MEDIUM = 2,
        HARD = 3
    }

    public static JobManager Instance;

    [SerializeField]
    private JobLoadManager jobLoadManager;

    private bool jobsLoaded = false;
    public Jobs jobs { get; set; }

    public List<Job> ActiveJobList = new List<Job>();


    //Difficulty related variables
    public int jobsCompletedInPeriod = 0;
    public CurrentGameDifficulty currentGameDifficulty = CurrentGameDifficulty.EASY;

    [SerializeField]
    private float difficultyDeltaTime = 0.0f;
    private float timeBetweenRemovingJob = 30.0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        if (jobLoadManager != null)
        {
            jobsLoaded = jobLoadManager.LoadData();

            if (jobsLoaded == true)
            {
                jobs = jobLoadManager.GetJobs();
            }
        }
        else
        {
            Debug.Log("No Load Manager Assigned To JobManager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jobsLoaded)
        {
            UpdateActiveJobsTimer(Time.deltaTime);
            UpdateCurrentDifficulty(Time.deltaTime);
        }

    }
    /// <summary>
    /// Since we wish for difficulty to be adaptive, change difficulty up depending upon current effectiveness in time period
    /// </summary>
    private void UpdateCurrentDifficulty(float _dt)
    {
        difficultyDeltaTime += _dt;

        if(difficultyDeltaTime > timeBetweenRemovingJob)
        {
            jobsCompletedInPeriod--;
        }

        if(jobsCompletedInPeriod < 2)
        {
            currentGameDifficulty = CurrentGameDifficulty.SUPER_EASY;
        }
        else if(jobsCompletedInPeriod < 4)
        {
            currentGameDifficulty = CurrentGameDifficulty.EASY;
        }
        else if(jobsCompletedInPeriod < 6)
        {
            currentGameDifficulty = CurrentGameDifficulty.MEDIUM;
        }
        else if(jobsCompletedInPeriod < 8)
        {
            currentGameDifficulty = CurrentGameDifficulty.HARD;
        }
    }

    private void UpdateActiveJobsTimer(float _dt)
    {
        if (GetNumberOfJobs() != 0)
        {
            foreach (var job in ActiveJobList)
            {
               job.currentActiveTime += _dt;

                //if (job.currentActiveTime > job.timeUntilDeque)
                //{
                //    job.isTaskActive = false;
                //    job.isInQueue = false;

                //    ActiveJobList = ActiveJobList.Where(x => x.taskID != job.taskID).ToList();
                //}
            }
        }
    }

    /// <summary>
    /// Get a job by ID
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public Job GetJobById(string _id)
    {
        return jobs.jobList.Where(x => x.taskID == _id).FirstOrDefault();
    }

    //=================Helper Functions===============
    /// <summary>
    /// Gets the number of Jobs loaded in from the JSON file
    /// </summary>
    public int GetNumberOfJobs()
    {
        return jobs.jobList.Count;
    }

    /// <summary>
    /// Gets a random inactive job, and adds it to the queue.
    /// </summary>
    public Job GetRandomInactiveJobAndAddToQueue()
    {
        List<Job> InactiveJobList = jobs.jobList.Where(x => !x.isTaskActive).ToList();

        int randomIndex = Random.Range(0, InactiveJobList.Count - 1);

        InactiveJobList[randomIndex].isTaskActive = true;

        ActiveJobList.Add(InactiveJobList[randomIndex]);

        return InactiveJobList[randomIndex];
    }

    /// <summary>
    /// Gets a current inactive job and returns it by reference. Include a difficulty as a parameter if a certain difficulty is required.
    /// </summary>
    public Job GetRandomInactiveJobAndAddToQueue(Difficulty _difficulty)
    {
        List<Job> InactiveJobList = jobs.jobList.Where(x => !x.isTaskActive && x.taskDifficulty == _difficulty).ToList();

        int randomIndex = Random.Range(0, InactiveJobList.Count - 1);

        InactiveJobList[randomIndex].isTaskActive = true;

        ActiveJobList.Add(InactiveJobList[randomIndex]);

        return ActiveJobList[randomIndex];
    }

    public List<Job> GetActiveJobs()
    {
        ActiveJobList = jobs.jobList.Where(x => x.isTaskActive).ToList();

        return ActiveJobList;
    }
}
