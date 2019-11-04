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
    void DrawArrowManage()
    {

        for (int i = 0; i < pathIndicators.Count; i++)
        {
            if (!pathIndicators[i].touchRef.tracking)
            {
                //If the startpoint is set, trigger the particle system
                if (pathIndicators[i].drawArrow.startPointSet)
                {
                    pathIndicators[i].drawArrow.endPointSet = true;
                }
                //End the loop
                if (pathIndicators[i].drawArrow.reached)
                {
                    //If this touch isn't being tracked, skip over this iteration and ensure it's disabled
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
                pathIndicators[i].instance.SetActive(true);

                pathIndicators[i].drawArrow.targetEmployee = pathIndicators[i].touchRef.selectedChar;
                pathIndicators[i].drawArrow.endDragLoc = pathIndicators[i].touchRef.worldEnd;
                pathIndicators[i].drawArrow.startPointSet = true;

                //print("Drawing line!!! Start:" + pathIndicators[i].drawArrow.startloc + ", End: " + pathIndicators[i].drawArrow.endLoc);
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
