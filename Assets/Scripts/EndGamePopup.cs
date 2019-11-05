using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pixelplacement;

public class EndGamePopup : MonoBehaviour
{
    [SerializeField] Text scoreValueText;
    [SerializeField] Text taskValueText;

    [SerializeField] AnimationCurve spawnCurve;
    [SerializeField] float spawnDuration = 1.0f;

    private void OnEnable()
    {
        float delay = 0.0f;
        Vector3 startScale = new Vector3(0, 0, 1);
        Vector3 endScale = new Vector3(1, 1, 1);
        Tween.LocalScale(this.transform, startScale, endScale, spawnDuration, delay, spawnCurve, Tween.LoopType.None);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void SetupScoreAndTaskCount(int _score, int _completedTasks)
    {
        scoreValueText.text = _score.ToString();
        taskValueText.text = _completedTasks.ToString();
    }
}
