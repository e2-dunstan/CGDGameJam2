using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager: MonoBehaviour
{
    public static ItemManager Instance;

    public List<GameObject> itemList = new List<GameObject>();

    public List<GameObject> spawnPositions = new List<GameObject>();

    public struct ColorStruct
    {
        public ColorStruct(Color _color)
        {
            color = _color;
            isInUse = false;
        }

        public Color color;
        public bool isInUse;
    };

    public List<ColorStruct> colorList = new List<ColorStruct>();

    public List<Color> colors = new List<Color>();
    // Start is called before the first frame update

    int referencePoint = 0;
    int referenceSpawnPoint = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

    }

    private void Start()
    {
        foreach(var col in colors)
        {
            colorList.Add(new ColorStruct(col));
        }
    }
    public GameObject GetRandomItem()
    {
        List<GameObject> spawnablePositions = spawnPositions.Where(x => x.GetComponent<CollectableSpawnPosition>().isSomethingSpawnedHere != true).ToList();

        int randomIndex = Random.Range(0, spawnablePositions.Count);

        if (itemList.Count > 0 && spawnablePositions != null)
        {
            spawnablePositions[randomIndex].GetComponent<CollectableSpawnPosition>().isSomethingSpawnedHere = true;
            GameObject tempObj = Instantiate(itemList[randomIndex], spawnablePositions[randomIndex].transform);
            tempObj.GetComponent<CollectableItem>().spawnPosition = spawnablePositions[randomIndex];
            return tempObj;
        }
        else
        {
            return null;
        }
    }

    public Color GetColor()
    {
        //I know this is disgusting. It's crunch.
        if (referencePoint > colorList.Count)
        {
            return Color.red;
        }
        else
        {
            referencePoint++;
            return colorList[referencePoint - 1].color;
        }
    }

    public void RemoveColorFromActive()
    {
        referencePoint--;
    }

    public void SetSpawnPositionToActive(GameObject _spawnPos)
    {
        GameObject tempObj = spawnPositions.Where(x => x.transform.position == _spawnPos.transform.position).FirstOrDefault();

        if(tempObj != null)
        {
            tempObj.GetComponent<CollectableSpawnPosition>().isSomethingSpawnedHere = false;
        }
    }
}
