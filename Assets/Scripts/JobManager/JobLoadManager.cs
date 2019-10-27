using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class JobLoadManager : MonoBehaviour
{
    private string jsonFilePath;

    private Jobs jobs = null;

    void Awake()
    {
        jsonFilePath = Application.dataPath + "/JSONFiles/";
    }


    private void SaveData()
    {

        string jsonData = JsonUtility.ToJson(jobs, true);
        File.WriteAllText(jsonFilePath, jsonData);

    }

    public void ForceSave()
    {
        print("Forcing save of current data");
        string jsonData = JsonUtility.ToJson(jobs, true);
        File.WriteAllText(jsonFilePath, jsonData);
    }

    public bool LoadData()
    {
        jobs = JsonUtility.FromJson<Jobs>(File.ReadAllText(jsonFilePath + "Jobs.json"));

        foreach(var job in jobs.jobList)
        {
            bool jobInitStatus = job.InitJob();

            if (jobInitStatus == false)
            {
                Debug.Log("Failed to initialise job: " + job.taskName + ". Please check values, and sprite file location");
            }
        }

        if (jobs != null && jobs.jobList.Count != 0)
        {
            Debug.Log("Loaded Jobs And Jobs Where Found");
            return true;
        }
        else if (jobs != null && jobs.jobList.Count == 0)
        {
            Debug.Log("Loaded Jobs And No Jobs Where Found");
            return false;
        }
        else
        {
            Debug.Log("Failed To Load Jobs");
            return false;
        }
    }

    public Jobs GetJobs()
    {
        return jobs;
    }
}