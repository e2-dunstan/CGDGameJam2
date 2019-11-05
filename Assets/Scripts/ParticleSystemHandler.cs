using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemHandler : MonoBehaviour
{
    public static ParticleSystemHandler Instance;

    [SerializeField] ParticleSystem taskCompleteParticle;
    [SerializeField] ParticleSystem taskSubmitStarParticle;
    [SerializeField] ParticleSystem taskSubmitSparkParticle;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    public void EmitTaskCompleteParticle(Vector3 position)
    {
        taskCompleteParticle.gameObject.transform.position = position;
        taskCompleteParticle.Play();
    }

    public void EmitTaskSubmitParticle(Vector3 position)
    {
        taskSubmitStarParticle.gameObject.transform.position = position;
        taskSubmitSparkParticle.gameObject.transform.position = position;
        taskSubmitStarParticle.Play();
        taskSubmitSparkParticle.Play();
    }
}
