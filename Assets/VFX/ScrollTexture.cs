using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    private Material mat;
    private float offset = 0;
    public float scrollSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<LineRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset += Time.deltaTime * scrollSpeed;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
