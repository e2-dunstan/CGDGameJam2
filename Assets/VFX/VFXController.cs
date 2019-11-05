using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public static VFXController Instance;

    private StraightArrow fxStraightArrow;
    private DrawArrow fxDrawArrow;

    public GameObject pathIndicatorPrefab;

    [SerializeField] private ParticleSystem runningPS;
    private List<PartSys> runningPSList = new List<PartSys>();
    [SerializeField] private ParticleSystem idlePS;
    private List<PartSys> idlePSList = new List<PartSys>();

    public class PathIndicator
    {
        public GameObject instance;
        public DrawArrow drawArrow;
        public StraightArrow straightArrow;
        public ParticleSystem ps;
        public TouchInput.PlayerTouch touchRef = null;
    }
    private List<PathIndicator> pathIndicators = new List<PathIndicator>();

    public class PartSys
    {
        public GameObject instance;
        public GameObject target;
        public ParticleSystem effect;
    }



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
        CreateParticleSystemForAllEmployees(idlePS, idlePSList);
        CreateParticleSystemForAllEmployees(runningPS, runningPSList);

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
            GameObject currentEmployee = pathIndicators[i].drawArrow.target;

            if (!pathIndicators[i].touchRef.tracking)
            {
                //If the startpoint is set, trigger the particle system
                if (pathIndicators[i].drawArrow.startPointSet)
                {
                    pathIndicators[i].drawArrow.endPointSet = true;
                    pathIndicators[i].instance.transform.position = currentEmployee.transform.position;

                    //Play particle systems
                    PlayParticleSystemOnEmployee(currentEmployee, runningPSList, 0, -0.25f);
                    PlayParticleSystemOnEmployee(currentEmployee, idlePSList, 0, 4);
                }
                //End the loop
                if (pathIndicators[i].drawArrow.reached)
                {
                    //If this touch isn't being tracked, skip over this iteration and ensure it's disabled
                    pathIndicators[i].drawArrow.Reset();
                    StopParticleSystemOnEmployee(currentEmployee, runningPSList);
                    StopParticleSystemOnEmployee(currentEmployee, idlePSList);
                    pathIndicators[i].instance.transform.position = Vector3.zero;
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

    public void CreateParticleSystemForAllEmployees(ParticleSystem _pEffect, List <PartSys>_list)
    {
        _list.Clear();
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Employee").Length; i++)
        {
            PartSys pSys = new PartSys();
            
            pSys.effect = Instantiate(_pEffect, transform);
            pSys.instance = pSys.effect.gameObject;
            pSys.target = pSys.instance;
            _list.Add(pSys);
            _list[i].instance.SetActive(false);
        }
    }
    public bool PlayParticleSystemOnEmployee(GameObject _target, List<PartSys> _list)
    {
        //  Check if there are already any of this system with this target
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].target == _target)
            {
                _list[i].instance.transform.position = _list[i].target.transform.position;
                return false;
            }
        }
        //Apply the first available system to the target
        for (int i = 0; i < _list.Count; i++)
        {
            if (!_list[i].instance.activeSelf)
            {
                _list[i].target = _target;
                _list[i].instance.SetActive(true);
                _list[i].instance.transform.position = _list[i].target.transform.position;
                _list[i].effect.Play();
                return true;
            }
        }
        return false;
    }
    //          Overloads       //
    public bool PlayParticleSystemOnEmployee(GameObject _target, List<PartSys> _list, float xAdjustment)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].target == _target)
            {
                _list[i].instance.transform.position = new Vector3(
                _target.transform.position.x + xAdjustment,
                _target.transform.position.y,
                _target.transform.position.z);

                return false;
            }
        }
        for (int i = 0; i < _list.Count; i++)
        {
            if (!_list[i].instance.activeSelf)
            {
                _list[i].target = _target;
                _list[i].instance.SetActive(true);
                _list[i].instance.transform.position = new Vector3(
                _target.transform.position.x + xAdjustment,
                _target.transform.position.y,
                _target.transform.position.z);
                _list[i].effect.Play();
                return true;
            }
        }
        return false;
    }
    public bool PlayParticleSystemOnEmployee(GameObject _target, List<PartSys> _list, float xAdjustment, float yAdjustment)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].target == _target)
            {
                _list[i].instance.transform.position = new Vector3(
                    _target.transform.position.x + xAdjustment,
                    _target.transform.position.y + yAdjustment,
                    _target.transform.position.z);

                return false;
            }
        }
        for (int i = 0; i < _list.Count; i++)
        {
            if (!_list[i].instance.activeSelf)
            {
                _list[i].target = _target;
                _list[i].instance.SetActive(true);

                _list[i].instance.transform.position = new Vector3(
                    _target.transform.position.x + xAdjustment,
                    _target.transform.position.y + yAdjustment,
                    _target.transform.position.z);

                _list[i].effect.Play();
                return true;
            }
        }
        return false;

    }
    public bool PlayParticleSystemOnEmployee(GameObject _target, List<PartSys> _list, float xAdjustment, float yAdjustment, float zAdjustment)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].target == _target)
            {
                _list[i].instance.transform.position = new Vector3(
                    _target.transform.position.x + xAdjustment,
                    _target.transform.position.y + yAdjustment,
                    _target.transform.position.z + zAdjustment);

                return false;
            }
        }
        for (int i = 0; i < _list.Count; i++)
        {
            if (!_list[i].instance.activeSelf)
            {
                _list[i].target = _target;
                _list[i].instance.SetActive(true);

                    _list[i].instance.transform.position = new Vector3(
                    _target.transform.position.x + xAdjustment,
                    _target.transform.position.y + yAdjustment,
                    _target.transform.position.z + zAdjustment);

                _list[i].effect.Play();
                return true;
            }
        }
        return false;
    }
    //      End of Overloads    //

    public void StopParticleSystemOnEmployee(GameObject _target, List<PartSys> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].instance.activeSelf)
            {
                if (_list[i].target == _target)
                {
                    _list[i].effect.Stop();
                    _list[i].target = _list[i].instance;
                    _list[i].instance.transform.position = Vector3.zero;
                    _list[i].instance.SetActive(false);
                }
            }
        }
    }
    //For debugging
    public void PlayParticleSystemOnAllEmployees(List <ParticleSystem> _list )
    {
        for (int i =0; i < _list.Count; i++)
        {
            _list[i].transform.position = GameObject.FindGameObjectsWithTag("Employee")[i].transform.position;
            if (!_list[i].isPlaying)
            {
                _list[i].Play();
            }
        }
    }
}
