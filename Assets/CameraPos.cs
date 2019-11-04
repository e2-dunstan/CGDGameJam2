using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraPos : MonoBehaviour
{
    CameraShake camShake;
    TouchInput input;
    bool focus = false;
    float turnSpeedX = 4.0f;
    float turnSpeedY = 4.0f;
    Vector3 lookAtPos = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 offset = new Vector3(0.0f, 25.0f, -15.0f);
    float dist, maxDist;
    Vector3 camPos = new Vector3(0.0f, 0.0f, 0.0f);

    const float RADIAN = 57.29577951f;

    float angleX = 90;
    float angleY = 90;
    float angleZ = 90;

    List<Employee> employees = new List<Employee>();
    List<float> dif = new List<float>();
    List<float> val = new List<float>();
    float yVal = 0.0f;
    float minXPos;
    float maxXPos;
    float minZPos;
    float maxZPos;

    // Start is called before the first frame update
    void Awake()
    {
        offset = new Vector3(lookAtPos.x, lookAtPos.y + 20.0f, lookAtPos.z - 10.0f);
        //offset2 = new Vector3(lookAtPos.x, lookAtPos.y, lookAtPos.z - 15.0f);
        camShake = GetComponent<CameraShake>();
        camShake.lookAtPos = new Vector3(0.0f, 0.0f, 0.0f);
        //transform.position = camShake.lookAtPos + offset;
        dist = Vector3.Distance(transform.position, camShake.lookAtPos);
        maxDist = dist;
        //camShake.StartPoint = lookAtPos;
        //var objects = GameObject.FindObjectsOfType<Employee>();
        employees.AddRange(GameObject.FindObjectsOfType<Employee>());
        dif.Add(0.0f);
        dif.Add(0.0f);
        val.Add(0.0f);
        val.Add(0.0f);
        //camShake.SetStartPoint(0, camShake.lookAtPos + offset);
        // Vector3 x = new Vector3(0, 0, 0);

        //for(int i  = 0; i < employees.Count; ++i)
        //{
        //    for (int j = 0; j < employees.Count; ++j)
        //    {
        //        if(i != j)
        //        {
        //            employees[i].transform.position.x;
        //        }
        //    }
        //}

    }

    void Start()
    {
        input = TouchInput.Instance;

    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (!focus)
        {
            //var count = input.playerTouches.Count;
            //for (int i = 0; i < count; ++i)
            //{
            //    if (!input.playerTouches[i].selectedChar && input.playerTouches[i].tracking)
            //    {
            //        if (offset.y < 13.0f)
            //            offset.y = 13.0f;
            //        //var mouseX = Input.GetAxis("Mouse X");
            //        //var mouseY = Input.GetAxis("Mouse Y");
            //        //offset = Quaternion.AngleAxis(mouseX * turnSpeedX, Vector3.up) * offset;
            //        //offset = Quaternion.AngleAxis(mouseY * turnSpeedY, Vector3.right) * offset;
            //        var mouseX = input.playerTouches[i].touchEnd.x - input.playerTouches[i].touchStart.x;
            //        var mouseY = input.playerTouches[i].touchEnd.y - input.playerTouches[i].touchStart.y;
            //        Debug.Log(input.playerTouches[i].touchStart);
            //        Debug.Log(input.playerTouches[i].touchEnd);
            //        if (mouseX > 0)
            //            Debug.Log(mouseX);
            //        if (mouseY > 0)
            //            Debug.Log(mouseY);
            //        offset = Quaternion.AngleAxis(mouseX, Vector3.up) * offset;
            //        offset = Quaternion.AngleAxis(mouseY, Vector3.right) * offset;
            //        dist = Vector3.Distance(transform.position, lookAtPos);
            //        if (dist > maxDist)
            //        {
            //            offset.Normalize();
            //            offset *= maxDist;
            //            // transform.position = lookAtPos + offset;
            //            transform.position = Vector3.Lerp(transform.position, lookAtPos + offset, 0.05f);
            //        }
            //        else
            //        {
            //            camShake.StartPoint = lookAtPos + offset;
            //            // transform.position = Vector3.Lerp(transform.position, lookAtPos + offset, 0.25f);
            //        }
            //    }
            //    transform.LookAt(lookAtPos);
            //}

            //if (Input.GetMouseButton(0))
            //{
            //    if (offset.y < 13.0f)
            //        offset.y = 13.0f;

            //    var mouseX = Input.GetAxis("Mouse X");
            //    var mouseY = Input.GetAxis("Mouse Y");
            //    offset = Quaternion.AngleAxis(mouseX * turnSpeedX, Vector3.up) * offset;
            //    offset = Quaternion.AngleAxis(mouseY * turnSpeedY, Vector3.right) * offset;
            //    dist = Vector3.Distance(transform.position, camShake.lookAtPos);

            //    if (dist > maxDist)
            //    {
            //        offset.Normalize();
            //        offset *= maxDist;
            //        // transform.position = lookAtPos + offset;
            //        camShake.SetStartPoint(0, Vector3.Lerp(camShake.GetStartPoint(0), lookAtPos + offset, 0.05f));
            //        camShake.SetStartPoint(1, camShake.lookAtPos);
            //    }
            //    else
            //    {
            //        camShake.SetStartPoint(0, Vector3.Lerp(camShake.GetStartPoint(0), lookAtPos + offset, 0.25f));
            //        camShake.SetStartPoint(1, camShake.lookAtPos);
            //        // transform.position = Vector3.Lerp(transform.position, lookAtPos + offset, 0.25f);
            //    }
            //}
            camShake.SetStartPoint(0, camShake.lookAtPos + offset);
            camShake.SetStartPoint(1, camShake.lookAtPos);
            transform.LookAt(camShake.lookAtPos);
        }
    }
}
