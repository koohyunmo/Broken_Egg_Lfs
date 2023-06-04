using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPosition : MonoBehaviour
{
    RectTransform _rect;
    Define.ItemType _type;
    // Start is called before the first frame update
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        if(_rect != null)
            _rect.position = Input.mousePosition;
        else
        {
            transform.position = Input.mousePosition;
        }

        //transform.position = new Vector2(Random.Range(0, 1080), Random.Range(0, 1920));

    }

    private void OnEnable()
    {
        Start();
    }

}
