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
    private bool hasCompletedJobBeenNotifiedToPresentationRoom = false;

    List<GameObject> employeesInRoom = new List<GameObject>();

    public bool roomUsesEvents = false;

    [SerializeField]
    private GameObject requiredItem = null;
    private bool isItemInRoom = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if(progressBar != null)
        {
            if (progressBar.GetComponent<TaskProgressBar>().active)
            {
                isTaskCompleted = progressBar.GetComponent<TaskProgressBar>().IsTaskDone();
            }

            if (roomUsesEvents)
            {
                if (CheckIfJobHasRequirements())
                {
                    progressBar.GetComponent<TaskProgressBar>().PauseProgress();
                }
                else
                {
                    progressBar.GetComponent<TaskProgressBar>().UnPauseProgress();
                }
            }

            if(progressBar.GetComponent<TaskProgressBar>().active)
            {
                isTaskCompleted = progressBar.GetComponent<TaskProgressBar>().IsTaskDone();
            }
        }

        if(isTaskCompleted)
        {
            if (!hasCompletedJobBeenNotifiedToPresentationRoom)
            {
                JobManager.Instance.AlertJobHasBeenCompleted();
                hasCompletedJobBeenNotifiedToPresentationRoom = true;
            }

            CompleteJobAndAssignToEmployee();

            foreach(var employee in employeesInRoom)
            {
                employee.GetComponent<Employee>().ChangeState(Employee.State.IDLE);
            }
        }

        //int numEmployeesWorking = employeesInRoom.Count(x => x.gameObject.GetComponent<Employee>().GetEmployeeState() == Employee.State.WORKING);

        if (job != null)
        {
            job.currentPlayersAssigned = employeesInRoom.Count(x => x.activeSelf);
        }
    }

    private bool CheckIfJobHasRequirements()
    {
        bool conditionRequired = false;

        if (job != null)
        {
            if (job.eventList.genericEventList.Count > 0)
            {
                foreach (var jobEvent in job.eventList.genericEventList)
                {
                   
                        switch (jobEvent.GetEvent())
                        {
                            case Event.REQUIRE_NUMBER_OF_PEOPLE:
                                {
                                    if (employeesInRoom.Count(x => x.activeSelf) < jobEvent.GetValue<Job.GenericInt>().number)
                                    {
                                        Debug.Log("The employees are stuck, the task needs more people assigned! Need: " + jobEvent.GetValue<Job.GenericInt>().number + "People");
                                        progressBar.GetComponent<TaskProgressBar>().taskHasRequirements = Event.REQUIRE_NUMBER_OF_PEOPLE;
                                        progressBar.GetComponent<TaskProgressBar>().numberOfPeopleRequired = jobEvent.GetValue<Job.GenericInt>().number;
                                        conditionRequired = true;
                                    }
                                    else
                                    {
                                        //Remove from event list becaues condition has been met
                                        job.RemoveEventFromEventList(jobEvent);
                                        progressBar.GetComponent<TaskProgressBar>().ResetProgressBarEvent();
                                    }
                                    break;
                                }
                            case Event.REQUIRE_PINK_PERSON:
                                {
                                    int numberOfFemalesInRoom = employeesInRoom.Where(x => x.GetComponent<Employee>().gender == Employee.Gender.FEMALE && x.activeSelf).Count();
                                    
                                    if (numberOfFemalesInRoom < jobEvent.GetValue<Job.GenericInt>().number)
                                    {
                                        //Debug.Log("Need " + jobEvent.GetValue<Job.GenericInt>().number + "Pink Persons");
                                        progressBar.GetComponent<TaskProgressBar>().taskHasRequirements = Event.REQUIRE_PINK_PERSON;
                                        conditionRequired = true;
                                    }
                                    else
                                    {
                                        //Remove from event list becaues condition has been met
                                        job.RemoveEventFromEventList(jobEvent);
                                        progressBar.GetComponent<TaskProgressBar>().ResetProgressBarEvent();
                                    }
                                    break;
                                }
                            case Event.REQUIRE_BLUE_PERSON:
                                {
                                    int numberOfMalesInRoom = employeesInRoom.Where(x => x.GetComponent<Employee>().gender == Employee.Gender.MALE).Count();
                                    if (numberOfMalesInRoom < jobEvent.GetValue<Job.GenericInt>().number)
                                    {
                                        Debug.Log("Need " + jobEvent.GetValue<Job.GenericInt>().number + "Blue Persons");
                                        progressBar.GetComponent<TaskProgressBar>().taskHasRequirements = Event.REQUIRE_BLUE_PERSON;
                                        conditionRequired = true;
                                    }
                                    else
                                    {
                                        //Remove from event list becaues condition has been met
                                        job.RemoveEventFromEventList(jobEvent);
                                        progressBar.GetComponent<TaskProgressBar>().ResetProgressBarEvent();
                                    }
                                    break;
                                }
                            case Event.REQUIRE_ITEM:
                            if (!isItemInRoom)
                            {
                                requiredItem = jobEvent.GetValue<Job.GenericGameObject>().gameObj;
                                progressBar.GetComponent<TaskProgressBar>().taskHasRequirements = Event.REQUIRE_ITEM;
                                progressBar.GetComponent<TaskProgressBar>().itemRequiredTextColour = jobEvent.GetColor();
                                //Debug.Log("Item Required In Room To Continue! Go Bring One");
                                conditionRequired = true;
                            }
                            else
                            {
                                isItemInRoom = false;
                                requiredItem = null;
                                job.RemoveEventFromEventList(jobEvent);
                                progressBar.GetComponent<TaskProgressBar>().ResetProgressBarEvent();
                            }
                            break;
                        }
                    }
                
            }
        }

        return conditionRequired;
    }
    private void CompleteJobAndAssignToEmployee()
    {
        GameObject employeeWithoutJob = employeesInRoom.Where(x => x.GetComponent<EmployeeJobManager>().hasJob != true && x.activeSelf == true).FirstOrDefault();

        if (employeeWithoutJob != null)
        {
            employeeWithoutJob.GetComponent<EmployeeJobManager>().SetJob(job, JobUIManager.UIElement.HAS_COMPLETED_TASK);
            progressBar.GetComponent<TaskProgressBar>().CloseProgressBar();
            ParticleSystemHandler.Instance.EmitTaskCompleteParticle(this.transform.position);
            job = null;
            isTaskCompleted = false;
            isJobInProgress = false;
            hasCompletedJobBeenNotifiedToPresentationRoom = false;
        }
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

            if (other.gameObject.GetComponent<EmployeeJobManager>().hasJob
                && isJobInProgress == false
                && !other.gameObject.GetComponent<EmployeeJobManager>().GetJob().isTaskCompleted)
            {
                other.GetComponent<Employee>().currentRoom = GetComponent<RoomType>().roomType;
                other.GetComponent<Employee>().ChangeState(Employee.State.WORKING);
                job = other.gameObject.GetComponent<EmployeeJobManager>().GetJobAndRemoveUIElement();
                progressBar = JobUIManager.Instance.SpawnUIElement(JobUIManager.UIElement.PROGRESS_BAR, gameObject);
                progressBar.GetComponent<TaskProgressBar>().SetJob(job);
                job.isTaskActive = true;
                isJobInProgress = true;
            }
            else if(isJobInProgress == true)
            {
                other.GetComponent<Employee>().currentRoom = GetComponent<RoomType>().roomType;
                other.GetComponent<Employee>().ChangeState(Employee.State.WORKING);
            }
        }

        if(other.gameObject.CompareTag("Item"))
        {
            if (requiredItem != null)
            {
                if (other.gameObject.GetInstanceID() == requiredItem.GetInstanceID())
                {
                    other.gameObject.GetComponent<CollectableItem>().DeactiveEffect();
                    ItemManager.Instance.RemoveColorFromActive(other.gameObject.GetComponent<CollectableItem>().GetParticleColour());
                    Destroy(other.gameObject);
                    isItemInRoom = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Employee"))
        {
            if (employeesInRoom.Where(x => x.gameObject.GetInstanceID() == other.gameObject.GetInstanceID()).FirstOrDefault() != null)
            {
                other.gameObject.GetComponent<Employee>().ChangeState(Employee.State.IDLE);
                if (job != null)
                {
                    job.currentPlayersAssigned--;
                }
                RemoveEmployeeFromList(other.gameObject);
            }
        }
    }
}
