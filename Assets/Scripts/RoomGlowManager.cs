using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGlowManager : MonoBehaviour
{
    public static RoomGlowManager Instance;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
