using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEmitter : MonoBehaviour
{
    //  Parent
    public Employee parentEmployee;
    private Transform parentTransform;

    //  Line positions
    public Vector3 startPosition;
    public Vector3 endPosition;

    //  Path checks
    [SerializeField] private List<Vector3> path;
    public int pathPoint;
    private int lengthOfPath;

    //  Components
    [SerializeField] private GameObject pathPrefab;
    private GameObject pathInstance;

    //  Particle system modules
    private ParticleSystem pathPS;
    private ParticleSystem.SizeOverLifetimeModule pathPS_SizeOL;

    //  Misc
    [SerializeField] private float pulseTimer;


    // Start is called before the first frame update
    private void Start()
    {
        parentTransform = parentEmployee.transform;
        pathInstance = Instantiate(pathPrefab);
    }

    private void OnEnable()
    {
        if (CheckVariablesInitialised())
        {
            ResetValues();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckVariablesInitialised()
    {
        if (startPosition == Vector3.zero || endPosition == Vector3.zero ||
            path.Count == 0 || pathPrefab == null)
        {
            Debug.LogWarning("Make sure all parameters for PathEmitter are set inside VFXController!");

            if (startPosition == Vector3.zero)
            {
                Debug.LogWarning("PathEmitter Start position is not set!");
                return false;
            }
            if (endPosition == Vector3.zero)
            {
                Debug.LogWarning("PathEmitter End position is not set!");
                return false;
            }
            if (path.Count == 0)
            {
                Debug.LogWarning("Path points are not set!");
                return false;
            }
            if (pathPrefab == null)
            {
                Debug.LogWarning("Path prefab not set!");
                return false;
            }
            return false;
        }
        return true;
    }
    private void ResetValues()
    {
        //If the instance has not been created, create it
        if (pathInstance == null)
        {
            pathInstance = Instantiate(pathPrefab);
        }
        transform.position = parentTransform.position;
        pulseTimer = 0.25f;
    }
}
