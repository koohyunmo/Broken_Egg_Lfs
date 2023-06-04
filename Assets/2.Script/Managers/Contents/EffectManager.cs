using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager
{
    public GameObject Root { get; private set; }
    public Canvas _canvas;
    public CanvasScaler _canvasScaler;
    public Camera EffectCam { get; private set; }
    public Camera TDUICam { get; private set; }

    public int MaxEffectCount { get; private set; }
    public int MaxTextCount { get; private set; }
    public List<GameObject> effects { get; private set; }

    /// <summary>
    /// [0] �Ϲ� [1] �Ϲ� small
    /// </summary>
    public GameObject[] damageTexts { get; private set; }

    public ParticleSystem EquipEffect { get; private set; }
    public ParticleSystem EquipEffect2 { get; private set; }
    public GameObject EffectObject { get; private set; } = null;

    GameObject levelUpEffect;


    public List<GameObject> usingParticles = new List<GameObject>(); 

    [Range(0.1f, 1.0f)]
    public float optimizationFactor = 0.1f; // ����ȭ ��� (70%�� ����ȭ�� ���Ѵٸ� 0.7�� ����)

    int layerNumber;

    public void Init()
    {


        EffectCam = GameObject.Find("EffectCamera").GetComponent<Camera>();
        TDUICam = GameObject.Find("3DUICamera").GetComponent<Camera>();

        GameObject root = GameObject.Find("@Effect");
        if (root == null)
        {
            root = new GameObject { name = "@Effect" };
            Root = root;
            Root.AddComponent<Canvas>();
            Root.AddComponent<GraphicRaycaster>();
            Root.AddComponent<CanvasScaler>();

            int layerIndex = LayerMask.NameToLayer("Effect");
            Root.layer = layerIndex;

            _canvas = Root.GetComponent<Canvas>();
            _canvasScaler = Root.GetComponent<CanvasScaler>();

            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.sortingOrder = 3;
            _canvas.worldCamera = EffectCam;

            _canvas.planeDistance = 15f;

            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = new Vector2(1080, 1920);


        }

        layerNumber = LayerMask.NameToLayer("Effect");
        damageTexts = Resources.LoadAll<GameObject>("Prefabs/Effect/DamageTexts/");
        levelUpEffect = Resources.Load<GameObject>("Prefabs/Effect/UIEffect/LevelUpEffect");

    }


    public void EffectCamOn()
    {
        EffectCam.gameObject.SetActive(true);
    }

    public void EffectCamOff()
    {
        EffectCam.gameObject.SetActive(false);
    }

    public GameObject LevelUpEffect()
    {
        GameObject go = GameObject.Instantiate(levelUpEffect);
        GameObject.Destroy(go, 3.5f);

        return go;
    }

    public void UseEffect(string key,bool isCritical)
    {

        if(key != Managers.Game.UseEquipment)
        {
            key = Managers.Game.UseEquipment;
        }

            EffectObject = Managers.Data.ItemDic[key].itemObject;


        {
            Vector2 mouse = EffectCam.ScreenToWorldPoint(Input.mousePosition);

            // ���� ����
            {

                //GameObject particle = Managers.Resource.Instantiate(EffectObject); // ��ƼŬ �ý����� �����մϴ�.
                GameObject particle = GameObject.Instantiate(EffectObject,mouse,Quaternion.identity); // ��ƼŬ �ý����� �����մϴ�.

                

               // particle.transform.localScale = new Vector3(particle.transform.localScale.x, particle.transform.localScale.y, -particle.transform.localScale.z);
               if(Managers.Data.ItemDic[Managers.Game.UseEquipment].soundType == Define.EffectSound.Bow)
                    particle.transform.localScale = new Vector3(particle.transform.localScale.x, -particle.transform.localScale.y, particle.transform.localScale.z);
               else
                {
                    if((int)Managers.Data.ItemDic[Managers.Game.UseEquipment].Grade > (int)(Define.Grade.Rare))
                    {
                        float zPower = UnityEngine.Random.Range(-1, 2);

                        if (isCritical)
                        {
                            GameObject criParticle = GameObject.Instantiate(EffectObject, mouse, Quaternion.identity); // ��ƼŬ �ý����� �����մϴ�.
                            criParticle.transform.localScale = new Vector3(-particle.transform.localScale.x, particle.transform.localScale.y, particle.transform.localScale.z);

                            criParticle.GetComponent<ParticleSystem>().loop = false;

                            usingParticles.Add(particle);
                        }
                        else
                        {
                            particle.transform.localScale = new Vector3(particle.transform.localScale.x, particle.transform.localScale.y, particle.transform.localScale.z);
                        }
                    }
                }

               // particle.transform.localScale = new Vector3(-particle.transform.localScale.x, particle.transform.localScale.y, particle.transform.localScale.z);

                usingParticles.Add(particle);

                EquipEffect = particle.GetComponent<ParticleSystem>(); // ��ƼŬ �ý��� ������Ʈ�� �����ɴϴ�.
                if (EquipEffect == null)
                {
                    Debug.LogWarning($"{key} : NO EFFECT");
                    return;
                }
                //OptimizeParticleSystem(EquipEffect);
                EquipEffect.loop = false;

                //var mainModule = EquipEffect.main;
                //mainModule.duration = mainModule.duration; // ���� �ð��� ������ ���Դϴ�.

                EquipEffect.Play(); // ��ƼŬ �ý����� ����մϴ�.

                GameObject.Destroy(particle, EquipEffect.main.duration + EquipEffect.main.startLifetime.constantMax);
            }
            // ���� 90��
            /*
            {
                GameObject particle2 = GameObject.Instantiate(EffectObject); // ��ƼŬ �ý����� �����մϴ�.
                particle2.transform.localPosition = mouse;
                usingParticles.Add(particle2);
                EquipEffect2 = particle2.GetComponent<ParticleSystem>(); // ��ƼŬ �ý��� ������Ʈ�� �����ɴϴ�.
                if (EquipEffect2 == null)
                {
                    Debug.LogWarning($"{key} : NO EFFECT");
                    return;
                }
                //OptimizeParticleSystem(EquipEffect2);
                EquipEffect2.loop = false;

                //var mainModule = EquipEffect2.main;
                //mainModule.duration = mainModule.duration; // ���� �ð��� ������ ���Դϴ�.

                EquipEffect2.gameObject.transform.position = mouse;
                EquipEffect2.Play(); // ��ƼŬ �ý����� ����մϴ�.

                GameObject.Destroy(particle2, EquipEffect2.main.duration + EquipEffect2.main.startLifetime.constantMax);
            }
            // ��ƼŬ �ý��� ����� ������ �ڵ����� �ı��ǵ��� �����մϴ�.
            */

        }
    }


    public void EffectClear()
    {
        foreach (GameObject go in usingParticles)
        {
            GameObject.Destroy(go);
        }

        usingParticles.Clear();
    }

    public void MakeText(long damage)
    {

        GameObject go = Managers.Resource.Instantiate(damageTexts[0], Root.transform);
        DamageText text = go.GetOrAddComponent<DamageText>();
        text.InitData(damage);
    }

    public void MakeCriticalText(long damage)
    {

        GameObject go = Managers.Resource.Instantiate(damageTexts[0], Root.transform);
        DamageText text = go.GetOrAddComponent<DamageText>();
        text.InitDataCri(damage);
    }

    void OptimizeParticleSystem(ParticleSystem ps)
    {
        Vector2 mouse = EffectCam.ScreenToWorldPoint(Input.mousePosition);

        var mainModule = ps.main;
        mainModule.maxParticles = Mathf.RoundToInt(mainModule.maxParticles * optimizationFactor); // �ִ� ��ƼŬ ���� ���Դϴ�.

        var renderer = ps.GetComponent<ParticleSystemRenderer>();

        try
        {
            if (renderer == null)
                return;

            if (renderer != null && renderer.material != null && renderer.material.mainTexture != null)
            {
                renderer.material.mainTexture.filterMode = FilterMode.Bilinear; // �ؽ�ó ���͸� ���̸��Ͼ�� �����Ͽ� ������ ǰ���� ����ϴ�.
            }
            else
            {
                Debug.LogWarning("Renderer, material, or mainTexture is null");
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }


    }

    public void PopEffect()
    {
        if (MaxEffectCount < 0)
            MaxEffectCount = 0;

        MaxEffectCount--;
    }
}

