using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public enum Difficulty
{
    EASY = 0,
    MEDIUM = 1,
    HARD = 2
}

[System.Serializable]
public class Job
{
    //Job Values Pre Obtaining in Game
    public string taskIconLocation;
    public Sprite taskIcon;
    public string taskName;
    public string taskID;
    public string taskDescription;
    public float taskTime;
    public Difficulty taskDifficulty;
    public int recommendedUnitCount;
    public bool isInQueue;
    public float baseTaskScore;
    public float timeUntilDeque;

    //Job Values Post Obtaining in Game
    public bool isTaskActive = false;
    public float currentActiveTime;
    public float completionTime = 0.0f;
    public int currentPlayersAssigned;
    //Amount by which task completion speed is effected for each extra person
    public int playerSpeedMultiplier;
    public int timeUntilFailure;
    public bool isTaskCompleted = false;
    
    public bool InitJob()
    {
        taskID = Guid.NewGuid().ToString();

        switch (taskDifficulty)
        {
            case Difficulty.EASY:
                recommendedUnitCount = 1;
                break;
            case Difficulty.MEDIUM:
                recommendedUnitCount = 2;
                break;
            case Difficulty.HARD:
                recommendedUnitCount = 3;
                break;
        }

        Debug.Log(taskIconLocation);
        taskIcon = Resources.Load<Sprite>(taskIconLocation);

        if(taskIcon != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetJob()
    {
        isInQueue = false;
        isTaskActive = false;
        currentActiveTime = 0.0f;
        isTaskCompleted = false;
        currentPlayersAssigned = 0;
        completionTime = 0.0f;
    }
}
