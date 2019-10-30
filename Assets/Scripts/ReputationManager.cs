using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    public static ReputationManager Instance;

    //Depending on your current Star rating set employees active or inactive
    //Rating will also be used in job manager to throw out harder to complete jobs
    [Header("General Settings")]
    [SerializeField] private int minActiveEmployees = 3;
    [SerializeField] private int maxActiveEmployees = 10;
    [SerializeField] private int minStarRating = 1;
    [SerializeField] private int maxStartRating = 3;
    [SerializeField] private Employee[] employeeArray;

    private int currentStarRating;
    public int CurrentRating { get => currentStarRating; private set => currentStarRating = value; }

    private int currentScore = 0;
    public int CurrentScore { get => currentScore; private set => currentScore = value; }

    private int totalTasksCompleted = 0;
    public int TotalTasksCompleted { get => totalTasksCompleted; private set => totalTasksCompleted = value; }

    //Checks tasks by the task frequency sees how many were completed in that time frame and adjust rep accordingly
    [Header("Reputation Settings")]
    [SerializeField] private float taskCheckFrequency = 30.0f;
    [SerializeField] private float taskToStarMultiplier = 1.0f;

    private int completedTasksInTimeframe = 0;
    private int currentActiveEmployees;
    private float taskCheckTimer = 0.0f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    void Start()
    {
        taskCheckTimer = taskCheckFrequency;
        currentActiveEmployees = minActiveEmployees;
        currentStarRating = minStarRating;
        InitialiseEmployees();
    }

    void Update()
    {
        if(taskCheckTimer > 0)
        {
            taskCheckTimer -= Time.deltaTime;

            if(taskCheckTimer < 0)
            {
                AdjustReputation();
                UpdateActiveEmployees();
            }
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

    private void AdjustReputation()
    {
        taskCheckTimer = taskCheckFrequency;

        for (int i = maxStartRating; i > minStarRating; i--)
        {
            if(completedTasksInTimeframe  >= i * taskToStarMultiplier)
            {
                currentStarRating = i;
                completedTasksInTimeframe = 0;
                return;
            }
        }
        currentStarRating = minStarRating;
        completedTasksInTimeframe = 0;
    }

    private void UpdateActiveEmployees()
    {
        float lerp = Mathf.InverseLerp(minStarRating, maxStartRating, currentStarRating);
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

    public void JobCompleted(int baseScoreForJob, float recommendedTimeToComplete, float actualTimeToComplete)
    {
        //Looks at tasks if they took 100% longer or 100% quicker then scale between 0.2 and 2.0x multipliers
        totalTasksCompleted++;
        completedTasksInTimeframe++;
        float multiplier = actualTimeToComplete > recommendedTimeToComplete ?
            1 - Mathf.InverseLerp(recommendedTimeToComplete, (recommendedTimeToComplete * 2.0f), actualTimeToComplete) :
            1 + Mathf.InverseLerp(0.0f, recommendedTimeToComplete, actualTimeToComplete);

        multiplier = multiplier < 0.2f ? 0.2f : multiplier;
        currentScore += Mathf.FloorToInt(baseScoreForJob * multiplier);
    }

    public void Reset()
    {
        currentStarRating = minStarRating;
        currentScore = 0;
        totalTasksCompleted = 0;
        currentActiveEmployees = minActiveEmployees;
    }
}
