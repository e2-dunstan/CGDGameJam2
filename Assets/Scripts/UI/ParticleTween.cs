using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTween : MonoBehaviour
{
    private float duration = 1.0f;
    private RectTransform[] particles;

    void Start()
    {
        particles = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform particle in particles)
        {
            particle.gameObject.SetActive(false);
        }
    }

    private void TweenParticles(Vector3 from, Vector3 to)
    {
        foreach (RectTransform particle in particles)
        {
            particle.localScale = Vector3.zero;
            particle.gameObject.SetActive(true);

            StartCoroutine(TweenCoroutine(particle, from, to));
        }
    }

    private IEnumerator TweenCoroutine(RectTransform particle, Vector3 from, Vector3 to)
    {
        float delay = Random.Range(0f, 100f) / 100f;
        particle.position = from;

        yield return new WaitForSeconds(delay);
        for(float t = 0.0001f; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;

            particle.position = Vector3.Slerp(from, to, progress);
            if (progress < 0.5f)
            {
                particle.localScale = Vector3.Slerp(Vector3.zero, Vector3.one * 1.5f, progress * 2);
            }
            else
            {
                particle.localScale = Vector3.Slerp(Vector3.one * 1.5f, Vector3.zero, (progress - 0.5f) * 2);
            }

            yield return null;
        }
        particle.position = to;
        particle.gameObject.SetActive(false);
        particle.localScale = Vector3.one;
    }
}
