using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int currentPlayersAssigned;
    //Amount by which task completion speed is effected for each extra person
    public int playerSpeedMultiplier;
    public int timeUntilFailure;

    public bool InitJob()
    {
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
}
