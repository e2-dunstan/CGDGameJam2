using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TaskRoomManager : MonoBehaviour
{
    public Job job = null;
    public GameObject progressBar = null;

    private bool isTaskCompleted = false;
    private bool isJobInProgress = true;

    List<GameObject> employeesInRoom = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if(progressBar != null)
        {
            isTaskCompleted = progressBar.GetComponent<TaskProgressBar>().IsTaskDone();
        }

        if(isTaskCompleted)
        {
            
                GameObject employeeWithoutJob = employeesInRoom.Where(x => x.GetComponent<EmployeeJobManager>().hasJob != true).FirstOrDefault();

                if (employeeWithoutJob != null)
                {
                    employeeWithoutJob.GetComponent<EmployeeJobManager>().SetJob(job, JobUIManager.UIElement.HAS_COMPLETED_TASK);
                    Destroy(progressBar);
                    isTaskCompleted = false;
                    isJobInProgress = false;
                }
        }
    }

    private void RemoveEmployeeFromList(GameObject _employee)
    {
        List<GameObject> newEmployeeList = employeesInRoom.Where(x => x.gameObject.GetInstanceID() != _employee.gameObject.GetInstanceID()).ToList();
        employeesInRoom = newEmployeeList;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Employee"))
        {
            Debug.Log("Player entered room");

            employeesInRoom.Add(other.gameObject);

            if (other.gameObject.GetComponent<EmployeeJobManager>().hasJob)
            {
                other.GetComponent<Employee>().ChangeState(Employee.State.WORKING);
                job = other.gameObject.GetComponent<EmployeeJobManager>().GetJob();
                progressBar = JobUIManager.Instance.SpawnUIElement(JobUIManager.UIElement.PROGRESS_BAR, gameObject);
                progressBar.GetComponent<TaskProgressBar>().SetJob(job);
                job.isTaskActive = true;
                isJobInProgress = true;
                job.currentPlayersAssigned++;
            }
            else if(isJobInProgress)
            {
                job.currentPlayersAssigned++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Employee"))
        {
            if (employeesInRoom.Where(x => x.gameObject.GetInstanceID() == other.gameObject.GetInstanceID()).FirstOrDefault() != null)
            {
                if (job != null)
                {
                    job.currentPlayersAssigned--;
                }
                RemoveEmployeeFromList(other.gameObject);
            }
        }
    }
}
