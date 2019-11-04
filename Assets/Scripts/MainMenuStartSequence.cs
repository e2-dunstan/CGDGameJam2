using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pixelplacement;

public class MainMenuStartSequence : MonoBehaviour
{
    [SerializeField] string sceneName;
    [Header("Tween Elements")]
    [SerializeField] Color logoEndColour;
    [SerializeField] Color frenzyEndColour;
    [SerializeField] Color textEndColour;

    [Header("Menu Elements")]
    [SerializeField] Image foundaryLogo;
    [SerializeField] Text frenzy;
    [SerializeField] Text touchToStart;

    private float delay = 0;

    private void Awake()
    {
        delay += 0.5f;
        Tween.Color(foundaryLogo, logoEndColour, 1.5f, delay, Tween.EaseBounce);

        delay += 1.0f;

        Vector3 frenzyStartSize = new Vector3(2.0f, 2.0f, 1.0f);
        Vector3 frenzyEndSize = new Vector3(1.0f, 1.0f, 1.0f);
        Tween.Color(frenzy, frenzyEndColour, 1.5f, delay, Tween.EaseOut);
        Tween.LocalScale(frenzy.transform, frenzyStartSize, frenzyEndSize, 1.5f, delay, Tween.EaseOut, Tween.LoopType.None, null, StartPulsingText);
    }

    void StartPulsingText()
    {
        Vector3 textStartSize = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 textEndSize = new Vector3(1.1f, 1.1f, 1.0f);

        Tween.Color(touchToStart, textEndColour, 0.5f, 0, Tween.EaseOut);
        Tween.LocalScale(touchToStart.transform, textStartSize, textEndSize, 0.5f, 0, Tween.EaseInOut, Tween.LoopType.PingPong);
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}