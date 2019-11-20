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

    [SerializeField]
    private MettingRoomJobManager mettingRoomJob;

    [SerializeField]
    private PresentationRoomManager presentationRoom;

    private bool jobsLoaded = false;
    public Jobs jobs { get; set; }

    public List<Job> ActiveJobList = new List<Job>();


    //Difficulty related variables
    public int jobsCompletedInPeriod = 0;
    public CurrentGameDifficulty currentGameDifficulty = CurrentGameDifficulty.EASY;

    [SerializeField]
    private float difficultyDeltaTime = 0.0f;
    [SerializeField]
    private float timeBetweenRemoving = 30.0f;


    //Event timer
    public float timeBetweenChaos = 20.0f;
    private float chaosDt = 0.0f;

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

        if(mettingRoomJob == null)
        {
            Debug.Log("No MeetingRoomManager On JobManager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jobsLoaded)
        {
            UpdateActiveJobsTimer(Time.deltaTime);
            UpdateCurrentDifficulty(Time.deltaTime);
            CauseMeSomeChaosPls(Time.deltaTime);
        }

    }
    /// <summary>
    /// Since we wish for difficulty to be adaptive, change difficulty up depending upon current effectiveness in time period
    /// </summary>
    private void UpdateCurrentDifficulty(float _dt)
    {
        if (jobsCompletedInPeriod > 0)
        {
            difficultyDeltaTime += _dt;

            if (difficultyDeltaTime > timeBetweenRemoving)
            {
                jobsCompletedInPeriod--;
                difficultyDeltaTime = 0.0f;
            }
        }

        switch (ReputationManager.Instance.CurrentRating)
        {
            case 0:
                currentGameDifficulty = CurrentGameDifficulty.EASY;
                break;
            case 1:
                currentGameDifficulty = CurrentGameDifficulty.EASY;
                break;
            case 2:
                currentGameDifficulty = CurrentGameDifficulty.MEDIUM;
                break;
            case 3:
                currentGameDifficulty = CurrentGameDifficulty.HARD;
                break;
            default:
                Debug.Log("Failed");
                break;
        }
    }

    //Function will cause an event to happen to all active jobs, job won't continue until this condition is met
    private void CauseMeSomeChaosPls(float _dt)
    {

        chaosDt += _dt;

        if (chaosDt > timeBetweenChaos)
        {
            foreach (var job in ActiveJobList)
            {
                if (job.isTaskActive && job.isTaskCompleted != true)
                {
                    if (job.eventList.genericEventList.Count < 1)
                    {
                        int randomEvent = Random.Range(0, 4);
                        //int randomEvent = 3;

                        switch (randomEvent)
                        {
                            case 0:
                                {
                                    int randomNumber = Random.Range(2, 3);
                                    Job.GenericInt genericInt = new Job.GenericInt(randomNumber);
                                    job.eventList.Add<Job.GenericInt>(Event.REQUIRE_NUMBER_OF_PEOPLE, genericInt, Color.red);
                                    Debug.Log("error1");
                                    break;
                                }
                            case 1:
                                { 
                                    int randomNumber = Random.Range(1, 2);
                                    Job.GenericInt genericInt = new Job.GenericInt(randomNumber);
                                    job.eventList.Add<Job.GenericInt>(Event.REQUIRE_BLUE_PERSON, genericInt, Color.red);
                                    break;
                                }
                            case 2:
                                {
                                    int randomNumber = Random.Range(1, 2);
                                    Job.GenericInt genericInt = new Job.GenericInt(randomNumber);
                                    job.eventList.Add<Job.GenericInt>(Event.REQUIRE_PINK_PERSON, genericInt, Color.red);
                                    Debug.Log("error3");
                                    break;
                                }
                            case 3:
                                {
                                    GameObject tempObj = ItemManager.Instance.GetRandomItem();
                                    Job.GenericGameObject genericObj = new Job.GenericGameObject(tempObj);
                                    Color randomColour = ItemManager.Instance.GetColor();
                                    Debug.Log(randomColour);
                                    job.eventList.Add<Job.GenericGameObject>(Event.REQUIRE_ITEM, genericObj, randomColour);
                                    tempObj.GetComponent<CollectableItem>().SetParticleColour(randomColour);
                                    Debug.Log("error4");
                                    break;
                                }
                        }
                    }
                }
            }

            chaosDt = 0.0f;
        }
    }

    private void UpdateActiveJobsTimer(float _dt)
    {
        if (GetNumberOfJobs() != 0)
        {
            foreach (var job in ActiveJobList)
            {
                if (job.isTaskActive)
                {
                    job.currentActiveTime += _dt;
                }

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
        List<Job> InactiveJobList = jobs.jobList.Where(x => !x.isInQueue).ToList();

        if (InactiveJobList.Count > 0)
        {
            int randomIndex = Random.Range(0, InactiveJobList.Count - 1);

            InactiveJobList[randomIndex].isInQueue = true;

            ActiveJobList.Add(InactiveJobList[randomIndex]);

            return InactiveJobList[randomIndex];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Gets a current inactive job and returns it by reference. Include a difficulty as a parameter if a certain difficulty is required.
    /// </summary>
    public Job GetRandomInactiveJobAndAddToQueue(Difficulty _difficulty)
    {
        List<Job> InactiveJobList = jobs.jobList.Where(x => !x.isInQueue && x.taskDifficulty == _difficulty).ToList();

        if (InactiveJobList.Count > 0)
        {
            int randomIndex = Random.Range(0, InactiveJobList.Count - 1);

            InactiveJobList[randomIndex].isInQueue = true;

            ActiveJobList.Add(InactiveJobList[randomIndex]);

            return InactiveJobList[randomIndex];
        }
        else
        {
            return null;
        }
    }

    public List<Job> GetActiveJobs()
    {
        ActiveJobList = jobs.jobList.Where(x => x.isTaskActive).ToList();

        return ActiveJobList;
    }

    public void CompleteJob(string _jobID)
    {
        Job jobToBeRemoved = ActiveJobList.Where(x => x.taskID == _jobID).FirstOrDefault();

        List<Job> tempJobList = ActiveJobList.Where(x => x.taskID != _jobID).ToList();

        ActiveJobList = tempJobList;

        jobToBeRemoved.ResetJob();
        jobsCompletedInPeriod++;
    }

    public void AcceptJobAndAssignToEmployee()
    {
        mettingRoomJob.AcceptJobAndAssignToEmployee();
    }

    public void DeclineJobAndAssignToEmployee()
    {
        mettingRoomJob.DeclineJobAndAssignToEmployee();
    }


    public void AlertJobHasBeenCompleted()
    {
        presentationRoom.AlertJobHasBeenCompleted();
    }
}

