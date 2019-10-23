using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public static TouchInput Instance;

    public class PlayerTouch
    {
        public Touch data;

        public bool tracking = false;
        public bool moved = false;
        public Employee selectedChar = null;
        public Vector3 touchStart;
        public Vector3 touchEnd;
    }
    private List<PlayerTouch> playerTouches = new List<PlayerTouch>();
    public int maxTouches = 4;

    private Employee toAssign = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        for (int i = 0; i < maxTouches; i++)
        {
            playerTouches.Add(new PlayerTouch());
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (!IsTracking(Input.GetTouch(i)) && IsEmployee(Input.GetTouch(i).position))
                {
                    PlayerTouch newTouch = GetNewTouch();
                    if (newTouch == null)
                    {
                        Debug.LogWarning("No free touches! Increase max touch count or stop touching the screen");
                        continue;
                    }
                    newTouch.tracking = true;
                    newTouch.touchStart = newTouch.data.position;
                    newTouch.selectedChar = toAssign;
                    newTouch.selectedChar.Selected = true;
                    toAssign = null;

                    Debug.Log("Employee touched");
                }
            }

            UpdateCurrentTouches();
        }
    }

    private bool IsEmployee(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<Employee>() != null)
            {
                toAssign = hit.transform.GetComponent<Employee>();
                return true;
            }
        }
        return false;
    }

    private void UpdateCurrentTouches()
    {
        foreach (PlayerTouch touch in playerTouches)
        {
            touch.touchEnd = touch.data.position;
            if (touch.data.phase == TouchPhase.Moved)
            {
                touch.moved = true;
            }
            if (touch.data.phase == TouchPhase.Ended)
            {
                touch.touchEnd = touch.data.position;
                ProcessPlayerTouchData(touch);

                ResetTouch(touch);
            }
        }
    }

    private bool IsTracking(Touch _touch)
    {
        for (int i = 0; i < playerTouches.Count; i++)
        {
            if (playerTouches[i].data.fingerId == _touch.fingerId)
            {
                return true;
            }
        }
        return false;
    }

    private PlayerTouch GetNewTouch()
    {
        for (int i = 0; i < playerTouches.Count; i++)
        {
            if (!playerTouches[i].tracking)
            {
                return playerTouches[i];
            }
        }
        return null;
    }

    private void ResetTouch(PlayerTouch _touch)
    {
        _touch.tracking = false;
        _touch.moved = false;
        _touch.selectedChar = null;
        _touch.touchStart = Vector3.zero;
        _touch.touchEnd = Vector3.zero;
    }

    private void ProcessPlayerTouchData(PlayerTouch _touch)
    {
        if (_touch.moved && _touch.selectedChar != null)
        {
            _touch.selectedChar.Selected = false;
            _touch.selectedChar.ProcessNewPath(_touch);
        }
    }
}
