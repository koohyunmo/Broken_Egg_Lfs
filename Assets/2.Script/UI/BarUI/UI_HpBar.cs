using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UI_Base
{
    enum GameObjects
    {
        HPBar
    }

    Canvas _canvas;


    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        transform.localScale = new Vector3(1, 1, 1);
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
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }

}
