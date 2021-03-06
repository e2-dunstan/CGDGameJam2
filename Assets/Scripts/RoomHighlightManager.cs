﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using Pixelplacement.TweenSystem;

public class RoomHighlightManager : MonoBehaviour
{
    public static RoomHighlightManager Instance;

    //public enum ROOM
    //{
    //    MEETING,
    //    PRESENTATION,
    //    IDLE,
    //    TASK1,
    //    TASK2,
    //    TASK3
    //}

    [Header("References")]
    [SerializeField] private Outline meetingRoomOutline;
    [SerializeField] private Outline presentationRoomOutline;
    [SerializeField] private Outline idleRoomOutline;
    [SerializeField] private Outline taskRoom1Outline;
    [SerializeField] private Outline taskRoom2Outline;
    [SerializeField] private Outline taskRoom3Outline;

    private TweenBase meetingTween;
    private TweenBase presentationTween;
    private TweenBase idleTween;
    private TweenBase task1Tween;
    private TweenBase task2Tween;
    private TweenBase task3Tween;

    private float meetingAlpha;
    private float presentationAlpha;
    private float idleAlpha;
    private float task1Alpha;
    private float task2Alpha;
    private float task3Alpha;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    void Start()
    {
        ResetAllRooms();
    }

    private void Update()
    {
        UpdateRoomsAlpha();
    }

    private void UpdateRoomsAlpha()
    {
        Color tempCol;

        //Meeting room update
        tempCol = meetingRoomOutline.OutlineColor;
        tempCol.a = meetingAlpha;
        meetingRoomOutline.OutlineColor = tempCol;

        //Presentation room update
        tempCol = presentationRoomOutline.OutlineColor;
        tempCol.a = presentationAlpha;
        presentationRoomOutline.OutlineColor = tempCol;

        //Idle room update
        tempCol = idleRoomOutline.OutlineColor;
        tempCol.a = idleAlpha;
        idleRoomOutline.OutlineColor = tempCol;

        //Task 1 room update
        tempCol = taskRoom1Outline.OutlineColor;
        tempCol.a = task1Alpha;
        taskRoom1Outline.OutlineColor = tempCol;

        //Task 2 room update
        tempCol = taskRoom2Outline.OutlineColor;
        tempCol.a = task2Alpha;
        taskRoom2Outline.OutlineColor = tempCol;

        //Task 3 room update
        tempCol = taskRoom3Outline.OutlineColor;
        tempCol.a = task3Alpha;
        taskRoom3Outline.OutlineColor = tempCol;
    }

    public void SetRoomHighlight(Room _room, bool isOn)
    {
        if (isOn)
        {
            StartRoomHighlight(_room);
        }
        else
        {
            StopRoomHighlight(_room);
        }
    }

    void StartRoomHighlight(Room _room)
    {
        switch (_room)
        {
            case Room.MEETING:
                if(meetingTween == null || meetingTween.Status != Tween.TweenStatus.Running)
                {
                    meetingTween = Tween.Value(0f, 1f, UpdateMeetingAlpha, 0.75f, 0f, Tween.EaseInOut, Tween.LoopType.PingPong);
                }
                break;
            case Room.PRESENTATION:
                if (presentationTween == null || presentationTween.Status != Tween.TweenStatus.Running)
                {
                    presentationTween = Tween.Value(0f, 1f, UpdatePresentationAlpha, 0.75f, 0f, Tween.EaseInOut, Tween.LoopType.PingPong);
                }
                break;
            case Room.RELAX:
                if (idleTween == null || idleTween.Status != Tween.TweenStatus.Running)
                {
                    idleTween = Tween.Value(0f, 1f, UpdateIdleAlpha, 0.75f, 0f, Tween.EaseInOut, Tween.LoopType.PingPong);
                }
                break;
            case Room.TASK_1:
                if (task1Tween == null || task1Tween.Status != Tween.TweenStatus.Running)
                {
                    task1Tween = Tween.Value(0f, 1f, UpdateTask1Alpha, 0.75f, 0f, Tween.EaseInOut, Tween.LoopType.PingPong);
                }
                break;
            case Room.TASK_2:
                if (task2Tween == null || task2Tween.Status != Tween.TweenStatus.Running)
                {
                    task2Tween = Tween.Value(0f, 1f, UpdateTask2Alpha, 0.75f, 0f, Tween.EaseInOut, Tween.LoopType.PingPong);
                }
                break;
            case Room.TASK_3:
                if (task3Tween == null || task3Tween.Status != Tween.TweenStatus.Running)
                {
                    task3Tween = Tween.Value(0f, 1f, UpdateTask3Alpha, 0.75f, 0f, Tween.EaseInOut, Tween.LoopType.PingPong);
                }
                break;
        }
    }

