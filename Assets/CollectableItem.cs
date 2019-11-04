using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public enum CollectableType
    {
        CONTROLLER = 0,
        KEYBOARD = 1,
        ARCADE_CABINET = 2
    }

    [SerializeField]
    private GameObject UIELement = null;

    public CollectableType collectableType = CollectableType.CONTROLLER;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Employee"))
        {
            gameObject.transform.parent = other.transform;
        }
    }
}
