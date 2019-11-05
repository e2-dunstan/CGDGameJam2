using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    private float speed = 100;

    void Update()
    {
        transform.eulerAngles += new Vector3(0, 0, Time.deltaTime * speed);
    }
}
