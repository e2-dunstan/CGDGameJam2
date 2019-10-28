using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightArrow : MonoBehaviour
{
    [SerializeField] private GameObject startloc;
    [SerializeField] private GameObject endLoc;

    // Original particle and instance
    public GameObject pathParticle;
    private GameObject pathParticleInst;
    private ParticleSystem instPS;
    ParticleSystem.SizeOverLifetimeModule sizeOL;
    ParticleSystem.ShapeModule shapeOL;
    [SerializeField] private float pathPos;
    [SerializeField] float timer;
    bool pulsed;
    // Start is called before the first frame update
    void Start()
    {
        pathPos = 0.0f;
    }   

    // Update is called once per frame
    void Update()
    {
        if (pathParticleInst != null)
        {
            instPS = pathParticleInst.GetComponent<ParticleSystem>();
            timer = instPS.sizeOverLifetime.sizeMultiplier;
            sizeOL = instPS.sizeOverLifetime;
            shapeOL = instPS.shape;
        }

        if (pathPos >= 1.35f)
        {
            pathPos = 1.35f;
        }
    }

    private void FixedUpdate()
    {

        if (pathParticleInst == null)
        {
            pathParticleInst = Instantiate(pathParticle);
        }
            
        if (pathPos < 1.35f)
        {
            pathPos += 0.035f;
        }
        if (pathPos >= 1.35f)
        {
            timer -= 0.05f;
            sizeOL.sizeMultiplier = timer;
            if (timer < 0.05f)
            {
                Kill();
            }
        }
        //Move along the path
        pathParticleInst.transform.position = Vector3.Lerp(startloc.transform.position, endLoc.transform.position, pathPos);

    }

    public void Kill()
    {
        if (pathParticleInst != null)
        {
            Destroy(pathParticleInst);
            pathPos = 0.0f;
        }
    }

}
