using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JobUIManager : MonoBehaviour
{
    public static JobUIManager Instance;

    private List<GameObject> ActiveUIElements = new List<GameObject>();

    public enum UIElement
    {
        HAS_TASK = 0,
        HAS_COMPLETED_TASK = 1,
        PROGRESS_BAR = 2
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


    public void SpawnUIElement(UIElement _uiElement, GameObject _gameObject)
    {
        switch(_uiElement)
        {
            case UIElement.HAS_TASK:
                {
                    GameObject obj = Instantiate(gameObject.GetComponent<JobUIObjects>().hasJobUIElement);
                    obj.transform.parent = gameObject.transform;
                    obj.GetComponent<AttachUIToGameObject>().SetTargetObject(_gameObject);
                    ActiveUIElements.Add(obj);
                    break;
                }
            case UIElement.PROGRESS_BAR:
                {
                    GameObject obj = Instantiate(gameObject.GetComponent<JobUIObjects>().progressBar);
                    obj.transform.parent = gameObject.transform;
                    obj.GetComponent<AttachUIToGameObject>().SetTargetObject(_gameObject);
                    ActiveUIElements.Add(obj);
                    break;
                }
        }
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
