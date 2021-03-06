﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public static TouchInput Instance;

    public enum InputType
    {
        MOUSE, TOUCH
    }
    public InputType inputType = InputType.TOUCH;

    public class PlayerTouch
    {
        public Touch data;

        public bool tracking = false;
        public bool moved = false;
        public Employee selectedChar = null;
        public Vector3 touchStart;
        public Vector3 worldStart;
        public Vector3 touchEnd;
        public Vector3 worldEnd;
    }
    [HideInInspector] public List<PlayerTouch> playerTouches = new List<PlayerTouch>();
    public int MaxTouches { get; private set; }
    [SerializeField] private int maxTouches = 10;

    private Employee toAssign = null;
    private bool mousePressed = false;

    public LayerMask layerMask;
    public LayerMask roomMask;

    private Room currentRoomHoveredOver = Room.NONE;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        MaxTouches = maxTouches;

        for (int i = 0; i < MaxTouches; i++)
        {
            playerTouches.Add(new PlayerTouch());
        }
    }

    private void Update()
    {
        // -- MOUSE INPUT -- //
        if (inputType == InputType.MOUSE)
        {
            UpdateMouseInput();
            if (playerTouches[0].tracking && playerTouches[0].moved)
            {
                RoomOutline(playerTouches[0]);
            }
            else
            {
                if (currentRoomHoveredOver != Room.NONE)
                {
                    DisableAllOutlines();
                    currentRoomHoveredOver = Room.NONE;
                }
            }
        }
        // -- TOUCH INPUT -- //
        else if (inputType == InputType.TOUCH && Input.touchCount > 0)
        {
            UpdateTouchInput();
            int numTracking = 0;
            foreach(PlayerTouch t in playerTouches)
            {
                if (t.tracking && t.moved)
                {
                    RoomOutline(t);
                    numTracking++;
                }
            }
            if (numTracking <= 0)
            {
                if (currentRoomHoveredOver != Room.NONE)
                {
                    DisableAllOutlines();
                    currentRoomHoveredOver = Room.NONE;
                }
            }
        }
    }

    private void UpdateMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && IsEmployeeAtThisPosition(Input.mousePosition))
        {
            mousePressed = true;
            playerTouches[0].touchStart = Input.mousePosition;
            playerTouches[0].selectedChar = toAssign;
            playerTouches[0].selectedChar.Selected = true;
            playerTouches[0].tracking = true;
            toAssign = null;

            //Debug.Log("mouse pressed");
        }
        if (Input.GetMouseButton(0)
            && playerTouches[0].tracking
            && mousePressed
            && Vector3.Distance(Input.mousePosition, playerTouches[0].touchStart) > 0)
        {
            playerTouches[0].moved = true;
            playerTouches[0].touchEnd = Input.mousePosition;
            playerTouches[0].worldEnd = TouchToWorldspace(Input.mousePosition);
            //Debug.Log("mouse moved");
        }
        if (Input.GetMouseButtonUp(0) && mousePressed)
        {
            playerTouches[0].touchEnd = Input.mousePosition;
            ProcessPlayerTouchData(playerTouches[0]);
            mousePressed = false;
            ResetTouch(playerTouches[0]);
            //Debug.Log("mouse released");
        }
    }

    private void UpdateTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (!IsTouchAlreadyBeingTracked(Input.GetTouch(i)) && IsEmployeeAtThisPosition(Input.GetTouch(i).position))
            {
                PlayerTouch newTouch = GetNewTouch();
                if (newTouch == null)
                {
                    Debug.LogWarning("No free touches! Increase max touch count or stop touching the screen");
                    continue;
                }
                newTouch.data = Input.GetTouch(i);
                newTouch.tracking = true;
                newTouch.touchStart = newTouch.data.position;
                newTouch.worldStart = TouchToWorldspace(newTouch.touchStart);
                newTouch.selectedChar = toAssign;
                newTouch.selectedChar.Selected = true;
                toAssign = null;
            }
        }
        UpdateEmployeeTouches();
    }

    private bool IsEmployeeAtThisPosition(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, layerMask))
        {
            if (hit.transform.GetComponent<Employee>() != null)
            {
                toAssign = hit.transform.GetComponent<Employee>();
                return true;
            }
        }
        return false;
    }

    private void UpdateEmployeeTouches()
    {
        for(int i = 0; i < Input.touchCount; i++)
        {
            foreach(PlayerTouch touch in playerTouches)
            {
                if (Input.GetTouch(i).fingerId == touch.data.fingerId)
                {
                    touch.data = Input.GetTouch(i);
                }
            }
        }

        foreach (PlayerTouch touch in playerTouches)
        {
            if (!touch.tracking) continue;

            touch.touchEnd = touch.data.position;
            touch.worldEnd = TouchToWorldspace(touch.touchEnd);

            if (touch.data.phase == TouchPhase.Moved)
            {
                touch.moved = true;
            }
            if (touch.data.phase == TouchPhase.Ended)
            {
                touch.touchEnd = touch.data.position;
                touch.worldEnd = TouchToWorldspace(touch.touchEnd);

                ProcessPlayerTouchData(touch);

                ResetTouch(touch);
            }
        }
    }

    private Vector3 TouchToWorldspace(Vector3 touchPos)
    {
        Vector3 world = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, layerMask))
        {
            world = hit.point;
            world.y = 0;
        }
        return world;
    }

    private bool IsTouchAlreadyBeingTracked(Touch _touch)
    {
        for (int i = 0; i < playerTouches.Count; i++)
        {
            if (playerTouches[i].data.fingerId == _touch.fingerId
                && playerTouches[i].tracking)
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
        //Debug.Log("Resetting touch");
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

    public List<PlayerTouch> GetCurrentTouches()
    {
        List<PlayerTouch> activeTouches = new List<PlayerTouch>();

        foreach(PlayerTouch touch in playerTouches)
        {
            if (touch.tracking)
                activeTouches.Add(touch);
        }

        return activeTouches;
    }

    private void RoomOutline(PlayerTouch _touch)
    {
        Room newRoom = Room.NONE;

        Ray ray = Camera.main.ScreenPointToRay(_touch.touchEnd);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, roomMask))
        {
            Debug.Log("Raycast " + hit.transform.gameObject.name);
            if (hit.transform.GetComponent<RoomType>() != null)
            {
                Debug.Log("room hovered over");
                newRoom = hit.transform.GetComponent<RoomType>().roomType;
            }
            else
            {
                newRoom = Room.NONE;
            }
        }
        Debug.Log(newRoom);

        if (newRoom != Room.NONE)
        {
            if (newRoom != currentRoomHoveredOver)
            {
                RoomHighlightManager.Instance.SetRoomHighlight(currentRoomHoveredOver, false);
            }
            RoomHighlightManager.Instance.SetRoomHighlight(newRoom, true);
            currentRoomHoveredOver = newRoom;
        }
        else
        {
            DisableAllOutlines();
        }
    }

    private void DisableAllOutlines()
    {
        RoomHighlightManager.Instance.SetRoomHighlight(Room.MEETING, false);
        RoomHighlightManager.Instance.SetRoomHighlight(Room.RELAX, false);
        RoomHighlightManager.Instance.SetRoomHighlight(Room.PRESENTATION, false);
        RoomHighlightManager.Instance.SetRoomHighlight(Room.TASK_1, false);
        RoomHighlightManager.Instance.SetRoomHighlight(Room.TASK_2, false);
        RoomHighlightManager.Instance.SetRoomHighlight(Room.TASK_3, false);
    }
}
