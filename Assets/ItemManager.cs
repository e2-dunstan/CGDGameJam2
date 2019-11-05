using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager: MonoBehaviour
{
    public static ItemManager Instance;

    public List<GameObject> itemList = new List<GameObject>();

    public List<GameObject> spawnPositions = new List<GameObject>();

    public class ColorStruct
    {
        public ColorStruct(Color _color)
        {
            color = _color;
            isInUse = false;
        }
        public void SetIsInUse(bool _setTo)
        {
            isInUse = _setTo;
        }

        public Color color;
        public bool isInUse;
    };

    public List<ColorStruct> colorList = new List<ColorStruct>();
    private List<ColorStruct> activeColors = new List<ColorStruct>();

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
        List<ColorStruct> tempColorList = colorList.Where(x => x.isInUse == false).ToList();
        int randomIndex = Random.Range(0, tempColorList.Count);

        for(int i = 0; i < colorList.Count; i++)
        {
            if (colorList[i].color == tempColorList[randomIndex].color)
            {
                colorList[i].isInUse = true;
            }
        }

        return tempColorList[randomIndex].color;
        
    }

    public void RemoveColorFromActive(Color _color)
    {
        for(int i = 0; i < colorList.Count; i++)
        {
            if(colorList[i].color == _color)
            {
                colorList[i].isInUse = false;
            }
        }
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
