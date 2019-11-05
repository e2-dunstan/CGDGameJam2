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

    [HideInInspector] public Room currentRoom = Room.RELAX; 

    public enum State
    {
        IDLE, RELAXING, MOVING, WORKING
    }
    private State state = State.IDLE;

    private Vector3 destination;
    private NavMeshAgent agent;
    private NavMeshPath path;

    public NavMeshPath EmployeeNavMeshPath { get => path; private set => path = value; }
    public NavMeshAgent EmployeeNavMeshAgent { get => agent; private set => agent = value; }

    private Animator anim;
    private float moveSpeed = 0;
    private float currentMaxSpeed = 1.0f;
    private float defaultMaxSpeed;
    private float timeIdle = 0;
    private float timeIdleToWait = 5;
    private float timeSitToWait = 5;

    public bool debugVisualisation = true;

    private bool shouldRelaxAfterMoving = false;
    private InteractableFurniture.Interactable currentInteractable = null;

    private Vector3 roomEntry = Vector3.zero;


    [SerializeField] private GameObject itemHoldPosition = null;
    [SerializeField] private GameObject pickupParticle = null;
    public bool hasItem = false;

    public bool Selected
    {
        get; set;
    }

    public State GetEmployeeState()
    {
        return state;
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

        timeIdleToWait = Random.Range(1, 3);
        timeSitToWait = Random.Range(10, 30);
    }

    private void OnEnable()
    {
        state = State.IDLE;

        InitialiseParticleSystems();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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
                if (currentInteractable != null && currentInteractable.type == InteractableFurniture.Interactable.Type.CHAIR
                    && !IsSitting())
                    anim.SetTrigger("Sit");
                VFXController.Instance.PlayParticleSystemOnEmployee(
                    gameObject,
                    VFXController.Instance.idlePSList,
                    0, 4);
                break;
        }
    }

    public void ChangeState(State newState)
    {
        if(state == State.WORKING && newState != State.WORKING)
        {
            //StartCoroutine(LerpFromTo(transform.position, roomEntry));
            anim.SetTrigger("Stand");
            VFXController.Instance.StopParticleSystemOnEmployee(
                gameObject,
                VFXController.Instance.idlePSList);
        }

        switch (newState)
        {
            case State.IDLE:
                if (currentInteractable != null && currentRoom != Room.TASK_1 && currentRoom != Room.TASK_2 && currentRoom != Room.TASK_3)
                {
                    currentInteractable.occupied = false;
                    if (currentInteractable.type == InteractableFurniture.Interactable.Type.CHAIR)
                    {
                        anim.SetTrigger("Stand");
                        //if (state != State.WORKING) 
                            //MoveTo(transform.position + (transform.forward * 1));
                    }

                    currentInteractable = null;
                }
                break;
            case State.RELAXING:
                if (currentInteractable == null) break;

                StartCoroutine(RotateTo(currentInteractable.origin.rotation));
                if (currentInteractable.type == InteractableFurniture.Interactable.Type.CHAIR)
                    anim.SetTrigger("Sit");


                break;
            case State.MOVING:

                break;
            case State.WORKING:
                if (state != State.WORKING)
                {
                    //agent.ResetPath();
                    FindWorkstation();
                }
                break;
        }


        if (currentInteractable != null && newState == State.RELAXING
            && (currentInteractable.room == Room.TASK_1 || currentInteractable.room == Room.TASK_2 || currentInteractable.room == Room.TASK_3))
        {
            state = State.WORKING;
        }
        else
        {
            state = newState;
        }
    }

    private bool IsSitting()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("male_sit")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("female_sit")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("male_talk_sit")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("female_talk_sit")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("male_laugh_sit")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("female_laugh_sit"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FindWorkstation()
    {
        currentInteractable = InteractableFurniture.Instance.GetInteractable(currentRoom);

        if (currentInteractable != null)
        {
            if (currentInteractable.type == InteractableFurniture.Interactable.Type.CHAIR) shouldRelaxAfterMoving = true;
            MoveTo(currentInteractable.origin.position);
            //StartCoroutine(GoToWorkstation());
        }
        else
        {
            Debug.LogWarning("Cannot find a work room interactable");
        }
    }

    //private IEnumerator GoToWorkstation()
    //{
    //    roomEntry = transform.position;

    //    yield return LerpFromTo(roomEntry, currentInteractable.origin.position);

    //    if (currentInteractable != null)
    //    {
    //        yield return RotateTo(currentInteractable.origin.rotation);

    //        if (currentInteractable.type == InteractableFurniture.Interactable.Type.CHAIR)
    //            anim.SetTrigger("Sit");
    //    }
    //}

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

        float waitTime = currentInteractable.type == InteractableFurniture.Interactable.Type.CHAIR ? timeSitToWait : timeIdleToWait;

        if (timeIdle > waitTime)
        {
            ChangeState(State.IDLE);
            timeIdle = 0;
            timeIdleToWait = Random.Range(0, 3);
            timeSitToWait = Random.Range(10, 30);
        }
    }

    private void UpdateMoving()
    {
        VFXController.Instance.PlayParticleSystemOnEmployee(
            gameObject,
            VFXController.Instance.runningPSList);
        //if (CanMove())
        //{
        //    currentMaxSpeed = defaultMaxSpeed;
        //    currentMaxSpeed = defaultMaxSpeed * (shouldRelaxAfterMoving ? 0.2f : 1.0f);
        //    agent.speed = currentMaxSpeed;
        //}
        //else
        //{
        //    agent.speed = 0;
        //}

        //if (moveSpeed > 0.7f)
        //{
        //    anim.SetBool("Pant", true);
        //}

        if (/*moveSpeed < 0.05 && */Vector3.Distance(agent.pathEndPosition, transform.position) < 0.05f)
        {
            if (shouldRelaxAfterMoving)
            {
                shouldRelaxAfterMoving = false;
                ChangeState(State.RELAXING);
            }
            else
            {
                if (currentRoom != Room.TASK_1 && currentRoom != Room.TASK_2 && currentRoom != Room.TASK_3)
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<RoomType>() != null)
        {
            currentRoom = other.gameObject.GetComponent<RoomType>().roomType;
            if (currentRoom != Room.RELAX)
                anim.SetBool("Pant", false);
        }
    }

    private bool CanMove()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Motion"))
            //|| anim.GetCurrentAnimatorStateInfo(0).IsName("female_idle_pant")
            //|| anim.GetCurrentAnimatorStateInfo(0).IsName("male_idle_pant"))
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

    //private IEnumerator LerpFromTo(Vector3 from, Vector3 to)
    //{
    //    float lerpTime = 1.0f;

    //    for (float t = 0; t < lerpTime; t += Time.deltaTime)
    //    {
    //        transform.position = Vector3.Lerp(from, to, t);
    //        yield return null;
    //    }

    //    transform.position = to;
    //}

    public State GetState()
    {
        return state;
    }

    public float GetMoveSpeed()
    {
        return anim.speed;
    }
    void InitialiseParticleSystems()
    {
        VFXController.Instance.CreateParticleSystemForEmployee(
            VFXController.Instance.runningPS,
            VFXController.Instance.runningPSList);

        VFXController.Instance.CreateParticleSystemForEmployee(
            VFXController.Instance.idlePS,
            VFXController.Instance.idlePSList);

        VFXController.Instance.CreateParticleSystemForEmployee(
            VFXController.Instance.pulsePS,
            VFXController.Instance.pulsePSList);
    }

    #region NAVMESH
    public void ProcessNewPath(TouchInput.PlayerTouch _touchInfo)
    {
        ChangeState(State.IDLE);

        Ray ray = Camera.main.ScreenPointToRay(_touchInfo.touchEnd);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, TouchInput.Instance.layerMask))
        {
            MoveTo(hit.point);
        }
    }

    private void MoveTo(Vector3 pos)
    {
        StopAllCoroutines();
        destination = pos;
        StartCoroutine(GetPath());
    }

    private IEnumerator GetPath()
    {
        if (!agent.enabled) agent.enabled = true;

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

        while (!CanMove()) yield return null;

        ChangeState(State.MOVING);
        agent.destination = destination;
        agent.SetPath(path);
    }

    #endregion

    public GameObject GetItemHoldTransform()
    {
        return itemHoldPosition;
    }

    public void SetPickupParticleActiveState(bool state)
    {
        pickupParticle.SetActive(state);
    }

    public void SetPickupParticleColour(Color color)
    {
        pickupParticle.GetComponent<ParticleSystem>().startColor = color;
    }
}
