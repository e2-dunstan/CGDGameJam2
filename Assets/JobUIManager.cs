using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JobUIManager : MonoBehaviour
{
    public static JobUIManager Instance;

    [SerializeField] private GameObject uiAnchor;

    private List<GameObject> ActiveUIElements = new List<GameObject>();

    public enum UIElement
    {
        HAS_TASK = 0,
        HAS_COMPLETED_TASK = 1,
        HAS_UNWANTED_TASK = 2,
        PROGRESS_BAR = 3,
        JOB_DESCRIPTION = 4,
        JOB_ALERT = 5,
        PRESENTATION_ROOM_ALERT = 6
    }
    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public GameObject SpawnUIElement(UIElement _uiElement, GameObject _gameObject)
    {
        switch(_uiElement)
        {
            case UIElement.HAS_TASK:
                {
                    GameObject obj = Instantiate(gameObject.GetComponent<JobUIObjects>().hasJobUIElement, uiAnchor.transform);
                    //obj.transform.parent = gameObject.transform;
                    obj.GetComponent<AttachUIToGameObject>().SetTargetObject(_gameObject);
                    ActiveUIElements.Add(obj);
                    return obj;
                }
            case UIElement.HAS_COMPLETED_TASK:
                {
                    GameObject obj = Instantiate(gameObject.GetComponent<JobUIObjects>().completedJobUIElement, uiAnchor.transform);
                    //obj.transform.parent = gameObject.transform;
                    obj.GetComponent<AttachUIToGameObject>().SetTargetObject(_gameObject);
                    ActiveUIElements.Add(obj);
                    return obj;
                }
            case UIElement.PROGRESS_BAR:
                {
                    GameObject obj = Instantiate(gameObject.GetComponent<JobUIObjects>().progressBar, uiAnchor.transform);
                    //obj.transform.parent = gameObject.transform;
                    obj.GetComponent<AttachUIToGameObject>().SetTargetObject(_gameObject);
                    ActiveUIElements.Add(obj);
                    return obj;
                }
            case UIElement.JOB_DESCRIPTION:
                {
                    GameObject obj = Instantiate(gameObject.GetComponent<JobUIObjects>().jobDescription, uiAnchor.transform);
                    //obj.transform.parent = gameObject.transform;
                    obj.GetComponent<AttachUIToGameObject>().SetTargetObject(_gameObject);
                    ActiveUIElements.Add(obj);
                    return obj;
                }
            case UIElement.JOB_ALERT:
                {
                    GameObject obj = Instantiate(gameObject.GetComponent<JobUIObjects>().jobAlert, uiAnchor.transform);
                    //obj.transform.parent = gameObject.transform;
                    obj.GetComponent<AttachUIToGameObject>().SetTargetObject(_gameObject);
                    ActiveUIElements.Add(obj);
                    return obj;
                }
            case UIElement.HAS_UNWANTED_TASK:
                {
                    GameObject obj = Instantiate(gameObject.GetComponent<JobUIObjects>().jobToBeDestroyedUIElement, uiAnchor.transform);
                    //obj.transform.parent = gameObject.transform;
                    obj.GetComponent<AttachUIToGameObject>().SetTargetObject(_gameObject);
                    ActiveUIElements.Add(obj);
                    return obj;
                }
            case UIElement.PRESENTATION_ROOM_ALERT:
                {
                    GameObject obj = Instantiate(gameObject.GetComponent<JobUIObjects>().presentationRoomAlert, uiAnchor.transform);
                    //obj.transform.parent = gameObject.transform;
                    obj.GetComponent<AttachUIToGameObject>().SetTargetObject(_gameObject);
                    ActiveUIElements.Add(obj);
                    return obj;
                }
        }

        return null;
    }

    public void RemoveUIElementFromObject(UIElement _uIElement, GameObject _gameObject)
    {
        foreach(var obj in ActiveUIElements)
        {
            if(obj.GetComponent<AttachUIToGameObject>().GetTargetObject().GetInstanceID() == _gameObject.GetInstanceID())
            {
                List<GameObject> tempList = ActiveUIElements.Where(x => x.GetInstanceID() != _gameObject.GetInstanceID()).ToList();
                ActiveUIElements = tempList;
                Destroy(obj);
            }
        }
    }
}
