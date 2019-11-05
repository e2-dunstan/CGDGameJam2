using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;


[System.Serializable]
public enum Difficulty
{
    EASY = 0,
    MEDIUM = 1,
    HARD = 2
}

public enum Event
{ 
    REQUIRE_NUMBER_OF_PEOPLE = 0,
    REQUIRE_ITEM = 1,
    REQUIRE_PINK_PERSON = 2,
    REQUIRE_BLUE_PERSON = 3,
    NONE = 4
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

    //Random Event System Variables
    public class GenericStruct
    {
        public void Init<T>(Event _key, T _value) where T : class
        {
            key = _key;
            genericObj = _value;
        }

        public Event GetEvent()
        {
            return key;
        }

        public T GetValue<T>() where T : class
        {
            return genericObj as T;
        }

        public string GetID()
            {
            return id;
            }

        public void SetColour(Color _color)
        {
            color = _color;
        }

        public Color GetColor()
        {
            return color;
        }

        private Event key;
        private object genericObj;
        private Color color;
        private string id = Guid.NewGuid().ToString();
    }

    public class GenericEventList
    {
        public List<GenericStruct> genericEventList = new List<GenericStruct>();

        public void Add<T>(Event key, T value, Color color) where T : class
        {
            GenericStruct tempStruct = new GenericStruct();
            tempStruct.Init(key, value);
            tempStruct.SetColour(color);
            genericEventList.Add(tempStruct);
        }
    }

    public class GenericInt
    {
        public GenericInt(int _val)
        {
            number = _val;
        }
        public int number = 3;
    }

    public class GenericGameObject
    {
        public GenericGameObject(GameObject _gameObj)
        {
            gameObj = _gameObj; 
        }

        public GameObject gameObj = null;
    }

    public GenericEventList eventList = new GenericEventList();

    public void RemoveEventFromEventList(GenericStruct _event)
    {
        List<GenericStruct> newEventList = eventList.genericEventList.Where(x => x.GetID() != _event.GetID()).ToList();
        eventList.genericEventList = newEventList;
    }

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
