using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TaskRoomManager : MonoBehaviour
{
    public Job job = null;
    private GameObject progressBar = null;

    private bool isTaskCompleted = false;
    private bool isJobInProgress = false;

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

            if (CheckIfJobHasRequirements())
            {
                progressBar.GetComponent<TaskProgressBar>().PauseProgress();
            }
            else
            {
                progressBar.GetComponent<TaskProgressBar>().UnPauseProgress();

            }
        }

        if(isTaskCompleted)
        {
                JobManager.Instance.AlertJobHasBeenCompleted();
                GameObject employeeWithoutJob = employeesInRoom.Where(x => x.GetComponent<EmployeeJobManager>().hasJob != true).FirstOrDefault();

                if (employeeWithoutJob != null)
                {
                    employeeWithoutJob.GetComponent<EmployeeJobManager>().SetJob(job, JobUIManager.UIElement.HAS_COMPLETED_TASK);
                    Destroy(progressBar);
                    job = null;
                    isTaskCompleted = false;
                    isJobInProgress = false;
                }
        }

        //int numEmployeesWorking = employeesInRoom.Count(x => x.gameObject.GetComponent<Employee>().GetEmployeeState() == Employee.State.WORKING);

        if (job != null)
        {
            job.currentPlayersAssigned = employeesInRoom.Count;
        }
    }

    private bool CheckIfJobHasRequirements()
    {
        bool conditionRequired = false;

        if (job.eventList.genericEventList.Count > 0)
        {
            foreach(var jobEvent in job.eventList.genericEventList)
            {
               
                switch(jobEvent.GetEvent())
                {
                    case Event.REQUIRE_NUMBER_OF_PEOPLE:
                        {
                            if (employeesInRoom.Count < jobEvent.GetValue<Job.GenericInt>("Test").number)
                            {
                                Debug.Log("The employees are stuck, the task needs more people assigned! Need: " + jobEvent.GetValue<Job.GenericInt>("Test").number + "People");
                                
                                conditionRequired = true;
                            }
                            else
                            {
                                //Remove from event list becaues condition has been met
                                job.RemoveEventFromEventList(jobEvent);
                            }
                            break;
                        }
                    case Event.REQUIRE_FEMALES:
                        {
                            int numberOfFemalesInRoom = employeesInRoom.Where(x => x.GetComponent<Employee>().gender == Employee.Gender.FEMALE).Count();
                            if (numberOfFemalesInRoom < jobEvent.GetValue<Job.GenericInt>("Test").number)
                            {
                                Debug.Log("Need " + jobEvent.GetValue<Job.GenericInt>("Test").number + "Females");
                                conditionRequired = true;
                            }
                            else
                            {
                                //Remove from event list becaues condition has been met
                                job.RemoveEventFromEventList(jobEvent);
                            }
                            break;
                        }
                    case Event.REQUIRE_MALES:
                        {
                            int numberOfMalesInRoom = employeesInRoom.Where(x => x.GetComponent<Employee>().gender == Employee.Gender.MALE).Count();
                            if (numberOfMalesInRoom < jobEvent.GetValue<Job.GenericInt>("Test").number)
                            {
                                Debug.Log("Need " + jobEvent.GetValue<Job.GenericInt>("Test").number + "Males");
                                conditionRequired = true;
                            }
                            else
                            {
                                //Remove from event list becaues condition has been met
                                job.RemoveEventFromEventList(jobEvent);
                            }
                            break;
                        }
                    case Event.REQUIRE_ITEM:
                        job.RemoveEventFromEventList(jobEvent);
                        Debug.Log("Item Required In Room To Continue! Go Bring One");
                        return false;
                        //CheckIfItemIsInRoom
       
                }
            }
        }

        return conditionRequired;
    }
    private void RemoveEmployeeFromList(GameObject _employee)
    {
        List<GameObject> newEmployeeList = employeesInRoom.Where(x => x.gameObject.GetInstanceID() != _employee.gameObject.GetInstanceID()).ToList();
        employeesInRoom = newEmployeeList;
    }

    private bool IsEmployeeAlreadyInRoom(GameObject _employee)
    {
        GameObject employee = employeesInRoom.Where(x => x.gameObject.GetInstanceID() == _employee.gameObject.GetInstanceID()).FirstOrDefault();

        if(employee == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    int numEmployeesWorking = employeesInRoom.Count(x => x.gameObject.GetComponent<Employee>().GetEmployeeState() == Employee.State.WORKING);

    //    Debug.Log(numEmployeesWorking);

    //    if (job != null)
    //    {
    //        job.currentPlayersAssigned = numEmployeesWorking;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Employee"))
        {
            Debug.Log("Player entered room");

            employeesInRoom.Add(other.gameObject);

            if (other.gameObject.GetComponent<EmployeeJobManager>().hasJob && isJobInProgress == false)
            {
                other.GetComponent<Employee>().ChangeState(Employee.State.WORKING);
                job = other.gameObject.GetComponent<EmployeeJobManager>().GetJob();
                progressBar = JobUIManager.Instance.SpawnUIElement(JobUIManager.UIElement.PROGRESS_BAR, gameObject);
                progressBar.GetComponent<TaskProgressBar>().SetJob(job);
                job.isTaskActive = true;
                isJobInProgress = true;
            }
            else if(isJobInProgress == true)
            {
                other.GetComponent<Employee>().ChangeState(Employee.State.WORKING);
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
