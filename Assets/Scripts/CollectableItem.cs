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

    private bool hasBeenCollected = false;

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
        if(other.gameObject.CompareTag("Employee") && !hasBeenCollected)
        {
            hasBeenCollected = true;
            gameObject.transform.position = other.GetComponent<Employee>().GetItemHoldTransform().transform.position;
            gameObject.transform.parent = other.GetComponent<Employee>().GetItemHoldTransform().transform.parent;
            gameObject.transform.rotation = other.GetComponent<Employee>().GetItemHoldTransform().transform.rotation;
        }
    }
}
