using Coffee.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Monster : UI_Popup
{
    enum Images
    {
        MonsterEgg
    }
    public enum Particles
    {
        GoldEvent,
        ExpEvent
    }
    Canvas canvas;


    public override void Init()
    {

        SettingCanvas();

        BindImage(typeof(Images));
        BindObject(typeof(Particles));

        {
            Managers.Loot.SetTransform();
            Managers.Loot.GetGoldParticle(GameObject.FindGameObjectWithTag("GPS").GetComponentInChildren<ParticleSystem>());
            Managers.Loot.GetExpParticle(GameObject.FindGameObjectWithTag("EPS").GetComponentInChildren<ParticleSystem>());
        }

        Debug.Log("PlayUI Monster");


        Managers.Loot.GoldIcon.gameObject.GetComponent<UIParticleAttractor>().m_ParticleSystem = Managers.Loot.GoldEvent;
        Managers.Loot.LevelIcon.gameObject.GetComponent<UIParticleAttractor>().m_ParticleSystem = Managers.Loot.ExpEvent;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void SettingCanvas()
    {
        {
            canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Managers.Loot.lootCam;
            canvas.planeDistance = 15;
            canvas.sortingOrder = 3;

            // CanvasScaler ������Ʈ ��������
            CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

            // UI Scale Mode�� Scale With Screen���� ����
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            // ũ�� ����
            canvasScaler.referenceResolution = new Vector2(1080f, 1920f);
        }

    }

}
