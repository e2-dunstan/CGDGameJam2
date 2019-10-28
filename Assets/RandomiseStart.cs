using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomiseStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 vec = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
        gameObject.transform.position = vec;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
