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

    public bool debugVisualisation = true;

    public bool Selected
    {
        get; set;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) Debug.LogError("All employees must have a NavMeshAgent!");
    }

    private void Update()
    {
        if (path != null)
        {
            state = State.MOVING;

            if (debugVisualisation)
            {
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                }
            }
        }
        else
        {
            state = State.IDLE;
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
}
