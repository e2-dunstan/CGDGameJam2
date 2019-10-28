using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    private StraightArrow fxStraightArrow;
    private DrawArrow fxDrawArrow;
    //Get start and end
    //Check is greater than 0
    // Player touches
    // Start is called before the first frame update
    private void Awake()
    {
        InitialiseSubScripts();
    }

    // Update is called once per frame
    void Update()
    {
        DrawArrowManage();
        List<TouchInput.PlayerTouch> touches = TouchInput.Instance.GetCurrentTouches();
        if (touches.Count >= 0)
        {
            foreach (TouchInput.PlayerTouch touch in touches)
            {
                fxDrawArrow.startloc = touch.touchStart;
            }
        }
    }

    void InitialiseSubScripts()
    {
        fxStraightArrow = GetComponent<StraightArrow>();
        fxStraightArrow.enabled = false;
        fxDrawArrow = GetComponent<DrawArrow>();
    }

    void DrawArrowManage()
    {
        if (fxDrawArrow.killPS)
        {
            fxStraightArrow.Kill();
        }

        if (fxDrawArrow.startPs)
        {
            fxStraightArrow.enabled = true;
        }
        else
        {
            fxStraightArrow.enabled = false;
        }

    }
}
