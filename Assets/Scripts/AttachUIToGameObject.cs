using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttachUIToGameObject : MonoBehaviour
{
    [SerializeField] GameObject targetObject;

    private void LateUpdate()
    {
        Vector3 UIposition = Camera.main.WorldToScreenPoint(targetObject.transform.position);
        this.transform.position = UIposition;
    }

    public void SetTargetObject(GameObject _obj)
    {
        targetObject = _obj;
    }

    public GameObject GetTargetObject()
    {
        return targetObject;
    }
}