    void UpdateMeetingAlpha(float a)
    {
        meetingAlpha = a;
    }

    void UpdatePresentationAlpha(float a)
    {
        presentationAlpha = a;
    }

    void UpdateIdleAlpha(float a)
    {
        idleAlpha = a;
    }

    void UpdateTask1Alpha(float a)
    {
        task1Alpha = a;
    }

    void UpdateTask2Alpha(float a)
    {
        task2Alpha = a;
    }

    void UpdateTask3Alpha(float a)
    {
        task3Alpha = a;
    }

    void StopRoomHighlight(Room _room)
    {
        switch (_room)
        {
            case Room.MEETING:
                meetingTween?.Stop();
                meetingAlpha = 0;
                //meetingTween = Tween.Value(meetingAlpha, 0f, UpdateMeetingAlpha, 0.5f, 0f, Tween.EaseInOut);
                break;
            case Room.PRESENTATION:
                presentationTween?.Stop();
                presentationAlpha = 0;
                //presentationTween = Tween.Value(presentationAlpha, 0f, UpdatePresentationAlpha, 0.5f, 0f, Tween.EaseInOut);
                break;
            case Room.RELAX:
                idleTween?.Stop();
                idleAlpha = 0;
                //idleTween = Tween.Value(idleAlpha, 0f, UpdateIdleAlpha, 0.5f, 0f, Tween.EaseInOut);
                break;
            case Room.TASK_1:
                task1Tween?.Stop();
                task1Alpha = 0;
                //task1Tween = Tween.Value(task1Alpha, 0f, UpdateTask1Alpha, 0.5f, 0f, Tween.EaseInOut);
                break;
            case Room.TASK_2:
                task2Tween?.Stop();
                task2Alpha = 0;
                //task2Tween = Tween.Value(task2Alpha, 0f, UpdateTask2Alpha, 0.5f, 0f, Tween.EaseInOut);
                break;
            case Room.TASK_3:
                task3Tween?.Stop();
                task3Alpha = 0;
                //task3Tween = Tween.Value(task3Alpha, 0f, UpdateTask3Alpha, 0.5f, 0f, Tween.EaseInOut);
                break;
        }
    }

    private void ResetAllRooms()
    {
        meetingTween?.Stop();
        presentationTween?.Stop();
        idleTween?.Stop();
        task1Tween?.Stop();
        task2Tween?.Stop();
        task3Tween?.Stop();

        meetingAlpha = 0f;
        presentationAlpha = 0f;
        idleAlpha = 0f;
        task1Alpha = 0f;
        task2Alpha = 0f;
        task3Alpha = 0f;

        UpdateRoomsAlpha();
    }

    [ContextMenu("Flash Task rooms")]
    void FlashTaskRooms()
    {
        SetRoomHighlight(Room.TASK_1, true);
        SetRoomHighlight(Room.TASK_2, true);
        SetRoomHighlight(Room.TASK_3, true);
    }

    [ContextMenu("Flash Idle Room")]
    void FlashIdle()
    {
        SetRoomHighlight(Room.RELAX, true);
    }

    [ContextMenu("Flash Presentation And Meeting")]
    void FlashMeetingAndIdle()
    {
        SetRoomHighlight(Room.MEETING, true);
        SetRoomHighlight(Room.PRESENTATION, true);
    }

    [ContextMenu("Stop all flashing")]
    void StopAllFlashing()
    {
        SetRoomHighlight(Room.TASK_1, false);
        SetRoomHighlight(Room.TASK_2, false);
        SetRoomHighlight(Room.TASK_3, false);
        SetRoomHighlight(Room.MEETING, false);
        SetRoomHighlight(Room.PRESENTATION, false);
        SetRoomHighlight(Room.RELAX, false);
    }
}
