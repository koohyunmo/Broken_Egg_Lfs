using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopupWithItem : UI_Popup
{
    protected const int WIDTH = 1070;
    protected const int HEIGHT = 1540;
    protected RectTransform _rect;

  

    public override void Init()
    {
        base.Init();
        SetCanvas();

    }

    public void SetCanvas()
    {

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        //canvas.sortingOrder = 4;

        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1080, 1920);

        canvas = null;
        canvasScaler = null;
    }
    public void SetCanvas2()
    {

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Managers.Effect.TDUICam;
        //canvas.sortingOrder = 4;

        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1080, 1920);

        canvas = null;
        canvasScaler = null;
    }


}
