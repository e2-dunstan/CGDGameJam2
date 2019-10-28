using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraPos : MonoBehaviour
{
    CameraShake camShake;
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
        camShake = GetComponent<CameraShake>();
        //var objects = GameObject.FindObjectsOfType<Employee>();
        employees.AddRange(GameObject.FindObjectsOfType<Employee>());
        dif.Add(0.0f);
        dif.Add(0.0f);
        val.Add(0.0f);
        val.Add(0.0f);
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

    // Update is called once per frame
    void Update()
    {

        minXPos = employees.Min(em => em.transform.position.x);
        maxXPos = employees.Max(em => em.transform.position.x);
        minZPos = employees.Min(em => em.transform.position.z);
        maxZPos = employees.Max(em => em.transform.position.z);

        if (minXPos < 0)
        {
            if(maxXPos < 0)
            {
                dif[0] = (minXPos - maxXPos) * -1;
                Debug.Log(dif[0]);
                val[0] = dif[0];
                dif[0] = maxXPos - dif[0] * 0.5f;
            }
            else
            {
                dif[0] = maxXPos - minXPos;
                Debug.Log(dif[0]);
                val[0] = dif[0];
                dif[0] = maxXPos - dif[0] * 0.5f;
            }
        }
        else
        {
            dif[0] = maxXPos - minXPos;
            Debug.Log(dif[0]);
            val[0] = dif[0];
        }

        if (minZPos < 0)
        {
            if (maxZPos < 0)
            {
                dif[1] = (minZPos - maxZPos) * -1;
                Debug.Log(dif[0]);
                val[1] = dif[1];
                dif[1] = maxZPos - dif[1] * 0.5f;
            }
            else
            {
                dif[1] = maxZPos - minZPos;
                Debug.Log(dif[1]);
                val[1] = dif[1];
                dif[1] = maxZPos - dif[1] * 0.5f;
            }
        }
        else
        {
            dif[1] = maxZPos - minZPos;
            Debug.Log(dif[1]);
            val[1] = dif[1];
        }

        yVal = (val[0] + val[1]) * 0.5f;
        Vector3 pos = new Vector3(dif[0], yVal, dif[1]);
        camShake.StartPoint = pos;
    }
}
