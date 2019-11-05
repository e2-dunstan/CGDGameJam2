using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    public static ReputationManager Instance;

    [Header("Script References")]
    [SerializeField] private ReputationStarsUI repUi;

    //Depending on your current Star rating set employees active or inactive
    //Rating will also be used in job manager to throw out harder to complete jobs
    [Header("General Settings")]
    [SerializeField] private int minActiveEmployees = 4;
    [SerializeField] private int maxActiveEmployees = 10;
    [SerializeField] private int minStarRating = 1;
    [SerializeField] private int maxStarRating = 3;
    [SerializeField] private Employee[] employeeArray;

    private int currentStarRating;
    public int CurrentRating { get => currentStarRating; private set => currentStarRating = value; }

    private int currentScore = 0;
    public int CurrentScore { get => currentScore; private set => currentScore = value; }

    private int totalTasksCompleted = 0;
    public int TotalTasksCompleted { get => totalTasksCompleted; private set => totalTasksCompleted = value; }

    private bool reputationDecay = true;
    public bool ReputationDecay { get => reputationDecay; set => reputationDecay = value; }

    //Checks tasks by the task frequency sees how many were completed in that time frame and adjust rep accordingly
    [Header("Reputation Settings")]
    [SerializeField] private float reputationDecayMultiplier = 0.25f;
    [SerializeField] private float taskCompletionBase = 1.0f;
    [SerializeField] private float easyTaskMultiplier = 1.0f;
    [SerializeField] private float mediumTaskMultiplier = 1.25f;
    [SerializeField] private float hardTaskMultiplier = 1.75f;

    private int currentActiveEmployees;
    private float currentStarRatingFloat;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    void Start()
    {
        currentActiveEmployees = minActiveEmployees;
        currentStarRating = minStarRating;
        currentStarRatingFloat = minStarRating;
        InitialiseEmployees();
    }

    void Update()
    {
        //Decrease current reputation every update
        if (reputationDecay)
        {
            currentStarRatingFloat -= Time.deltaTime * reputationDecayMultiplier;
            currentStarRatingFloat = currentStarRatingFloat < 0 ? 0 : currentStarRatingFloat;

            if (currentStarRatingFloat < currentStarRating)
            {
                UpdateReputation();
            }
        }

        PlayEmplyeeSounds();
    }

    private void PlayEmplyeeSounds()
    {
        bool chatting = false;
        bool walking = false;
        bool working = false;
        for (int i = 0; i < employeeArray.Length; ++i)
        {
            if (employeeArray[i].GetState() == Employee.State.RELAXING ||
                employeeArray[i].GetState() == Employee.State.IDLE)
            {
                if (!AudioManager.Instance.IsSoundPlaying((int)AudioManager.LoopSounds.CHATTER))
                {
                    AudioManager.Instance.Play(AudioManager.SoundsType.LOOPING, (int)AudioManager.LoopSounds.CHATTER);
                    AudioManager.Instance.FadeIn((int)AudioManager.LoopSounds.CHATTER, 0.5f, 0.5f);
                }
                chatting = true;
            }
            else if (employeeArray[i].GetMoveSpeed() > 0.0f)
            {
                if (!AudioManager.Instance.IsSoundPlaying((int)AudioManager.LoopSounds.FOOTSTEPS))
                {
                    AudioManager.Instance.Resume((int)AudioManager.LoopSounds.FOOTSTEPS);
                   //AudioManager.Instance.FadePlay((int)AudioManager.LoopSounds.FOOTSTEPS, 0.2f, 0.2f);
                }
                walking = true;
            }
            else if (employeeArray[i].GetState() == Employee.State.WORKING)
            {
                if (!AudioManager.Instance.IsSoundPlaying((int)AudioManager.LoopSounds.TYPING))
                {
                    AudioManager.Instance.Play(AudioManager.SoundsType.LOOPING, (int)AudioManager.LoopSounds.TYPING);
                    AudioManager.Instance.FadeIn((int)AudioManager.LoopSounds.TYPING, 0.2f, 0.5f);
                }
                working = true;
            }

            //if (i == employeeArray.Length - 1)
            //{
            //}
        }

        if (!chatting && AudioManager.Instance.IsSoundPlaying((int)AudioManager.LoopSounds.CHATTER))
        {
            //AudioManager.Instance.Stop((int)AudioManager.LoopSounds.CHATTER);
            AudioManager.Instance.FadeOut((int)AudioManager.LoopSounds.CHATTER, 0.5f);
            AudioManager.Instance.SetFade((int)AudioManager.LoopSounds.CHATTER, false);
        }
        if (!walking && AudioManager.Instance.IsSoundPlaying((int)AudioManager.LoopSounds.FOOTSTEPS))
        {
            AudioManager.Instance.Pause((int)AudioManager.LoopSounds.FOOTSTEPS);
            // AudioManager.Instance.FadeStop((int)AudioManager.LoopSounds.FOOTSTEPS, 0.8f);
        }
        if (!working && AudioManager.Instance.IsSoundPlaying((int)AudioManager.LoopSounds.TYPING))
        {
            AudioManager.Instance.FadeOut((int)AudioManager.LoopSounds.TYPING, 0.5f);
            AudioManager.Instance.SetFade((int)AudioManager.LoopSounds.TYPING, false);
        }
    }

    private void InitialiseEmployees()
    {
        int counter = minActiveEmployees;
        foreach (var employee in employeeArray)
        {
            if (!employee.gameObject.activeSelf)
            {
                employee.gameObject.SetActive(true);
                counter--;
                if (counter == 0) { return; }
            }
        }
    }

    private void UpdateReputation()
    {
        //This will get called when either a projects score has been added to the reputation or if its been removed
        currentStarRating = Mathf.FloorToInt(currentStarRatingFloat) > maxStarRating ? maxStarRating : Mathf.FloorToInt(currentStarRatingFloat);
        UpdateActiveEmployees();

        if (repUi != null)
        {
            repUi.SetReputation(currentStarRating);
        }
    }

    private void UpdateActiveEmployees()
    {
        float lerp = Mathf.InverseLerp(minStarRating, maxStarRating, currentStarRating);
        int newEmployeeAmount = Mathf.FloorToInt(Mathf.Lerp(minActiveEmployees, maxActiveEmployees, lerp));

        int currentActiveCount = 0;
        //Get current amount of employees active
        for (int i = 0; i < employeeArray.Length; i++)
        {
            if (employeeArray[i].gameObject.activeSelf)
            {
                currentActiveCount++;
            }
        }

        //If we need to add or remove employees loop over the employees in the local array add / remove them then break out once the required value has been reached
        if(currentActiveCount == newEmployeeAmount)
        {
            currentActiveEmployees = currentActiveCount;
            return;
        }
        else if(currentActiveCount < newEmployeeAmount)
        {
            foreach (var employee in employeeArray)
            { 
                if (!employee.gameObject.activeSelf)
                {
                    employee.gameObject.SetActive(true);
                    currentActiveCount++;

                    if(currentActiveCount == newEmployeeAmount)
                    {
                        currentActiveEmployees = currentActiveCount;
                        return;
                    }
                }     
            }
        }
        else if(currentActiveCount > newEmployeeAmount)
        {
            foreach (var employee in employeeArray)
            {
                if (employee.gameObject.activeSelf && !employee.GetComponent<EmployeeJobManager>().hasJob)
                {
                    employee.gameObject.SetActive(false);
                    currentActiveCount--;

                    if (currentActiveCount == newEmployeeAmount)
                    {
                        currentActiveEmployees = currentActiveCount;
                        return;
                    }
                }
            }
        }
    }

    public void JobCompleted(int baseScoreForJob, float recommendedTimeToComplete, float actualTimeToComplete, Difficulty jobDifficulty)
    {
        totalTasksCompleted++;

        //Add to score based on the diffculty of the job
        float multiplier = actualTimeToComplete > recommendedTimeToComplete ?
            1 - Mathf.InverseLerp(recommendedTimeToComplete, (recommendedTimeToComplete * 2.0f), actualTimeToComplete) :
            1 + Mathf.InverseLerp(0.0f, recommendedTimeToComplete, actualTimeToComplete);

        multiplier = multiplier < 0.2f ? 0.2f : multiplier;
        currentScore += Mathf.FloorToInt(baseScoreForJob * multiplier);

        //Add to the reputation based on the difficult of the job and update the current reputation
        switch (jobDifficulty)
        {
            case Difficulty.EASY:
                currentStarRatingFloat += taskCompletionBase * easyTaskMultiplier;
                break;
            case Difficulty.MEDIUM:
                currentStarRatingFloat += taskCompletionBase * mediumTaskMultiplier;
                break;
            case Difficulty.HARD:
                currentStarRatingFloat += taskCompletionBase * hardTaskMultiplier;
                break;
        }
        UpdateReputation();
    }

    public void Reset()
    {
        currentStarRating = minStarRating;
        currentScore = 0;
        totalTasksCompleted = 0;
        currentActiveEmployees = minActiveEmployees;
        currentStarRatingFloat = minStarRating;
    }
}
