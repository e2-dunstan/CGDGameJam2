using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Employee : MonoBehaviour
{
    public enum State
    {
        IDLE, MOVING, WORKING
    }
    private State state = State.IDLE;

    private Vector3 destination;
    private NavMeshAgent agent;
    private NavMeshPath path;

    private Animator anim;

    public bool debugVisualisation = true;

    public bool Selected
    {
        get; set;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) Debug.LogError("All employees must have a NavMeshAgent!");

        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        float moveSpeed = agent.velocity.magnitude / agent.speed;
        Debug.Log(moveSpeed);
        anim.SetFloat("MoveSpeed", moveSpeed);

        switch (state)
        {
            case State.IDLE:
                if (moveSpeed > 0.05)
                {
                    state = State.MOVING;
                }
                anim.SetBool("Pant", false);
                break;
            case State.MOVING:
                if (moveSpeed > 0.7f)
                {
                    anim.SetBool("Pant", true);
                }

                if (moveSpeed < 0.05)
                {
                    state = State.IDLE;
                    //if (anim.GetBool("Pant")) StartCoroutine(Panting());
                }

                if (debugVisualisation)
                {
                    for (int i = 0; i < path.corners.Length - 1; i++)
                    {
                        Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                    }
                }

                break;
            case State.WORKING:
                break;
        }
    }

    public void ProcessNewPath(TouchInput.PlayerTouch _touchInfo)
    {
        Ray ray = Camera.main.ScreenPointToRay(_touchInfo.touchEnd);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
        }

        StartCoroutine(GetPath());
    }

    private IEnumerator GetPath()
    {
        path = new NavMeshPath();
        while (path.status != NavMeshPathStatus.PathComplete && !Selected)
        {
            agent.CalculatePath(destination, path);

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            }

            yield return null;
        }

        agent.SetPath(path);
    }

    //private IEnumerator Panting()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    anim.SetBool("Pant", false);
    //}
}
