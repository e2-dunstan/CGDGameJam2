using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightArrow : MonoBehaviour
{
    public Vector3 startloc;
    public Vector3 endLoc;

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
        if (pathParticleInst != null && pathParticleInst.activeSelf)
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
        if (!pathParticleInst.activeSelf)
        {
            pathParticleInst.transform.position = startloc;
            pathPos = 0.0f;
            pathParticleInst.SetActive(true);

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
        pathParticleInst.transform.position = Vector3.Lerp(startloc, endLoc, pathPos);

    }

    public void Kill()
    {
        if (pathParticleInst.activeSelf)
        {
            //Destroy(pathParticleInst);
            pathParticleInst.SetActive(false);
            pathPos = 0.0f;
        }
    }

}
