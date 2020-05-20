using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationRoomManager : MonoBehaviour
{
    GameObject alertUIElement = null;

    int numberOfJobsCompleted = 0;

    ParticleTween particleTween;

    private void Awake()
    {
        particleTween = GetComponent<ParticleTween>();
    }

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
                AudioManager.Instance.Play(AudioManager.SoundsType.TASK, (int)AudioManager.TaskSounds.COMPLETED, 0.1f);
                JobManager.Instance.CompleteJob(job.taskID);
                ReputationManager.Instance.JobCompleted(Mathf.FloorToInt(job.baseTaskScore), job.taskTime, job.completionTime, job.taskDifficulty);

                ParticleSystemHandler.Instance.EmitTaskSubmitParticle(transform.position + new Vector3(0, 1, 0));
                AudioManager.Instance.Play(AudioManager.SoundsType.MISC, (int)AudioManager.MiscSounds.CELEBRATION, 0.1f);

                numberOfJobsCompleted--;

                particleTween.TweenParticles(Camera.main.WorldToScreenPoint(this.transform.position), ReputationManager.Instance.repUi.GetPosition(), job.taskDifficulty);

                if (numberOfJobsCompleted == 0)
                {
                    alertUIElement.SetActive(false);
                }
            }
        }
    }
}
