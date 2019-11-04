using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public static VFXController Instance;

    private StraightArrow fxStraightArrow;
    private DrawArrow fxDrawArrow;

    public GameObject pathIndicatorPrefab;
    public ParticleSystem puff;

    public class PathIndicator
    {
        public GameObject instance;
        public DrawArrow drawArrow;
        public StraightArrow straightArrow;
        public TouchInput.PlayerTouch touchRef = null;
    }
    private List<PathIndicator> pathIndicators = new List<PathIndicator>();
    private List<ParticleSystem> puffList = new List<ParticleSystem>();



    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {

        for(int i = 0; i < TouchInput.Instance.MaxTouches; i++)
        {
            //Initialise a new path indicator on start
            PathIndicator pathIndicator = new PathIndicator();
            //Instantiate the prefab
            pathIndicator.instance = Instantiate(pathIndicatorPrefab, transform);
            //Assign script references
            pathIndicator.drawArrow = pathIndicator.instance.GetComponent<DrawArrow>();
            pathIndicator.straightArrow = pathIndicator.instance.GetComponent<StraightArrow>();
            pathIndicator.straightArrow.enabled = false;

            //Assign touch reference
            pathIndicator.touchRef = TouchInput.Instance.playerTouches[i];

            //Disable the object by default
            pathIndicator.instance.SetActive(false);

            pathIndicators.Add(pathIndicator);
        }
        CreateParticleSystemForAllEmployees(puff, puffList);

        //This will need modifying to allow for the path indicators
    }

    // Update is called once per frame
    void Update()
    {
        DrawArrowManage();
        
    }

    void InitialiseSubScripts()
    {
        fxStraightArrow = GetComponent<StraightArrow>();
        fxDrawArrow = GetComponent<DrawArrow>();
    }

    void DrawArrowManage()
    {
        ArrowCommands();

        for (int i = 0; i < pathIndicators.Count; i++)
        {
            if (!pathIndicators[i].touchRef.tracking)
            {
                //If the startpoint is set, trigger the particle system
                if (pathIndicators[i].drawArrow.startPointSet)
                {
                    pathIndicators[i].drawArrow.endPointSet = true;
                }
                //Wait for the script to activate, then set target
                if (pathIndicators[i].straightArrow.isActiveAndEnabled)
                {
                        pathIndicators[i].straightArrow.target = pathIndicators[i].drawArrow.target;
                        pathIndicators[i].straightArrow.transform.parent = pathIndicators[i].drawArrow.target.transform;

                        for (int j = 0; j < pathIndicators[i].drawArrow.empT.EmployeeNavMeshPath.corners.Length; j++)
                        {
                            if (pathIndicators[i].drawArrow.empT.EmployeeNavMeshAgent.hasPath)
                            {
                                pathIndicators[i].straightArrow.path.Add(pathIndicators[i].drawArrow.empT.EmployeeNavMeshPath.corners[j]);
                            }
                        }
                    pathIndicators[i].straightArrow.endLoc = pathIndicators[i].drawArrow.endLoc;
                }
                //End the loop
                if (pathIndicators[i].straightArrow.reachedLastPoint)
                {
                    //If this touch isn't being tracked, skip over this iteration and ensure it's disabled
                    pathIndicators[i].straightArrow.path.Clear();
                    pathIndicators[i].straightArrow.path = new List<Vector3>();

                    pathIndicators[i].straightArrow.reachedLastPoint = false;
                    pathIndicators[i].drawArrow.Reset();
                    pathIndicators[i].instance.SetActive(false);
                    continue;
                }
            }
            //If it's not active, set it active
            if (!pathIndicators[i].instance.activeInHierarchy)
                pathIndicators[i].instance.SetActive(false);


            //update the positions here using pathIndicators[i].touchRef.worldStart/End and the references to the scripts
            if (pathIndicators[i].touchRef.tracking)
            {
                PlayParticleSystemOnAllEmployees(puffList);
                pathIndicators[i].instance.SetActive(true);
                pathIndicators[i].drawArrow.startLoc = new Vector3(
                    pathIndicators[i].touchRef.selectedChar.transform.position.x,
                    pathIndicators[i].touchRef.selectedChar.transform.position.y + 1,
                    pathIndicators[i].touchRef.selectedChar.transform.position.z);

                pathIndicators[i].drawArrow.endLoc = pathIndicators[i].touchRef.worldEnd;
                pathIndicators[i].drawArrow.empT = pathIndicators[i].touchRef.selectedChar;
                pathIndicators[i].drawArrow.startPointSet = true;

                //print("Drawing line!!! Start:" + pathIndicators[i].drawArrow.startloc + ", End: " + pathIndicators[i].drawArrow.endLoc);
            }
        }
    }

    void ArrowCommands()
    {
        for (int i = 0; i < pathIndicators.Count; i++)
        {
            if (pathIndicators[i].drawArrow.killPS && pathIndicators[i].straightArrow.instanceActive)
            {
                pathIndicators[i].straightArrow.Kill();
            }

            if (pathIndicators[i].drawArrow.startPs)
            {
                pathIndicators[i].straightArrow.enabled = true;
            }
            else
            {
                pathIndicators[i].straightArrow.enabled = false;
            }
        }
    }
    public void CreateParticleSystemForAllEmployees(ParticleSystem _pSys, List <ParticleSystem>_list)
    {
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Employee").Length; i++)
        {
            ParticleSystem pSys = new ParticleSystem();

            pSys = Instantiate(_pSys);
            //pSys.Stop();
            _list.Add(pSys);
        }
    }
    public void PlayParticleSystemOnAllEmployees(List <ParticleSystem> _list )
    {
        for (int i =0; i < _list.Count; i++)
        {
            if (!_list[i].isPlaying)
            {
                _list[i].transform.position = GameObject.FindGameObjectsWithTag("Employee")[i].transform.position;
                _list[i].Play();
            }
        }
    }
}
