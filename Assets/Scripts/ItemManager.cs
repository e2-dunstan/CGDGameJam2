using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public List<GameObject> itemList = new List<GameObject>();

    public List<GameObject> spawnPositions = new List<GameObject>();

    // Start is called before the first frame update

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    public GameObject GetRandomItem()
    {
        int randomIndex = Random.Range(0, itemList.Count);

        int randomSpawnIndex = Random.Range(0, spawnPositions.Count);

        if(itemList.Count > 0)
        {
            GameObject tempObj = Instantiate(itemList[randomIndex], spawnPositions[randomSpawnIndex].transform);
            return tempObj;
        }
        else
        {
            return null;
        }
    }
}
