using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskRoomManager : MonoBehaviour
{
    public Job job = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Employee"))
        {
            Debug.Log("Player entered room");

            if (other.gameObject.GetComponent<EmployeeJobManager>().hasJob)
            {
                job.isTaskActive = true;
                job = other.gameObject.GetComponent<EmployeeJobManager>().GetJob();
                JobUIManager.Instance.SpawnUIElement(JobUIManager.UIElement.PROGRESS_BAR, gameObject);
            }
        }
    }
}
