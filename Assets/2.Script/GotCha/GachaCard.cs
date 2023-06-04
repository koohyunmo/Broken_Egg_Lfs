using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaCard : UI_Base
{

    public GameObject ItemCard;
    public GameObject OpenEffect;
    public delegate void AnimEvent();
    public event AnimEvent End;

    enum Images
    {
        Frame,
        ItemIcon,
    }

    enum GameObejcts
    {
        GradeTMP
    }

    string _id;
    GameObject particle;
    public void InitData(string id, GameObject go)
    {
        _id = id;
        particle = go;
    }

    private void Start()
    {
        Init();

    }

    IEnumerator c_OpenCard()
    {
        yield return new WaitForSeconds(1f);
        ConfigParticleSystem();
        ItemCard.SetActive(true);
        ItemCard.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.3f);
        OpenEffect.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        if (End != null)
        {
            End();
        }

    }

    public override void Init()
    {
        BindImage(typeof(Images));
        BindObject(typeof(GameObejcts));

        GetImage((int)Images.Frame).GetComponent<Image>().sprite = Managers.Data.Frames[(int)Managers.Data.ItemDic[_id].Grade];
        GetImage((int)Images.ItemIcon).GetComponent<Image>().sprite = Managers.Data.ItemDic[_id].itemIcon;
        GetObject((int)GameObejcts.GradeTMP).GetComponent<TextMeshProUGUI>().text = Managers.Data.ItemDic[_id].Grade.ToString();
        
        ItemCard.SetActive(false);

        OpenEffect.SetActive(true);
        StartCoroutine(c_OpenCard());

    }

    void ConfigParticleSystem()
    {
        int grade = (int)Managers.Data.ItemDic[_id].Grade;
        ParticleSystem ps = particle.GetComponent<ParticleSystem>();
        

        if (grade <= 2)
        {
            ps.gameObject.SetActive(false);
        }

    }
}
