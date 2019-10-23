using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArrow : MonoBehaviour
{
    private StraightArrow anim;
    [SerializeField] private GameObject startloc;
    [SerializeField] private GameObject endLoc;
    [SerializeField] private bool onClick;
    [SerializeField] private bool startPointSet;
    [SerializeField] private bool endPointSet;
    public bool validMove;
    Vector3[] points = new Vector3[2];
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
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
            print("Set end position!! " + startloc.transform.position);
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
        points[0] = startloc.transform.position;
        points[1] = endLoc.transform.position;
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
        }
    }

    private void Debug()
    {
        //Vector3 heading = (endLoc.transform.position - startloc.transform.position);
        //float distance = heading.magnitude;
        //Vector3 direction = heading / distance;
        //Debug.DrawRay(startloc.transform.position, direction * distance, Color.black);

    }
}
