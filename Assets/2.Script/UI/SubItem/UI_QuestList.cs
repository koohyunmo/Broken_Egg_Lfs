using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestList : UI_Base
{
    enum Images
    {
        QuestIcon,
        RewardButton,
        RewardFrame,
        RewardIcon
    }

    enum GameObjests
    {
        QuestTitleTMP,
        QuestCountTMP,
        LevelTMP,
        QuestSlider
    }
    TextMeshProUGUI _titleTMP;
    TextMeshProUGUI _levelTMP;
    TextMeshProUGUI _countTMP;

    string _titleStr;
    Slider _slider;

    Sprite _icon;

    int _maxPoint;
    long _hasPoint;
    int _level;
    int _multiple;

    bool _init;

    Image _rewardFrame;
    Image _rewardIcon;

    string _rewardId;

    Define.QuestType _qt;

    Transform _target;

    GameObject rewardEffect;

    public void InitData(QuestScriptable qs)
    {
        _icon = qs.questIcon;
        _titleStr = qs.questTile;
        _maxPoint = qs.point;
        _qt = qs.questType;
        _multiple = qs.multiple;
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        transform.GetComponent<Transform>().localPosition = Vector3.zero;
        transform.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);

        BindObject(typeof(GameObjests));
        BindImage(typeof(Images));

        GetImage((int)Images.QuestIcon).sprite = _icon;

        _titleTMP = GetObject((int)GameObjests.QuestTitleTMP).gameObject.GetComponent<TextMeshProUGUI>();
        _countTMP = GetObject((int)GameObjests.QuestCountTMP).gameObject.GetComponent<TextMeshProUGUI>();
        _levelTMP = GetObject((int)GameObjests.LevelTMP).gameObject.GetComponent<TextMeshProUGUI>();
        _slider = GetObject((int)GameObjests.QuestSlider).gameObject.GetComponent<Slider>();

        _rewardFrame = GetImage((int)Images.RewardFrame).gameObject.GetComponent<Image>();
        _rewardIcon = GetImage((int)Images.RewardIcon).gameObject.GetComponent<Image>();

        rewardEffect = Resources.Load<GameObject>("Prefabs/RewardItem/RewardItem");

        GetImage((int)Images.RewardButton).gameObject.BindEvent((PointerEventData) => 
        {

            PointCheck();

            if (_hasPoint /_maxPoint >= 1)
            {


                MakeEffect();
                QuestLevelUP();
                UpdateUI();
                Managers.Game.SaveGame("QuestList");
            }
            UpdateUI();
        });





        _titleTMP.text = _titleStr;
        UpdateUI();

        _init = true;


    }

    Action b;

    void MakeEffect()
    {

        Transform startTr = _rewardFrame.transform;

        startTr.DOPunchScale(new Vector3(0.1f, 0.1f), 0.1f).OnComplete(() => 
        {
            startTr.localScale = new Vector3(1, 1, 1);
        });

        GameObject go = Managers.Resource.Instantiate(rewardEffect);
        go.GetComponent<Image>().sprite = _rewardIcon.sprite;
        go.transform.SetParent(transform.parent.parent.parent);


        ItemMovement im = go.GetOrAddComponent<ItemMovement>();
        Action a = () => Destroy(go);
        im.InitData(startTr, _target, a,go);

    }

    private void OnEnable()
    {
        if (_init == true)
            UpdateUI();
        
    }

    void UpdateUI()
    {
        RewardCheck();
        PointCheck();

        _countTMP.text = $"{_hasPoint}/{_maxPoint}";
        _slider.value = (float)(_hasPoint) / (_maxPoint);
        _levelTMP.text = $"LV.{_level + 1}";



    }

    void PointCheck()
    {
        switch (_qt)
        {
            case Define.QuestType.Click:
                _hasPoint = Managers.Game.PlayerData.clickCount;
                _maxPoint = Managers.Quest.ClickCountQuest();
                _level = Managers.Game.QuestData.clickCountQuestLevel;
                _target = Managers.Loot.GoldIcon;
                break;
            case Define.QuestType.Gold:
                _hasPoint = Managers.Game.PlayerData.usingGold;
                _maxPoint = Managers.Quest.UseGoldQuest();
                _level = Managers.Game.QuestData.usingGoldQuestLevel;
                _target = Managers.Loot.GemIcon;
                break;
            case Define.QuestType.Gem:
                _hasPoint = Managers.Game.PlayerData.usingGem;
                _maxPoint = Managers.Quest.UseGemQuest();
                _level = Managers.Game.QuestData.usingGemQuestLevel;
                _target = Managers.Loot.InventoryButton;
                break;
            case Define.QuestType.Reinforce:
                _hasPoint = Managers.Game.PlayerData.reinforceCount;
                _maxPoint = Managers.Quest.ReinforceCountQuest();
                _level = Managers.Game.QuestData.reinforceCountGoldQuestLevel;
                _target = Managers.Loot.InventoryButton;
                break;
        }
    }

    void RewardCheck()
    {
        ItemScriptbale rewardData = null;

        switch (_qt)
        {
            case Define.QuestType.Click:
                Managers.Data.ItemDic.TryGetValue(Managers.Game.QuestData.clickRewardId, out rewardData);
                break;
            case Define.QuestType.Gold:
                Managers.Data.ItemDic.TryGetValue(Managers.Game.QuestData.usingGoldRewardId, out rewardData);
                break;
            case Define.QuestType.Gem:
                Managers.Data.ItemDic.TryGetValue(Managers.Game.QuestData.usingGemRewardId, out rewardData);
                break;
            case Define.QuestType.Reinforce:
                Managers.Data.ItemDic.TryGetValue(Managers.Game.QuestData.reinforceRewardId, out rewardData);
                break;
        }

        _rewardId = rewardData.itemID;
        _rewardFrame.sprite = Managers.Data.Frames[(int)rewardData.Grade];
        _rewardIcon.sprite = rewardData.itemIcon;
    }

    void QuestLevelUP()
    {
        switch (_qt)
        {
            case Define.QuestType.Click:
                Managers.Game.ClickQuestLevelUP(_maxPoint,_rewardId);
                break;
            case Define.QuestType.Gold:
                Managers.Game.GoldQuestLevelUP(_maxPoint, _rewardId);
                break;
            case Define.QuestType.Gem:
                Managers.Game.GemQuestLevelUP(_maxPoint, _rewardId);
                break;
            case Define.QuestType.Reinforce:
                Managers.Game.ReinforceQuestLevelUP(_maxPoint, _rewardId);
                break;
        }

    }
}
