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

            Job job = other.gameObject.GetComponent<EmployeeJobManager>().GetJob();

            JobManager.Instance.CompleteJob(job.taskID);

            numberOfJobsCompleted--;

            if (numberOfJobsCompleted == 0)
            {
                alertUIElement.SetActive(false);
            }
        }
    }
}
