using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class FlashOutline : MonoBehaviour
{
    private Outline outline;


    void Start()
    {
        outline = GetComponent<Outline>();
    }

    public void Flash(float duration, int totalFlashes)
    {
        StartCoroutine(FlashCoroutine(duration, totalFlashes));
    }

    private IEnumerator FlashCoroutine(float duration, int totalFlashes)
    {
        if (duration <= 0 || totalFlashes <= 0) Debug.LogError("Flash Outline variables must be greater than 0");

        float timeBetweenFlashes = (float)duration / (float)totalFlashes;
        int currentFlash = 0;

        while(currentFlash < totalFlashes)
        {
            outline.enabled = !outline.enabled;
            currentFlash++;
            yield return new WaitForSeconds(timeBetweenFlashes);
        }
        outline.enabled = false;
    }
}
