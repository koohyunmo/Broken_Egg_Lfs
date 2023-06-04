using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDraw : MonoBehaviour
{

    BoxCollider2D box;
    float x, y;


    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        x = box.size.x;
        y = box.size.y;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, new Vector2(x,y));
    }
#endif

}
