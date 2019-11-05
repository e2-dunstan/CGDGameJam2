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

    private GameObject parentGameobject = null;
    [SerializeField] private MeshRenderer objMesh = null;
    [SerializeField] private GameObject objParticleSystem = null;

    private Color particleColour;

    public GameObject spawnPosition;

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
            if (other.gameObject.GetComponent<Employee>().hasItem == false)
            {
                ItemManager.Instance.SetSpawnPositionToActive(spawnPosition);
                objParticleSystem.gameObject.SetActive(false);
                objMesh.enabled = false;
                parentGameobject = other.gameObject;
                other.gameObject.GetComponent<Employee>().hasItem = true;
                ActivateEffect();
                hasBeenCollected = true;
                gameObject.transform.position = other.GetComponent<Employee>().GetItemHoldTransform().transform.position;
                gameObject.transform.parent = other.GetComponent<Employee>().GetItemHoldTransform().transform.parent;
                gameObject.transform.rotation = other.GetComponent<Employee>().GetItemHoldTransform().transform.rotation;
            }
        }
    }

    private void ActivateEffect()
    {
        parentGameobject.GetComponent<Employee>().SetPickupParticleActiveState(true);
        parentGameobject.GetComponent<Employee>().SetPickupParticleColour(particleColour);
    }

    public void DeactiveEffect()
    {
        parentGameobject.GetComponent<Employee>().SetPickupParticleActiveState(false);
        parentGameobject.GetComponent<Employee>().hasItem = false;
    }

    public void SetParticleColour(Color _color)
    {
        particleColour = _color;
        objParticleSystem.GetComponent<ParticleSystem>().startColor = _color;
    }
}
