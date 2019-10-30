using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightArrow : MonoBehaviour
{
    public Vector3 startloc;
    public Vector3 endLoc;
    public GameObject target;
    // Original particle and instance
    public GameObject pathParticle;
    private GameObject pathParticleInst;
    private ParticleSystem instPS;
    ParticleSystem.SizeOverLifetimeModule sizeOL;
    ParticleSystem.ShapeModule shapeOL;
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
        defaultMultiplier = instPS.sizeOverLifetime.sizeMultiplier;
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
            pathParticleInst.transform.LookAt(endLoc, Vector3.up);


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
            pathParticleInst = Instantiate(pathParticle, target.transform);
        }
        if (!pathParticleInst.activeSelf)
        {
            pathParticleInst.transform.position = startloc;
            pathPos = 0.0f;
            pathParticleInst.SetActive(true);

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
            sizeOL.sizeMultiplier = defaultMultiplier;
            timer = instPS.sizeOverLifetime.sizeMultiplier;

            pathPos = 0.0f;
            pathParticleInst.SetActive(false);
        }
    }

}
