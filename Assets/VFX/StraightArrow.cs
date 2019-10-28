using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightArrow : MonoBehaviour
{
    [SerializeField] private GameObject startloc;
    [SerializeField] private GameObject endLoc;

    public GameObject pathParticle;
    private GameObject pathParticleInst;
    [SerializeField] private float pathPos;
    // Start is called before the first frame update
    void Start()
    {
        pathPos = 0.0f;
    }   

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
            if (pathParticleInst == null)
            {
                pathParticleInst = Instantiate(pathParticle);
            }
            pathParticleInst.transform.position = Vector3.Lerp(startloc.transform.position, endLoc.transform.position, pathPos);
            pathPos += 0.035f;
            if (pathPos >= 1.35f)
            {
                Destroy(pathParticleInst);
                pathPos = 0.0f;
            }
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
