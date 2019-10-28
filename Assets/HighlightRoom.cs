using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class HighlightRoom : MonoBehaviour
{

    Outline outline;
    bool hover = true;
    float minWidth = 0.0f;
    float maxWidth = 10.0f;
    Pixelplacement.TweenSystem.TweenBase tween;

    // Start is called before the first frame update
    void Awake()
    {
        outline = GetComponent<Outline>();
        tween = Tween.Value(minWidth, maxWidth, SetWidth, 1.0f, 0, Pixelplacement.Tween.EaseInOut, Pixelplacement.Tween.LoopType.PingPong);
        tween.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (hover)
        {
            if(tween.Status != Tween.TweenStatus.Running)
            {
                tween.Start();
            }
        }
        else
        {
            if (tween.Status == Tween.TweenStatus.Running)
            {
                tween.Stop();
                SetWidth(0.0f);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            hover = !hover;
        }
    }

    void SetWidth(float width)
    {
        outline.OutlineWidth = width;
    }
}
