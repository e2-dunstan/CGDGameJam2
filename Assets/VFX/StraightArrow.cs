using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightArrow : MonoBehaviour
{
    //Line point values
    public Vector3 startloc;
    public Vector3 endLoc;
    public List<Vector3> path;
    public GameObject target;

    // Components
    public GameObject pathParticle;
    private GameObject pathParticleInst;
    private ParticleSystem instPS;
    ParticleSystem.SizeOverLifetimeModule sizeOL;
    ParticleSystem.ShapeModule shapeOL;
    ParticleSystem.MainModule mOL;

    //Duration and checks
    [SerializeField] private float pathPos;
    [SerializeField] float timer;
    float defaultMultiplier;
    bool pulsed;
    public bool instanceActive = false;
    public bool reached = false;

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
            instanceActive = true;
            instPS = pathParticleInst.GetComponent<ParticleSystem>();
            timer = instPS.sizeOverLifetime.sizeMultiplier;
            sizeOL = instPS.sizeOverLifetime;
            shapeOL = instPS.shape;
            mOL = instPS.main;

            if (defaultMultiplier == 0)
            {
                defaultMultiplier = instPS.sizeOverLifetime.sizeMultiplier;
            }
        }
        else
        {
            instanceActive = false;
        }

        if (pathPos >= 1.35f)
        {
            pathPos = 1.35f;
        }

        if (target != null)
        {
            startloc = target.transform.position;
            if (pathParticleInst != null)
            {
                pathParticleInst.SetActive(true);
                pathParticleInst.transform.LookAt(endLoc, Vector3.up);
            }
            if (!instPS.isPlaying)
            {
                instPS.Play();
            }
        }
        else
        {
            Debug.Log("No Target!!!");
        }
    }

    private void FixedUpdate()
    {

        if (pathParticleInst == null)
        {
            pathParticleInst = Instantiate(pathParticle, transform);
        }

        reached = (Vector3.Distance(startloc, endLoc) < 0.5f);
        Debug.Log(reached);
        enabled = !reached;

        if (pathPos < 1.35f)
        {
            pathPos += 0.035f;
        }
        if (pathPos >= 1.35f)
        {
            timer -= 0.05f;
            sizeOL.sizeMultiplier = timer;
            mOL.duration = timer;
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
            sizeOL.sizeMultiplier = defaultMultiplier;
            timer = instPS.sizeOverLifetime.sizeMultiplier;
            target = null;
            pathParticleInst.transform.position = Vector3.zero;

            pathPos = 0.0f;
            pathParticleInst.SetActive(false);
        }
    }

}
