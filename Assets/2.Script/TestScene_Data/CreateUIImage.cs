using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateUIImage : MonoBehaviour
{
    public Sprite sprite;
    public string objectName;
    public ParticleSystem[] particles;
    public ParticleSystem[] particles3;
    public ParticleSystem[] particles4;
    public ParticleSystem[] particlesCFXR;
    public ParticleSystem[] particlesFireWork;
    public ParticleSystem[] WF;


    void Start()
    {


        particles = Resources.LoadAll<ParticleSystem>("Prefabs/Effect/CFX Prefabs (Mobile)");
        particles3 = Resources.LoadAll<ParticleSystem>("Prefabs/Effect/CFX3 Prefabs (Mobile)");
        particles4 = Resources.LoadAll<ParticleSystem>("Prefabs/Effect/CFX4 Prefabs (Mobile)");
        particlesCFXR = Resources.LoadAll<ParticleSystem>("Prefabs/Effect/CFXR Prefabs");
        particlesFireWork = Resources.LoadAll<ParticleSystem>("Prefabs/Effect/FireWork/_Mobile");
        WF = Resources.LoadAll<ParticleSystem>("Prefabs/Effects/FIRE");


        //CreateEffectPreview(particles);
        //CreateEffectPreview(particles3);
        //CreateEffectPreview(particles4);
        //CreateEffectPreview(particlesCFXR);
        //CreateEffectPreview(particlesFireWork);


       //StartCoroutine(c_CheckEffect(particles));
       //StartCoroutine(c_CheckEffect(particles3));
       //StartCoroutine(c_CheckEffect(particles4));
       //StartCoroutine(c_CheckEffect(particlesCFXR));
       //StartCoroutine(c_CheckEffect(particlesFireWork));
    }

    public void ButtonClick()
    {
        ClearChild();
        StartCoroutine(c_CheckEffect(particles));
    }
    public void ButtonClick1()
    {
        ClearChild();
        StartCoroutine(c_CheckEffect(particles3));
    }
    public void ButtonClick2()
    {
        ClearChild();
        StartCoroutine(c_CheckEffect(particles4));
    }
    public void ButtonClick3()
    {
        ClearChild();
        StartCoroutine(c_CheckEffect(particlesCFXR));
    }
    public void ButtonClick4()
    {
        ClearChild();
        StartCoroutine(c_CheckEffect(particlesFireWork));
    }

    public void ButtonClick5()
    {
        ClearChild();
        StartCoroutine(c_CheckEffect(WF));
    }

    public void ClearChild()
    {
        StopCoroutine("c_CheckEffect");

        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }

    void CreateEffectPreview(ParticleSystem[] particles)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            objectName = particles[i].name;

            GameObject newImageObject = new GameObject(objectName);
            Image newImage = newImageObject.AddComponent<Image>();

            // 이미지 스프라이트 설정
            newImage.sprite = sprite;
            newImage.color = new Vector4(0, 0, 0, 0.1f);



            // 부모 게임 오브젝트 지정
            newImageObject.transform.SetParent(transform, false);

            if (objectName.Contains("CFXM4 Rain Storm"))
                continue;

            GameObject newChildObject = Instantiate(particles[i].gameObject, newImageObject.transform);

            //newChildObject.GetComponent<ParticleSystem>().loop = true;

            newChildObject.transform.localPosition = Vector3.zero;
            newChildObject.transform.localScale = new Vector3(100, 100, 100);





        }


    }


    IEnumerator c_CheckEffect(ParticleSystem[] particles)
    {
        List<ParticleSystem> psPlayList = new List<ParticleSystem>();

        for (int i = 0; i < particles.Length; i++)
        {
            objectName = particles[i].name;

            if (objectName.Contains("Falling"))
                continue;

            particles[i].loop = true;

            GameObject newImageObject = new GameObject(objectName);
            Image newImage = newImageObject.AddComponent<Image>();

            // 이미지 스프라이트 설정
            newImage.sprite = sprite;
            newImage.color = new Vector4(0, 0, 0, 0.1f);



            // 부모 게임 오브젝트 지정
            newImageObject.transform.SetParent(transform, false);

            if (objectName.Contains("CFXM4 Rain Storm"))
                continue;

            GameObject newChildObject = Instantiate(particles[i].gameObject, newImageObject.transform);
            newChildObject.transform.localRotation = Quaternion.identity;


            //newChildObject.GetComponent<ParticleSystem>().loop = true;
            psPlayList.Add(newChildObject.GetComponent<ParticleSystem>());

            newChildObject.transform.localPosition = Vector3.zero;
            newChildObject.transform.localScale = new Vector3(10, 10, 10);

            //yield return new WaitForSeconds(0.3f);

        }

        while (true)
        {
            for (int i = 0; i < psPlayList.Count; i++)
            {
                psPlayList[i].Play();
            
            }

            yield return new WaitForSeconds(1f);

        }


    }
}