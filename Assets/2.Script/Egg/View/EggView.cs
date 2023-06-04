using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EggView : MonoBehaviour
{

    public MonsterController.MonsterStatement eggState { get; set; }
 
    [SerializeField]Transform _auraParent;
    [SerializeField]Transform _stageAura;
    [SerializeField] public Image _eggImg { get;set; }
    public int _eggID { get; private set; } = 1;

    Camera _effectCamera;
    Transform _barParent;
    GameObject barUI;
    GameObject _bossAura;
    GameObject _aura;


    private void Init()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localPosition = new Vector3(1, 1, 1);

        _barParent = GameObject.Find("Monster").transform;
       
        //_auraParent = Managers.Effect.Root.transform
            ;
        _eggImg = gameObject.GetComponent<Image>();

        if (gameObject.GetComponent<UI_HpBar_NoCanvas>() == null)
        {
            barUI = Managers.UI.MakeBarUI<UI_HpBar_NoCanvas>(_barParent, "UI_HpBar_Slider").gameObject;
        }

        for (int i = 0; i < _auraParent.transform.childCount; i++)
        {
            Destroy(_auraParent.transform.GetChild(i));
        }

            

        _effectCamera = Managers.Effect.EffectCam;

        EggDataSetter(_eggID);

        Managers.Game.SetEggTransform(transform);
    }

    private void Start()
    {
        Init(); 
    }

    private void Update()
    {

        if (Managers.Game.StageData.currentStage != _eggID)
        {

            _eggID = Managers.Game.StageData.currentStage;
            EggDataSetter(_eggID);
        }
        else
            return;
    }

    public void EggDie()
    {
        _eggID = Managers.Game.StageData.currentStage;
        EggDataSetter(_eggID);
        DestoryUIBar();
    }

    public void OnStageAura()
    {
        if (_aura != null)
            return;

        if (Managers.Stage.GetAura() == null)
            return;

        _aura = Instantiate(Managers.Stage.GetAura(), _stageAura);
        _aura.transform.localPosition = Vector3.zero;
        _aura.transform.localScale = new Vector3(500, 500, 0);
        _aura.transform.rotation = Quaternion.identity;
    }

    public void OffAura()
    {
        if (_aura == null)
            return;

        Destroy(_aura);
        _aura = null;

    }

    public void OnBossAura()
    {
        if (_bossAura != null)
            return;

        _bossAura = Instantiate(Managers.Stage.GetBossAura(), _auraParent);
        _bossAura.transform.localPosition = Vector3.zero;
        _bossAura.transform.localScale = new Vector3(500, 500, 0);
        _bossAura.transform.rotation = Quaternion.identity;
    }

    public void OffBossAura()
    {
        if (_bossAura == null)
            return;

        Destroy(_bossAura);
        _bossAura = null;

    }

    public void DestoryUIBar()
    {
        Managers.Resource.Destroy(barUI);
    }

    private void EggDataSetter(int id)
    {

        if(Managers.Game.StageData.currentStage > Managers.Data.EggDic.Count-1 )
        {
           int index = (Managers.Game.StageData.currentStage % Managers.Data.EggDic.Count);
            Managers.Data.EggImageIndexSetter(index);

            //TODO
            try
            {
                if(index == 0)
                {
                    index++;
                    Managers.Data.EggImageIndexSetter(index);
                }

                _eggImg.sprite = Managers.Data.EggDic[index].eggImage;
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
            

            
        }
        else
        {
            _eggImg.sprite = Managers.Data.EggDic[id].eggImage;
            Managers.Data.EggImageIndexSetter(id);
        }

        
    }

    public void MakeDamageText(long damage)
    {
        Managers.Effect.MakeText(damage);
    }
    public void MakeCriticalDamageText(long damage)
    {
        Managers.Effect.MakeCriticalText(damage);
    }


}
