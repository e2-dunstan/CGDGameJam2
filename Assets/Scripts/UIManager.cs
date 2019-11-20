using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Job job;

    // Start is called before the first frame update
    private void Start()
    {
        job = JobManager.Instance.GetRandomInactiveJobAndAddToQueue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
