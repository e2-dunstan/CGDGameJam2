using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

public class JobOfferBox : MonoBehaviour
{
    [SerializeField] Text jobTitle;
    [SerializeField] Text jobDescription;
    [SerializeField] Image jobIcon;
    [SerializeField] GameObject[] difficultyArray;

    [SerializeField] AnimationCurve spawnCurve;
    [SerializeField] float spawnDuration = 1.0f;

    Vector3 startScale = new Vector3(0, 0, 0);
    Vector3 endScale = new Vector3(1, 1, 1);

    private void Awake()
    {
        startScale = new Vector3(0, 0, 1);
        endScale = this.transform.localScale;
    }

    private void OnEnable()
    {
        float delay = 0.0f;
        Tween.LocalScale(this.transform, startScale, endScale, spawnDuration, delay, spawnCurve, Tween.LoopType.None);
    }

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
        Debug.Log("Accepted Job, Attempting To Asssign To A Valid Target");
        JobManager.Instance.AcceptJobAndAssignToEmployee();
    }

    public void DeclineJob()
    {
        //Remove job
        Debug.Log("Declined Job, Attempting To Asssign To A Valid Target");
        JobManager.Instance.DeclineJobAndAssignToEmployee();
    }

    public void CloseJobOfferBox()
    {
        float delay = 0.0f;
        Tween.LocalScale(this.transform, endScale, startScale, 0.75f, delay, Tween.EaseInBack, Tween.LoopType.None, null, DestroyBox);
    }

    private void DestroyBox()
    {
        Destroy(this.gameObject);
    }
}
