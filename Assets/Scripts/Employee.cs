using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Employee : MonoBehaviour
{
    public enum Gender
    {
        MALE, FEMALE
    }
    public Gender gender;

    public enum State
    {
        IDLE, RELAXING, MOVING, WORKING
    }
    private State state = State.IDLE;

    private Vector3 destination;
    private NavMeshAgent agent;
    private NavMeshPath path;

    private Animator anim;
    private float moveSpeed = 0;
    private float currentMaxSpeed = 1.0f;
    private float defaultMaxSpeed;
    private float timeIdle = 0;
    private float timeIdleToWait = 5;

    public bool debugVisualisation = true;

    private bool shouldRelaxAfterMoving = false;
    private InteractableFurniture.Interactable currentInteractable = null;

    public bool Selected
    {
        get; set;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) Debug.LogError("All employees must have a NavMeshAgent!");
        defaultMaxSpeed = agent.speed;

        anim = GetComponentInChildren<Animator>();

        if (gender == Gender.MALE)
            anim.SetBool("Male", true);
        else
            anim.SetBool("Male", false);

        timeIdleToWait = Random.Range(3, 10);
    }

    private void Update()
    {
        moveSpeed = agent.velocity.magnitude / defaultMaxSpeed;
        anim.SetFloat("MoveSpeed", moveSpeed);

        switch (state)
        {
            case State.IDLE:
                UpdateIdle();
                break;
            case State.RELAXING:
                UpdateRelaxing();
                break;
            case State.MOVING:
                UpdateMoving();
                break;
            case State.WORKING:
                break;
        }
    }

    private void ChangeState(State newState)
    {
        switch (newState)
        {
            case State.IDLE:
                if (currentInteractable != null)
                {
                    currentInteractable.occupied = false;
                    if (currentInteractable.type == InteractableFurniture.Interactable.Type.CHAIR)
                    {
                        anim.SetTrigger("Stand");
                        MoveTo(transform.position + (transform.forward * 3));
                    }

                    currentInteractable = null;
                }
                break;
            case State.RELAXING:

                StartCoroutine(RotateTo(currentInteractable.origin.rotation));
                if (currentInteractable.type == InteractableFurniture.Interactable.Type.CHAIR)
                    anim.SetTrigger("Sit");

                break;
            case State.MOVING:

                break;
            case State.WORKING:

                break;
        }
        state = newState;
    }

    #region UPDATE STATES

    private void UpdateIdle()
    {
        if (moveSpeed > 0.05)
        {
            ChangeState(State.MOVING);
        }
        if (anim.GetBool("Pant")) anim.SetBool("Pant", false);

        timeIdle += Time.deltaTime;
        if (timeIdle > timeIdleToWait)
        {
            shouldRelaxAfterMoving = true;

            currentInteractable = InteractableFurniture.Instance.GetInteractable();
            if (currentInteractable != null)
                MoveTo(currentInteractable.origin.position);
            
            timeIdle = 0; 
            timeIdleToWait = Random.Range(3, 10);
        }
    }

    private void UpdateRelaxing()
    {
        if (currentInteractable == null) return;

        timeIdle += Time.deltaTime;
        if (timeIdle > timeIdleToWait)
        {
            ChangeState(State.IDLE);
            timeIdle = 0;
            timeIdleToWait = Random.Range(3, 10);
        }
    }

    private void UpdateMoving()
    {
        if (CanMove())
        {
            currentMaxSpeed = defaultMaxSpeed * (shouldRelaxAfterMoving ? 0.2f : 1.0f);
            agent.speed = currentMaxSpeed;
        }
        else
        {
            agent.speed = 0;
        }

        if (moveSpeed > 0.7f)
        {
            anim.SetBool("Pant", true);
        }

        if (moveSpeed < 0.05 && Vector3.Distance(agent.pathEndPosition, transform.position) < 0.05f)
        {
            if (shouldRelaxAfterMoving)
            {
                shouldRelaxAfterMoving = false;
                ChangeState(State.RELAXING);
            }
            else
            {
                ChangeState(State.IDLE);
            }
        }

        if (debugVisualisation)
        {
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            }
        }
    }

    #endregion

    private bool CanMove()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Motion")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("female_idle_pant")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("male_idle_pant"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator RotateTo(Quaternion rot)
    {
        rot.x = transform.rotation.x;
        rot.z = transform.rotation.z;

        float rotTime = 0.2f;
        for(float t = 0; t < rotTime; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, t / rotTime);

            yield return null;
        }

        transform.rotation = rot;
    }


    #region NAVMESH
    public void ProcessNewPath(TouchInput.PlayerTouch _touchInfo)
    {
        ChangeState(State.IDLE);

        Ray ray = Camera.main.ScreenPointToRay(_touchInfo.touchEnd);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            MoveTo(hit.point);
        }
    }

    private void MoveTo(Vector3 pos)
    {
        destination = pos;
        StartCoroutine(GetPath());
    }

    private IEnumerator GetPath()
    {
        path = new NavMeshPath();
        float timeToFindPath = 0;
        while (path.status != NavMeshPathStatus.PathComplete && !Selected)
        {
            agent.CalculatePath(destination, path);

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            }

            timeToFindPath += Time.deltaTime;
            if (timeToFindPath > 3.0f)
            {
                break;
            }
            yield return null;
        }

        timeToFindPath = 0;

        ChangeState(State.MOVING);
        agent.destination = destination;
        agent.SetPath(path);
    }

    #endregion
}
