using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public static VFXController Instance;

    private StraightArrow fxStraightArrow;
    private DrawArrow fxDrawArrow;

    public GameObject pathIndicatorPrefab;

    public ParticleSystem runningPS;
    public List<PartSys> runningPSList = new List<PartSys>();
    public ParticleSystem idlePS;
    public List<PartSys> idlePSList = new List<PartSys>();
    public ParticleSystem pulsePS;
    public List<PartSys> pulsePSList = new List<PartSys>();

    public class PathIndicator
    {
        public GameObject instance;
        public DrawArrow drawArrow;
        public StraightArrow straightArrow;
        public ParticleSystem ps;
        public TouchInput.PlayerTouch touchRef = null;
    }

    private List<PathIndicator> pathIndicators = new List<PathIndicator>();
    private List<PathIndicator> employeePaths = new List<PathIndicator>();

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
        //CreateParticleSystemForAllEmployees(idlePS, idlePSList);
        //CreateParticleSystemForAllEmployees(runningPS, runningPSList);

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
                    GameObject currentEmployee = pathIndicators[i].drawArrow.target;

                    //Initialise a new path indicator on start
                    PathIndicator employeePath = new PathIndicator();
                    employeePath.instance = Instantiate(pathIndicatorPrefab, transform);
                    employeePath.drawArrow = employeePath.instance.GetComponent<DrawArrow>();
                    employeePath.drawArrow.targetEmployee = pathIndicators[i].drawArrow.targetEmployee;
                    employeePath.drawArrow.endDragLoc = pathIndicators[i].drawArrow.endDragLoc;
                    employeePath.drawArrow.startPointSet = pathIndicators[i].drawArrow.startPointSet;
                    employeePath.drawArrow.endPointSet = true;
                    employeePath.instance.transform.position = currentEmployee.transform.position;

                    employeePaths.Add(employeePath);

                    pathIndicators[i].drawArrow.Reset();
                    pathIndicators[i].instance.transform.position = Vector3.zero;
                    pathIndicators[i].instance.SetActive(false);
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
                PlayParticleSystemOnEmployee(pathIndicators[i].drawArrow.targetEmployee.gameObject, pulsePSList);
                pathIndicators[i].drawArrow.endDragLoc = pathIndicators[i].touchRef.worldEnd;
                pathIndicators[i].drawArrow.startPointSet = true;

                //print("Drawing line!!! Start:" + pathIndicators[i].drawArrow.startloc + ", End: " + pathIndicators[i].drawArrow.endLoc);
            }
        }

        for (int i = 0; i < employeePaths.Count; i++)
        {
            //End the loop
            if (employeePaths[i].drawArrow.reached)
            {
                GameObject currentEmployee = employeePaths[i].drawArrow.target;

                PathIndicator employeePath = employeePaths[i];
                employeePath.drawArrow.Reset();

                employeePaths.Remove(employeePath);
                employeePath.instance.SetActive(false);
                Destroy(employeePath.instance);
            }
        }
    }

    public void CreateParticleSystemForEmployee(ParticleSystem _pEffect, List<PartSys> _list)
    {
        PartSys pSys = new PartSys();

        pSys.effect = Instantiate(_pEffect, transform);
        pSys.instance = pSys.effect.gameObject;
        pSys.target = pSys.instance;
        _list.Add(pSys);
        _list[_list.Count-1].instance.SetActive(false);
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
                if (!_list[i].effect.isPlaying)
                {
                    StopParticleSystemOnEmployee(_list[i].target, _list);
                }
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

                if (!_list[i].effect.isPlaying)
                {
                    StopParticleSystemOnEmployee(_list[i].target, _list);
                }
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

                if (!_list[i].effect.isPlaying)
                {
                    StopParticleSystemOnEmployee(_list[i].target, _list);
                }
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

                if (!_list[i].effect.isPlaying)
                {
                    StopParticleSystemOnEmployee(_list[i].target, _list);
                }
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
