using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightArrow : MonoBehaviour
{
    //Line point values
    public Vector3 startloc;
    public Vector3 endLoc;
    private int position = 0;
    public List<Vector3> path;
    public GameObject target;

    // Components
    public GameObject pathParticle;
    private GameObject pathParticleInst;
    private ParticleSystem instPS;
    ParticleSystem.SizeOverLifetimeModule sizeOL;
    ParticleSystem.ShapeModule shapeOL;

    //Duration and checks
    [SerializeField] private float pathPos;
    [SerializeField] float timer;
    float defaultMultiplier;
    bool pulsed;
    public bool instanceActive = false;
    public bool reachedLastPoint = false;
    private bool reachedNextPoint = false;
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
            pathParticleInst = Instantiate(pathParticle, target.transform);
        }

        reachedLastPoint = (Vector3.Distance(target.transform.position, endLoc) < 0.5f);
        Debug.Log(reachedLastPoint);
        enabled = !reachedLastPoint;

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

        if (path.Count > 0)
        {
            if ((Vector3.Distance(pathParticleInst.transform.position, path[position + 1]) < 0.1f))
            {
                position++;
                pathParticleInst.transform.position = startloc;
            }
            //Move along the path
            pathParticleInst.transform.position = Vector3.Lerp(startloc, path[(position + 1)], pathPos);
        }
    }

    public void Kill()
    {
        if (pathParticleInst.activeSelf)
        {
            transform.parent = null;
            instPS.Stop();
            sizeOL.sizeMultiplier = defaultMultiplier;
            timer = instPS.sizeOverLifetime.sizeMultiplier;
            target = null;
            pathPos = 0.0f;
            pathParticleInst.SetActive(false);
            position = 0;
            path.Clear();
            path = new List<Vector3>();
        }
    }

}
