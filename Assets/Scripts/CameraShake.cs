using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public static CameraShake Instance;

    public Vector3 lookAtPos;
    List<Vector3> startPoints = new List<Vector3>();
    List<Vector3> maxPoints = new List<Vector3>();
    List<Vector3> minPoints = new List<Vector3>();
    List<Vector3> newPoints = new List<Vector3>();
    List<Vector3> lastPoints = new List<Vector3>();
    float time = 0.0f;
    float timeout = 0.0f;
    float intense = 0.0f;



    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        //startPoint = gameObject.transform.position;
        //lastPoint = startPoint;
        ////minPoint = startPoint - new Vector3(3, 0, 1);
        ////maxPoint = startPoint + new Vector3(3, 0, 1);
        //minPoint = startPoint - gameObject.transform.right;
        //maxPoint = startPoint + gameObject.transform.right;
        ////newPoint.x = Random.Range(minPoint.x, maxPoint.x);
        ////newPoint.y = Random.Range(minPoint.y, maxPoint.y);
        //newPoint = maxPoint;
    }

    void Start()
    {
        startPoints.Add(gameObject.transform.position);
        startPoints.Add(lookAtPos);
        lastPoints.Add(startPoints[0]);
        lastPoints.Add(startPoints[1]);
        minPoints.Add(startPoints[0] - transform.right * 0.5f);
        minPoints.Add(startPoints[1] - transform.right * 0.5f);
        maxPoints.Add(startPoints[0] + transform.right * 0.5f);
        maxPoints.Add(startPoints[1] + transform.right * 0.5f);
        newPoints.Add(maxPoints[0]);
        newPoints.Add(maxPoints[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeout >= 0.0f)
        {
            Shake();
            timeout -= Time.deltaTime;
        }
        else
        {
            gameObject.transform.position = startPoints[0];
            //lookAtPos = startPoints[1];
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetIntenLength(10.0f, 0.25f);
        }
    }

    private void Shake()
    {
        if (gameObject.transform.position == newPoints[0])
        {
            lastPoints[0] = newPoints[0];
            //lastPoints[1] = newPoints[1];
            if (newPoints[0] == maxPoints[0])
            {
                newPoints[0] = minPoints[0];
                // newPoints[1] = minPoints[1];
            }
            else
            {
                newPoints[0] = maxPoints[0];
                //  newPoints[1] = maxPoints[1];
            }
            time = 0.0f;
        }
        time += (Time.deltaTime * intense);
        gameObject.transform.position = Vector3.Lerp(lastPoints[0], newPoints[0], time);
        // lookAtPos = Vector3.Lerp(lastPoints[1], newPoints[1], time);
    }

    public void SetIntenLength(float _intense = 5.0f, float _length = 0.25f)
    {
        intense = _intense;
        timeout = _length;
    }

    public Vector3 StartPoint
    {
        get { return startPoints[0]; }
        set
        {
            //startPoints[0] = value;
            //minPoints = startPoint - gameObject.transform.right;
            //maxPoint = startPoint + gameObject.transform.right;
            //newPoint = maxPoint;
        }
    }
    public Vector3 GetStartPoint(int i)
    {
        return startPoints[i];
    }
    public void SetStartPoint(int i, Vector3 sP)
    {
        startPoints[i] = sP;
        minPoints[i] = startPoints[i] - transform.right * 0.5f;
        maxPoints[i] = startPoints[i] + transform.right * 0.5f;
        newPoints[i] = maxPoints[i];
        lastPoints[i] = startPoints[i];
    }
}
