using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public GameObject parent;
    private StraightArrow fxStraightArrow;
    private DrawArrow fxDrawArrow;


    //Get start and end
    //Check is greater than 0
    // Player touches
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        InitialiseSubScripts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitialiseSubScripts()
    {
        fxStraightArrow = GetComponentInChildren<StraightArrow>();
        fxStraightArrow.enabled = false;
        fxDrawArrow = GetComponentInChildren<DrawArrow>();
    }
}
