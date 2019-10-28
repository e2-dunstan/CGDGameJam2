using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public static VFXController Instance;

    private StraightArrow fxStraightArrow;
    private DrawArrow fxDrawArrow;

    public GameObject pathIndicatorPrefab;

    public class PathIndicator
    {
        public GameObject instance;
        public DrawArrow drawArrow;
        public StraightArrow straightArrow;
        public TouchInput.PlayerTouch touchRef = null;
    }
    private List<PathIndicator> pathIndicators = new List<PathIndicator>();



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
            //Assign touch reference
            pathIndicator.touchRef = TouchInput.Instance.playerTouches[i];

            //Disable the object by default
            pathIndicator.instance.SetActive(false);

            pathIndicators.Add(pathIndicator);
        }

        //This will need modifying to allow for the path indicators
        InitialiseSubScripts();
    }

    // Update is called once per frame
    void Update()
    {
        DrawArrowManage();
        
        //New code
        for(int i = 0; i < pathIndicators.Count; i++)
        {
            if (!pathIndicators[i].touchRef.tracking)
            {
                //If this touch isn't being tracked, skip over this iteration and ensure it's disabled
                pathIndicators[i].instance.SetActive(false);
                continue;
            }

            //If it's not active, set it active
            if (!pathIndicators[i].instance.activeInHierarchy)
                pathIndicators[i].instance.SetActive(false);

            //update the positions here using pathIndicators[i].touchRef.worldStart/End and the references to the scripts
        }
    }

    void InitialiseSubScripts()
    {
        fxStraightArrow = GetComponent<StraightArrow>();
        fxStraightArrow.enabled = false;
        fxDrawArrow = GetComponent<DrawArrow>();
    }

    void DrawArrowManage()
    {
        if (fxDrawArrow.killPS)
        {
            fxStraightArrow.Kill();
        }

        if (fxDrawArrow.startPs)
        {
            fxStraightArrow.enabled = true;
        }
        else
        {
            fxStraightArrow.enabled = false;
        }
    }
}
