using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationRoomManager : MonoBehaviour
{
    GameObject alertUIElement = null;

    int numberOfJobsCompleted = 0;

    private void Start()
    {
        alertUIElement = JobUIManager.Instance.SpawnUIElement(JobUIManager.UIElement.PRESENTATION_ROOM_ALERT, gameObject);
        alertUIElement.SetActive(false);
    }

    public void AlertJobHasBeenCompleted()
    {
        alertUIElement.SetActive(true);
        numberOfJobsCompleted++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Employee"))
        {
            Debug.Log("Employee entered presentation room");

            if (other.gameObject.GetComponent<EmployeeJobManager>().hasJob && other.gameObject.GetComponent<EmployeeJobManager>().GetJob().isTaskCompleted)
            {
                Job job = other.gameObject.GetComponent<EmployeeJobManager>().GetJobAndRemoveUIElement();

                JobManager.Instance.CompleteJob(job.taskID);
                ReputationManager.Instance.JobCompleted(Mathf.FloorToInt(job.baseTaskScore), job.taskTime, job.completionTime, job.taskDifficulty);

                numberOfJobsCompleted--;

                if (numberOfJobsCompleted == 0)
                {
                    alertUIElement.SetActive(false);
                }
            }
        }
    }
}
