using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    Vector3 startPoint;
    Vector3 minPoint;
    Vector3 maxPoint;
    Vector3 lastPoint;
    Vector3 newPoint;
    float time = 0.0f;
    float timeout = 0.0f;
    public float count = 0.0f;
    public float intense = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        startPoint = gameObject.transform.position;
        lastPoint = startPoint;
        //minPoint = startPoint - new Vector3(3, 0, 1);
        //maxPoint = startPoint + new Vector3(3, 0, 1);
        minPoint = startPoint - gameObject.transform.right;
        maxPoint = startPoint + gameObject.transform.right;
        //newPoint.x = Random.Range(minPoint.x, maxPoint.x);
        //newPoint.y = Random.Range(minPoint.y, maxPoint.y);
        newPoint = maxPoint;
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
            gameObject.transform.position = startPoint;
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    timeout = count;
        //}
        //Pixelplacement.Tween.Value();
    }

    private void Shake()
    {

        //Debug.Log("MinPoint: " + minPoint);
        //Debug.Log("MaxPoint: " + maxPoint);
        // Debug.Log("NewPoint: " + newPoint);
        //Debug.Log("LastPoint: " + lastPoint);
        if (gameObject.transform.position == newPoint)
        {
            lastPoint = newPoint;
            if (newPoint == maxPoint)
            {
                newPoint = minPoint;
                Debug.Log("NewPoint: " + newPoint);
                Debug.Log("maxPoint: " + maxPoint);
            }
            else
            {
                newPoint = maxPoint;
                Debug.Log("NewPoint: " + newPoint);
                Debug.Log("minPoint: " + minPoint);
            }
            time = 0.0f;
        }
        time += (Time.deltaTime * intense);
        gameObject.transform.position = Vector3.Lerp(lastPoint, newPoint, time);
    }

    public void Shake(float _intense = 5.0f, float _length = 0.25f)
    {
        intense = _intense;
        timeout = _length;
    }
}
