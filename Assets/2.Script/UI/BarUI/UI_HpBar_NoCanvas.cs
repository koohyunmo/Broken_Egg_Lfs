using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar_NoCanvas : UI_Base
{
    enum GameObjects
    {
        UI_HpBar_Slider
    }

    Canvas _canvas;
    [SerializeField] RectTransform _rect;
    [SerializeField] Vector3 _position;

    const int Y = 745;
    const int X = 0;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        _rect = GetComponent<RectTransform>();
        _position = _rect.localPosition;
        
        transform.localScale = new Vector3(1, 1, 1);
        _rect.localPosition = new Vector3(X, Y, 0);
        //_canvas = GetComponent<Canvas>();

        //_canvas.renderMode = RenderMode.ScreenSpaceCamera;
        //_canvas.worldCamera = Camera.main;
    }

    // 동적인 움직임 구현시 사용
    private void Update()
    {

        Bilborad();

        float _ratio = Managers.Game.StageData.currentHp / (float)Managers.Game.StageData.maxHp;
        SetHpRatio(_ratio);
    }

    private void Bilborad()
    {
        Transform parent = transform.parent;
        //transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;
    }
    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.UI_HpBar_Slider).GetComponent<Slider>().value = ratio;
    }

}
