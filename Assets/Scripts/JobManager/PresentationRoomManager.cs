using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationRoomManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Employee"))
        {
            Debug.Log("Employee entered presentation room");

            Job job = other.gameObject.GetComponent<EmployeeJobManager>().GetJob();

            JobManager.Instance.CompleteJob(job.taskID);
        }
    }
}
