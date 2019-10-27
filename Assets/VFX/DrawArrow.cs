using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArrow : MonoBehaviour
{
  //  private TouchInput ti;
   // private TouchInput.PlayerTouch pt;
    private StraightArrow anim;
    [SerializeField] private GameObject startloc;
    [SerializeField] private GameObject endLoc;
    [SerializeField] private bool onClick;
    [SerializeField] private bool startPointSet;
    [SerializeField] private bool endPointSet;
    public bool validMove;
    Vector3[] points = new Vector3[2];
    private LineRenderer lr;
    float distLine = 0.0f;

    float min = 0.45f;
    float max = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
       // ti = GameObject.FindGameObjectWithTag("GameController").GetComponent<TouchInput>();
       // pt = GameObject.FindGameObjectWithTag("GameController").GetComponent<TouchInput.PlayerTouch>();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        anim = GetComponent<StraightArrow>();
        anim.enabled = false;
        onClick = false;
        startPointSet = false;
        endPointSet = false;
        validMove = false;
    }

    // Update is called once per frame
    void Update()
    {

        //Once the player clicks a Dev
        if (onClick && !startPointSet && !endPointSet)
        {
            print("Set starting position!! " + startloc.transform.position);
            startPointSet = true;
        }

        //Once the player drops a dev
        if (onClick && !endPointSet && startPointSet && validMove)
        {
            print("Set end position!! " + endLoc.transform.position);
            endPointSet = true;
        }
        //Start playing the animation once the player drops the dev
        if (startPointSet && endPointSet)
        {
            anim.enabled = true;
        }
        else
        {
            anim.Kill();
            anim.enabled = false;
        }
        float distance = Vector3.Distance(startloc.transform.position, endLoc.transform.position);
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
            distLine = 1 -(distance / max);
        }
        if (distLine <= 0.25f)
        {
            distLine = 0.1f;
        }
        points[0] = startloc.transform.position;
        points[1] = endLoc.transform.position;
        print("Distance" + distance);

    }

    private void FixedUpdate()
    {
        if (!startPointSet && !endPointSet || startPointSet && endPointSet)
        {
            if (lr.enabled == true)
            {
                lr.enabled = false;
            }
        }
        else if (startPointSet && !endPointSet)
        {
            if (lr.enabled == false)
            {
                lr.enabled = true;
            }
            GetComponent<LineRenderer>().SetPositions(points);
            lr.startWidth = distLine;
            lr.endWidth = distLine;
            print("DistLine" +distLine);
        }
    }
}
