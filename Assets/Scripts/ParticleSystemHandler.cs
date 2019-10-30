using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemHandler : MonoBehaviour
{
    public static ParticleSystemHandler Instance;

    [SerializeField] ParticleSystem taskCompleteParticle;

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
}
