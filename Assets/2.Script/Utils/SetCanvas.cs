using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCanvas : MonoBehaviour
{

    Canvas _canvas;
    CanvasScaler _canvasScaler;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }


    public void Init()
    {
            _canvas = GetComponent<Canvas>();
            _canvasScaler = GetComponent<CanvasScaler>();

            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = Camera.main;

            _canvas.planeDistance = 15f;

            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = new Vector2(1080, 1920);
    }

}
