using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimeManager : MonoBehaviour
{
    //Singleton Instance
    public static GameTimeManager Instance;

    [Header("References")]
    [SerializeField] private TimerProgressBar timerObject;

    [Header("Timer Settings")]
    [SerializeField] private float gameDuration = 120.0f;
    [SerializeField] private bool timerPaused = true;
    [SerializeField] private string sceneName = "MainMenu";

    private float timer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    void Start()
    {
        ResetTimer();
        timerPaused = false;
    }

    void Update()
    {
        if (!timerPaused)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                TogglePaused();
                timer = 0;
                TimerComplete();
            }
            timerObject.UpdateTimer(gameDuration, timer);
        }
    }

    public void ResetTimer()
    {
        timer = gameDuration;
    }

    public void SetTimer(float time)
    {
        timer = time;
    }

    public void TogglePaused()
    {
        timerPaused = !timerPaused;
    }

    private void TimerComplete()
    {
        //TODO add calls here to end the game
        Debug.Log("Game over has been called");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}