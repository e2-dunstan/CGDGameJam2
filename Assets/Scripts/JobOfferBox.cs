using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobOfferBox : MonoBehaviour
{
    [SerializeField] Text jobTitle;
    [SerializeField] Text jobDescription;
    [SerializeField] Image jobIcon;
    [SerializeField] GameObject[] difficultyArray;

    public void SetUpJobUI(Job job)
    {
        jobTitle.text = job.taskName;
        jobDescription.text = job.taskDescription;
        jobIcon.sprite = job.taskIcon;

        int difficulty = 0;

        switch (job.taskDifficulty)
        {
            case Difficulty.EASY:
                difficulty = 1;
                break;
            case Difficulty.MEDIUM:
                difficulty = 2;
                break;
            case Difficulty.HARD:
                difficulty = 3;
                break;
        }

        for (int i = 0; i < difficultyArray.Length; i++)
        {
            difficultyArray[i].SetActive(i < difficulty);
        }
    }

    public void AcceptJob()
    {
        Debug.Log("Accepted Job");
        JobManager.Instance.AcceptJobAndAssignToEmployee();
    }

    public void DeclineJob()
    {
        //Remove job
        JobManager.Instance.DeclineJobAndAssignToEmployee();
    }
}
