using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableFurniture : MonoBehaviour
{
    public static InteractableFurniture Instance;

    [System.Serializable]
    public class Interactable
    {
        public enum Type
        {
            EMPTY, CHAIR
        }

        public string helperText = "";

        public Type type = Type.EMPTY;
        public Room room = Room.RELAX;

        public Transform origin;

        [HideInInspector]
        public bool occupied = false;
    }
    public Interactable[] interactables;


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public Interactable GetInteractable(Room _room = Room.RELAX)
    {
        List<int> freeObjs = new List<int>();
        for(int i = 0; i < interactables.Length; i++)
        {
            if (!interactables[i].occupied && interactables[i].room == _room)
                freeObjs.Add(i);
        }

        if (freeObjs.Count <= 0)
            return null;

        Interactable var = interactables[freeObjs[Random.Range(0, freeObjs.Count)]];
        var.occupied = true;
        return var;
    }
}
