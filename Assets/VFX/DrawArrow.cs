using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArrow : MonoBehaviour
{
    public Vector3 startLoc;
    public Vector3 endDragLoc;
    public bool startPointSet;
    public bool endPointSet;

    public Employee targetEmployee = null;
    public GameObject target;
    private Vector3[] drawPoints = new Vector3[2];
    public List<Vector3> pathPoints = new List<Vector3>();
    private LineRenderer lr;
    float distLine = 0.0f;

    private float min = 0.65f;
    private float max = 20.0f;
    public bool reached;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        endPointSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        lr.enabled = ActivateLineRenderer();
        if (targetEmployee != null)
        {
            target = targetEmployee.gameObject;

            drawPoints[0] = new Vector3(
                        target.transform.position.x,
                        target.transform.position.y + 1,
                        target.transform.position.z);
        }
        if (startPointSet)
        {
            drawPoints[1] = endDragLoc;
            float distance = Vector3.Distance(startLoc, endDragLoc);
            ChangeSizeBasedOnDistance(distance);
            lr.SetPositions(drawPoints);
            lr.startWidth = distLine;
            lr.endWidth = distLine;
        }
        if (endPointSet)
        {

            if (pathPoints.Count == 0)
            {
                lr.positionCount = targetEmployee.EmployeeNavMeshPath.corners.Length;
                lr.SetPosition(0, target.transform.position);

                for (int i = 1; i < targetEmployee.EmployeeNavMeshPath.corners.Length ; i++)
                {
                    lr.SetPosition(i, targetEmployee.EmployeeNavMeshPath.corners[i]);
                }
            }

            lr.startWidth = 0.5f;
            lr.endWidth = 1;
        }
        reached = (Vector3.Distance(target.transform.position, endDragLoc) < 0.5f);
    }

    private bool ActivateLineRenderer()
    {
        if (startPointSet  || endPointSet)
        {
            return true;
        }
        else
        return false;
    }
    public void Reset()
    {
        startLoc = Vector3.zero;
        endDragLoc = Vector3.zero;
        lr.enabled = false;
        startPointSet = false;
        endPointSet = false;
        targetEmployee = null;
        lr.positionCount = 2;
        pathPoints.Clear();
        pathPoints = new List<Vector3>();
    }
    void ChangeSizeBasedOnDistance(float distance)
    {
        if (distance < min)
        {
            distLine = 1.0f;
        }
        else if (distance > max)
        {
            distLine = 0.1f;
        }
        else
        {
            distLine = 1 - (distance / max);
        }
        if (distLine <= 0.1f)
        {
            distLine = 0.1f;
        }
    }
}
