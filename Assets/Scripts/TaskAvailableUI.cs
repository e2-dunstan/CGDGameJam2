using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class TaskAvailableUI : MonoBehaviour
{
    [SerializeField] AnimationCurve spawnCurve;

    Vector3 startScale = new Vector3(0, 0, 0);
    Vector3 endScale = new Vector3(1, 1, 1);

    private void Awake()
    {
        endScale = this.transform.localScale;
    }

    private void OnEnable()
    {
        Spawn();
    }

    void Spawn()
    {
        float duration = 1.5f;
        float delay = 0.0f;
        Tween.LocalScale(this.transform, startScale, endScale, duration, delay, spawnCurve, Tween.LoopType.None, null, LoopStatus);
    }

    void LoopStatus()
    {
        Vector3 startScale = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 endScale = new Vector3(1.1f, 1.1f, 1.1f);
        float duration = 0.5f;
        float delay = 0.0f;
        Tween.LocalScale(this.transform, startScale, endScale, duration, delay, Tween.EaseInOut, Tween.LoopType.PingPong);
    }
}
