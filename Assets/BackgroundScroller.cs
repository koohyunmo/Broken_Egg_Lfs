using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float speed = 0.1f;
    public float offsetX = 0f;

    private Material _material;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
        offsetX = _material.GetTextureOffset("_MainTex").x;
    }

    private void Update()
    {
        offsetX += Time.deltaTime * speed;
        _material.SetTextureOffset("_MainTex", new Vector2(offsetX, 0));
    }
}
