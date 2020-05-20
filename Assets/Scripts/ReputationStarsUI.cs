using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

public class ReputationStarsUI : MonoBehaviour
{
    [System.Serializable]
    struct star
    {
        public Transform transform;
        public bool active;
    }

    [SerializeField] star[] starArray;
    public Slider slider;

    private void Start()
    {
        SetReputation(0);
        slider.value = 0;
    }

    public void SetReputation(int reputation)
    {
        for (int i = 0; i < starArray.Length; i++)
        {
            SetStarActive(i, i < reputation);
        }
    }

    void SetStarActive(int index, bool active)
    {
        if(active != starArray[index].active)
        {
            starArray[index].active = active;

            //Stop all tweens on star
            Tween.Stop(starArray[index].transform.GetInstanceID(), Tween.TweenType.LocalScale);

            if (active)
            {
                Vector3 startScale = new Vector3(0, 0, 0);
                Vector3 endScale = new Vector3(1, 1, 1);
                float duration = 0.75f;
                float delay = 0.0f;

                Tween.LocalScale(starArray[index].transform, startScale, endScale, duration, delay, Tween.EaseOutBack, Tween.LoopType.None);
            }
            else
            {
                Vector3 startScale = new Vector3(1, 1, 1);
                Vector3 endScale = new Vector3(0, 0, 0);
                float duration = 0.75f;
                float delay = 0.0f;

                Tween.LocalScale(starArray[index].transform, startScale, endScale, duration, delay, Tween.EaseInBack, Tween.LoopType.None);
            }
        }
    }

    public Vector3 GetPosition()
    {
        return starArray[0].transform.position;
    }
}
