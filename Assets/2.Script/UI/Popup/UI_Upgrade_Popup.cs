using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_Upgrade_Popup : UI_PopupWithItem
{

    enum Buttons
    {
        GoldUpgradeButton,
        GemUpgradeButton,
        CloseInformationButton,
        Gold10UpgradeButton,
        Gem10UpgradeButton
    }

    enum Images
    {
        ItemFrame,
        ItemIcon,
    }

    enum TMPs
    {
        LEVELTMP,
        AddLevelTMP,
        DMGTMP,
        AddDmgTMP,
        GoldUpgradeTMP,
        GemUpgradeTMP,
        GradeTMP,
        ReinforceTMP,
        ArmorReductionTMP,
        AddReductionTMP,
        CriPercentTMP,
        AddCriPecentTMP,
        CriDMGTMP,
        AddCriTMG,
        Gem10UpgradePriceTMP,
        Gold10UpgradePriceTMP
    }

    enum Sliders
    {
        GemSlider,
        GoldSlider,
        Gold10Slider,
        Gem10Slider
    }

    string _id;
    bool _isReinAnim;

    TextMeshProUGUI _goldUpgradeTMP;
    TextMeshProUGUI _gemUpgradeTMP;

    TextMeshProUGUI _gold10UpgradeTMP;
    TextMeshProUGUI _gem10UpgradeTMP;

    TextMeshProUGUI _currentLevelTMP;
    TextMeshProUGUI _addLevelTMP;

    TextMeshProUGUI _currenDMGTMP;
    TextMeshProUGUI _addDMGTMP;

    TextMeshProUGUI _lv;
    TextMeshProUGUI _grade;

    TextMeshProUGUI _reductionTMP;
    TextMeshProUGUI _AddreductionTMP;

    TextMeshProUGUI _criPerTMP;
    TextMeshProUGUI _addCriPerTMP;

    TextMeshProUGUI _criDMGTMP;
    TextMeshProUGUI _addCriDMGTMP;

    Slider _goldSlider;
    Slider _gemSlider;

    Slider _gold10Slider;
    Slider _gem10Slider;

    Image _frame;
    [SerializeField] Image _icon;


    Action update;


    private void Start()
    {
        Init();
    }


    bool _isAnim;

    Text reinforeceText;

    public void InitData(string id, Action T)
    {
        _id = id;
        update = T;
    }


    public void InitData()
    {
        Debug.Log("Upgrade Popup");

    }

    public override void Init()
    {

        base.Init();

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TMPs));
        Bind<Slider>(typeof(Sliders));


        this.gameObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<RectTransform>().transform.localPosition = new Vector3(0, 0, 0);

        GetButton((int)Buttons.GoldUpgradeButton).gameObject.BindEvent(GoldUpgradeButtonClick);
        GetButton((int)Buttons.GemUpgradeButton).gameObject.BindEvent(GemUpgradeButtonClick);

        GetButton((int)Buttons.Gold10UpgradeButton).gameObject.BindEvent(Gold10UpgradeButtonClick);
        GetButton((int)Buttons.Gem10UpgradeButton).gameObject.BindEvent(Gem10UpgradeButtonClick);


        GetButton((int)Buttons.CloseInformationButton).gameObject.BindEvent(OnClickUpgradeCloseButton);

        _goldUpgradeTMP = Get<TextMeshProUGUI>((int)TMPs.GoldUpgradeTMP);
        _gemUpgradeTMP = Get<TextMeshProUGUI>((int)TMPs.GemUpgradeTMP);

        _gold10UpgradeTMP = Get<TextMeshProUGUI>((int)TMPs.Gold10UpgradePriceTMP);
        _gem10UpgradeTMP = Get<TextMeshProUGUI>((int)TMPs.Gem10UpgradePriceTMP);

        _grade = Get<TextMeshProUGUI>((int)TMPs.GradeTMP);
        _lv = Get<TextMeshProUGUI>((int)TMPs.ReinforceTMP);

        _currentLevelTMP = Get<TextMeshProUGUI>((int)TMPs.LEVELTMP);
        _addLevelTMP = Get<TextMeshProUGUI>((int)TMPs.AddLevelTMP);

        _currenDMGTMP = Get<TextMeshProUGUI>((int)TMPs.DMGTMP);
        _addDMGTMP = Get<TextMeshProUGUI>((int)TMPs.AddDmgTMP);

        _reductionTMP = Get<TextMeshProUGUI>((int)TMPs.ArmorReductionTMP);
        _AddreductionTMP = Get<TextMeshProUGUI>((int)TMPs.AddReductionTMP);

        _criDMGTMP = Get<TextMeshProUGUI>((int)TMPs.CriDMGTMP);
        _addCriDMGTMP = Get<TextMeshProUGUI>((int)TMPs.AddCriTMG);

        _criPerTMP = Get<TextMeshProUGUI>((int)TMPs.CriPercentTMP);
        _addCriPerTMP = Get<TextMeshProUGUI>((int)TMPs.AddCriPecentTMP);

        _frame = GetImage((int)Images.ItemFrame);
        _icon = GetImage((int)Images.ItemIcon);

        _gemSlider = Get<Slider>((int)Sliders.GemSlider);
        _goldSlider = Get<Slider>((int)Sliders.GoldSlider);

        _gem10Slider = Get<Slider>((int)Sliders.Gem10Slider);
        _gold10Slider = Get<Slider>((int)Sliders.Gold10Slider);


        UpdateUI();

    }


    Tween _iconPunch;
    Tween _framePunch;
    void GoldUpgradeButtonClick(PointerEventData data)
    {

        if (_coTimer != null)
        {
            return;
        }

        bool ok = Managers.Reinforce.GoldUpgrade(_id);
        if (ok == true && _isReinAnim == false)
        {
            _isReinAnim = true;
            UpdateUI();
            {
                GameObject go = Managers.Resource.Instantiate(Managers.Reinforce.UpgradeEffect);
                go.transform.SetParent(_icon.transform);
                go.transform.localPosition = Vector3.zero;


                PunchEffect();

                _coTimer = StartCoroutine(Timer(go));

            }

        }

    }


    void Gold10UpgradeButtonClick(PointerEventData data)
    {

        if (_coTimer != null)
        {
            return;
        }

        bool ok = Managers.Reinforce.Gold10Upgrade(_id);
        if (ok == true && _isReinAnim == false)
        {
            _isReinAnim = true;
            UpdateUI();
            {
                GameObject go = Managers.Resource.Instantiate(Managers.Reinforce.UpgradeEffect);
                go.transform.SetParent(_icon.transform);
                go.transform.localPosition = Vector3.zero;


                PunchEffect();

                _coTimer = StartCoroutine(Timer(go));

            }

        }

    }


    void Gem10UpgradeButtonClick(PointerEventData data)
    {
        if (_coTimer != null)
        {
            return;
        }


        bool ok = Managers.Reinforce.Gem10Upgrade(_id);
        if (ok == true && _isReinAnim == false)
        {
            _isReinAnim = true;
            UpdateUI();
            {

                GameObject go = Managers.Resource.Instantiate(Managers.Reinforce.UpgradeEffect);
                go.transform.SetParent(_icon.transform);
                go.transform.localPosition = Vector3.zero;

                PunchEffect();

                _coTimer = StartCoroutine(Timer(go));
            }

        }
    }

    void GemUpgradeButtonClick(PointerEventData data)
    {
        if (_coTimer != null)
        {
            return;
        }


        bool ok = Managers.Reinforce.GemUpgrade(_id);
        if (ok == true && _isReinAnim == false)
        {
            _isReinAnim = true;
            UpdateUI();
            {

                GameObject go = Managers.Resource.Instantiate(Managers.Reinforce.UpgradeEffect);
                go.transform.SetParent(_icon.transform);
                go.transform.localPosition = Vector3.zero;

                PunchEffect();

                _coTimer = StartCoroutine(Timer(go));
            }

        }
    }

    Coroutine _coTimer;

    IEnumerator Timer(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        _isReinAnim = false;
        //Destroy(go);
        _coTimer = null;
        //Clear();

    }



    void Clear()
    {
        if (_iconPunch != null)
        {
            _iconPunch.Kill(true);
            _iconPunch = null;
        }
        if (_framePunch != null)
        {
            _framePunch.Kill(true);
            _framePunch = null;
        }
        if (_coTimer != null)
        {
            StopCoroutine(_coTimer);
            _coTimer = null;
        }
    }


    void PunchEffect()
    {
        // 1. 크기 줄이기
        // GetImage((int)Images.ItemIcon).transform.DOScale(new Vector3(0.7f, 0.7f, 0.3f), 0.5f)
        //.SetEase(Ease.InOutQuad)
        //.OnComplete(() =>
        //{

        // RectTransform rectTransform = GetComponent<RectTransform>();



        // 2. 펀치 스케일 효과 적용
        _iconPunch = _icon.transform?.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.4f, 1, 1f)
        .SetEase(Ease.OutElastic)
        .OnComplete(() =>
        {
            if (_icon != null && _icon.transform != null)
            {
                // 3. 원래 크기로 돌아가기
                _icon.transform.DOScale(Vector3.one, 0.2f)
            .SetEase(Ease.InOutQuad);
            }

        }).SetLink(_icon?.gameObject);

        // 2. 펀치 스케일 효과 적용
        _framePunch = _frame.transform?.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.4f, 1, 1f)
            .SetEase(Ease.OutElastic)
            .OnComplete(() =>
            {
                if (_frame != null && _frame.transform != null)
                {
                    // 3. 원래 크기로 돌아가기
                    _frame.transform.DOScale(Vector3.one, 0.2f)
                            .SetEase(Ease.InOutQuad);
                }
            })
            .SetLink(_frame?.gameObject);

        //});

    }


    void OnClickUpgradeCloseButton(PointerEventData pointEventData)
    {

        Clear();
        update.Invoke();
        StartCoroutine(c_Clear());
    }

    IEnumerator c_Clear()
    {
        yield return new WaitForSeconds(0.2f);
        Managers.UI.ClosePopupUI(this);
    }



    void UpdateUI()
    {
        _goldUpgradeTMP.text = CUtil.BigIntegerFormatNumber(Managers.Reinforce.RequrieGoldCheck(_id));
        _gemUpgradeTMP.text = Managers.Reinforce.RequireGemCheck(_id).ToString();


        long gold10UpgradeCost = Managers.Reinforce.Get10GoldUpgrade(_id);
        long gem10UpgradeCost = Managers.Reinforce.Get10GemUpgrade(_id);

        _gold10UpgradeTMP.text = CUtil.BigIntegerFormatNumber(gold10UpgradeCost);
        _gem10UpgradeTMP.text = CUtil.BigIntegerFormatNumber(gem10UpgradeCost);

        ItemScriptbale iso = null;
        ItemData idata = null;

        if (Managers.Data.ItemDic.TryGetValue(_id, out iso))
        {
            _grade.text = iso.Grade.ToString();
            _frame.sprite = Managers.Data.Frames[(int)iso.Grade];
            _icon.sprite = iso.itemIcon;
        }
        else
            return;

        if (Managers.Game.InventoryData.item.TryGetValue(_id, out idata))
        {
            _lv.text = "LV." + idata.reinforce.ToString();
            _currentLevelTMP.text = "LV : " + idata.reinforce.ToString();
            _addLevelTMP.text = "+1";

            _currenDMGTMP.text = "DMG : " + idata.itemDamage.ToString();
            _addDMGTMP.text = "+" + (Managers.Reinforce.CalReinforceDMG(idata)<=0 ? 1 : Managers.Reinforce.CalReinforceDMG(idata));
            

            _reductionTMP.text = " : " + idata.shieldAttack.ToString();
            _AddreductionTMP.text = "+" + Managers.Reinforce.CalShiedAttack(_id);

            if (idata.itemCriticalPercent >= 100)
            {
                _criPerTMP.text = "CRI : 100%";
                _addCriPerTMP.text = "+0";
            }
            else
            {
                _criPerTMP.text = "CRI : " + idata.itemCriticalPercent.ToString("F2") + "%";
                _addCriPerTMP.text = "+" + ((idata.itemCriticalPercent * Managers.Reinforce.CalCriPer(_id)) <= 0 ? 0.1f : idata.itemCriticalPercent * Managers.Reinforce.CalCriPer(_id)).ToString("F2") + "%";
            }


            _criDMGTMP.text = "CRI DMG : " + (idata.itemCriticalPlusDamage* 100).ToString("F2")+"%";
            _addCriDMGTMP.text = "+" + (idata.itemCriticalPlusDamage * Managers.Reinforce.CalCriDmg(_id)*100).ToString("F2") +"%";


        }
        else
            return;


        float goldRatio = 0;
        float gemRatio = 0;

        float gold10Ratio = 0;
        float gem10Ratio = 0;

        goldRatio = Managers.Game.Gold / (float)Managers.Reinforce.RequrieGoldCheck(_id);
        gemRatio = Managers.Game.Gem / (float)Managers.Reinforce.RequireGemCheck(_id);

        gold10Ratio = (float)Managers.Game.Gold / gold10UpgradeCost;
        gem10Ratio = Managers.Game.Gem / (float)gem10UpgradeCost;

        _goldSlider.value = goldRatio;
        _gemSlider.value = gemRatio;

        _gold10Slider.value = gold10Ratio;
        _gem10Slider.value = gem10Ratio;

    }




}
