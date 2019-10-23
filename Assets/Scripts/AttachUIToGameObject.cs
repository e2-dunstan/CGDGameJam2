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
}
