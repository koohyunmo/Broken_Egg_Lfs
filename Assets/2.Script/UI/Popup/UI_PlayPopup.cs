using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Coffee.UIExtensions;

public class UI_PlayPopup : UI_Popup
{
    const int INVENTORY_COUNT = 28;

    enum TMPS
    {
        StageText,
        LevelText,
        MoneyText,
        GemText,
        DpsTMP,
        ClickDmgTMP,
        CriDmgTMP
    }
    enum Buttons
    {
        SettingButtonSide,
        MarketPopupQuitButton,
        InventoryQuitButton,
        CraftPopupQuitButton,
        MarketButton,
        InventoryButton,
        ItemCraftButton,
        ChestTapButton,
        ChestPopupQuitButton,
        PrevStageButton,
        NextStageButton,
        AdButton,
        AdPopupQuitButton,
        CashShopButton,
        CashShopPopupQuitButton,
        GemAddButton,
        AdButtonSide,
        OptionQuitButton
        //TODO
    }

    enum GameObjects
    {
        InventoryPanel,
        LevelUp,
        MarketPanel,
        MarketPopup,
        SettingPopup,
        InventoryPopup,
        QuestPopup,
        ChestPopup,
        AdPopup,
        CashShopPopup,
        InventoryTapBG,
        CraftTapBG,
        MarketTapBG,
        ChestTapBG,
        AdTapBG,
        BackGroundImage,
        Monster,
        KillCountTMP,
        TimeSlider,
        //TODO
    }


    public enum PlayTab
    {
        None,
        Market,
        Setting,
        Chest,
        Craft,
        Inventory,
        Ad,
        CashShop,
    }


    public enum Images
    {
        StageEgg,
        BackGround,
        EquipFrame,
        EquipItemIcon
    }

    TextMeshProUGUI _stageText, _levelText, _moneyText, _gemText, _dpsTMP, _clickDPS, _criDPS;


    int _level;

    [SerializeField] PlayTab _playTab;
    [SerializeField] Camera _lootCam;


    /// <summary>
    /// 인벤토리 0 크래프트 1 마켓 2 플레이어 업그레이드 3 광고 4
    /// </summary>
    [SerializeField]List<Image> tabBgs = new List<Image>();
    Color bgColor = new Color(1, 1, 1, 0.4f);
    // background
    [SerializeField]RectTransform _backgroundRect;

    int currentStage = 0;

    TextMeshProUGUI _killCount;
    Image _stageEggIcon;

    private void Start()
    {
        Init();

    }

    public override void Init()
    {
        base.Init();

        _playTab = PlayTab.None;




        // Bind
        Bind<TextMeshProUGUI>(typeof(TMPS));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));
        //BindObject(typeof(Particles));

        // Buttons
        GetButton((int)Buttons.CashShopButton).gameObject.BindEvent(OnClickCashShopButton); // extension 메소드
        GetButton((int)Buttons.GemAddButton).gameObject.BindEvent(OnClickCashShopButton); // extension 메소드
        GetButton((int)Buttons.SettingButtonSide).gameObject.BindEvent(OnClickSettingButton); // extension 메소드

        //Tab Button
        GetButton((int)Buttons.MarketButton).gameObject.BindEvent(OnClickMarketButton); // extension 메소드
        GetButton((int)Buttons.InventoryButton).gameObject.BindEvent(OnClickInventoryButton); // extension 메소드
        GetButton((int)Buttons.ItemCraftButton).gameObject.BindEvent(OnClickCraftButton); // extension 메소드
        GetButton((int)Buttons.ChestTapButton).gameObject.BindEvent(OnClickUpgradeButton); // extension 메소드
        GetButton((int)Buttons.AdButton).gameObject.BindEvent(OnClickAdpopup);

#if UNITY_ANDROID
        GetButton((int)Buttons.GemAddButton).gameObject.SetActive(true);
#else
        GetButton((int)Buttons.GemAddButton).gameObject.SetActive(false);
#endif

        GetObject((int)GameObjects.InventoryTapBG).gameObject.BindEvent(OnClickInventoryButton);
        GetObject((int)GameObjects.CraftTapBG).gameObject.BindEvent(OnClickCraftButton);
        GetObject((int)GameObjects.MarketTapBG).gameObject.BindEvent(OnClickMarketButton);
        GetObject((int)GameObjects.ChestTapBG).gameObject.BindEvent(OnClickUpgradeButton);
        GetObject((int)GameObjects.AdTapBG).gameObject.BindEvent(OnClickAdpopup);
        


        {
            tabBgs.Add(GetObject((int)GameObjects.InventoryTapBG).GetComponent<Image>());
            tabBgs.Add(GetObject((int)GameObjects.CraftTapBG).GetComponent<Image>());
            tabBgs.Add(GetObject((int)GameObjects.MarketTapBG).GetComponent<Image>());
            tabBgs.Add(GetObject((int)GameObjects.ChestTapBG).GetComponent<Image>());
            tabBgs.Add(GetObject((int)GameObjects.AdTapBG).GetComponent<Image>());
        }

        //Quit Button
        GetButton((int)Buttons.MarketPopupQuitButton).gameObject.BindEvent(OnClickPopupClose); // extension 메소드
        GetButton((int)Buttons.CraftPopupQuitButton).gameObject.BindEvent(OnClickPopupClose); // extension 메소드
        GetButton((int)Buttons.InventoryQuitButton).gameObject.BindEvent(OnClickPopupClose); // extension 메소드
        GetButton((int)Buttons.ChestPopupQuitButton).gameObject.BindEvent(OnClickPopupClose); // extension 메소드
        GetButton((int)Buttons.AdPopupQuitButton).gameObject.BindEvent(OnClickPopupClose); // extension 메소드
        GetButton((int)Buttons.CashShopPopupQuitButton).gameObject.BindEvent(OnClickPopupClose); // extension 메소드
        GetButton((int)Buttons.OptionQuitButton).gameObject.BindEvent(OnClickPopupClose); // extension 메소드

        //Inventory Filter Button
        //GetButton((int)Buttons.EquipFilterButton).gameObject.BindEvent(OnClickEquipFilter); // extension 메소드

        //Stage UI Button
        GetButton((int)Buttons.NextStageButton).gameObject.BindEvent(OnClickNextStageButton);
        GetButton((int)Buttons.PrevStageButton).gameObject.BindEvent(OnClickPrevStageButton);

        // Popup
        GetObject((int)GameObjects.MarketPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.SettingPopup).gameObject.SetActive(false); 
        GetObject((int)GameObjects.InventoryPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.QuestPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.ChestPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.AdPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.CashShopPopup).gameObject.SetActive(false);

        //Alarm
        //GetObject((int)GameObjects.InventoryAlarm).gameObject.SetActive(false);

        // Effects
        Managers.Game.SetLevelAction(LevelUP);

        // HUD
        _stageText = Get<TextMeshProUGUI>((int)TMPS.StageText);
        _levelText = Get<TextMeshProUGUI>((int)TMPS.LevelText);
        _moneyText = Get<TextMeshProUGUI>((int)TMPS.MoneyText);
        _gemText = Get<TextMeshProUGUI>((int)TMPS.GemText);
        _dpsTMP = Get<TextMeshProUGUI>((int)TMPS.DpsTMP);
        _clickDPS = Get<TextMeshProUGUI>((int)TMPS.ClickDmgTMP);
        _criDPS = Get<TextMeshProUGUI>((int)TMPS.CriDmgTMP);


        //Stage
        currentStage = Managers.Game.StageData.currentStage;

        int index = Managers.Game.StageData.currentStage % Managers.Data.EggDic.Count;
        GetImage((int)Images.StageEgg).sprite = Managers.Data.EggDic[index].eggImage;

        _stageEggIcon = GetImage((int)Images.StageEgg);
        _killCount = GetObject((int)GameObjects.KillCountTMP).gameObject.GetComponent<TextMeshProUGUI>();
        Managers.Stage.GetTimerSlider(GetObject((int)GameObjects.TimeSlider).gameObject);

        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        gameObject.GetComponent<Canvas>().planeDistance = 15;


        _level = Managers.Game.Level;

        _lootCam = GameObject.Find("LootCamera").GetComponent<Camera>();


        //Add Component
        GetObject((int)GameObjects.InventoryPopup).GetOrAddComponent<UI_InventoryPopup>();
        GetObject((int)GameObjects.QuestPopup).GetOrAddComponent<UI_QuestPopup>();
        GetObject((int)GameObjects.MarketPopup).GetOrAddComponent<UI_MarketPopup>();
        GetObject((int)GameObjects.ChestPopup).GetOrAddComponent<UI_ChestPopup>();
        //GetObject((int)GameObjects.SettingPopup).GetOrAddComponent<UI_SettingPopup>();

        //Get Comppnent

        if (GetObject((int)GameObjects.BackGroundImage).TryGetComponent<RectTransform>(out RectTransform rect))
        {
            _backgroundRect = rect;
        }

        StartCoroutine(c_UpdateUI());

        Managers.Stage.SetImage(GetImage((int)Images.BackGround).GetComponent<Image>());
        Managers.Stage.StageMapCheck();


        // 광고 버튼
        //GetButton((int)Buttons.AdButtonSide).gameObject.BindEvent((PointerEventData ped) => { Managers.AD.GetSideButtonReward(); });


        Managers.Sound.SetBGMVolume(0.5f);
        Managers.Sound.SetEFVolume(0.5f);


        currentEquip = Managers.Game.UseEquipment;
        Sprite icon = Managers.Data.ItemDic[currentEquip].itemIcon;
        GetImage((int)Images.EquipItemIcon).sprite = icon;
            GetImage((int)Images.EquipFrame).sprite = Managers.Data.Frames[(int)Managers.Game.EquipItemData.itemGrade];

            Debug.Log("PlayUI Init");

    }


    void DoTapSound()
    {
        Managers.Sound.Play("Sounds/Bag_01");
    }
    private void OnClickCashShopButton(PointerEventData obj)
    {
        Debug.Log("캐쉬샵");
        GetObject((int)GameObjects.CashShopPopup).gameObject.SetActive(true);
        _playTab = PlayTab.CashShop;
    }

    public void OnClickSettingButton(PointerEventData data)
    {
        Debug.Log("세팅");
        GetObject((int)GameObjects.SettingPopup).gameObject.SetActive(true);
        _playTab = PlayTab.Setting;
    }


    #region Button Event



    public void OnClickInventoryButton(PointerEventData data)
    {
        if (_playTab == PlayTab.Inventory)
            return;

        OnCliCkAllPopupClose();
        tabBgs[0].color = bgColor;
        GetObject((int)GameObjects.InventoryPopup).gameObject.SetActive(true);
        Managers.Game.CheckInventory();
        _playTab = PlayTab.Inventory;
    }
    public void OnClickCraftButton(PointerEventData data)
    {
        if (_playTab == PlayTab.Craft)
            return;

        OnCliCkAllPopupClose();
        tabBgs[1].color = bgColor;
        GetObject((int)GameObjects.QuestPopup).gameObject.SetActive(true);
        _playTab = PlayTab.Craft;
    }

    public void OnClickMarketButton(PointerEventData data)
    {
        if (_playTab == PlayTab.Market)
            return;

        OnCliCkAllPopupClose();
        tabBgs[2].color = bgColor;
        GetObject((int)GameObjects.MarketPopup).gameObject.SetActive(true);
        _playTab = PlayTab.Market;
    }

    public void OnClickUpgradeButton(PointerEventData data)
    {
        if (_playTab == PlayTab.Chest)
            return;
     
        OnCliCkAllPopupClose();
        tabBgs[3].color = bgColor;
        GetObject((int)GameObjects.ChestPopup).gameObject.SetActive(true);
        _playTab = PlayTab.Chest;
    }



    void OnClickAdpopup(PointerEventData data)
    {
        OnCliCkAllPopupClose();
        tabBgs[4].color = bgColor;
        GetObject((int)GameObjects.AdPopup).gameObject.SetActive(true);
        _playTab = PlayTab.Ad;
    }
    public void OnClickPopupClose(PointerEventData data)
    {
        GetObject((int)GameObjects.MarketPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.SettingPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.InventoryPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.QuestPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.ChestPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.AdPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.CashShopPopup).gameObject.SetActive(false);
        _playTab = PlayTab.None;

        for (int i = 0; i < tabBgs.Count; i++)
        {
            tabBgs[i].color = Color.white;
        }
    }
    void OnCliCkAllPopupClose()
    {
        GetObject((int)GameObjects.MarketPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.SettingPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.InventoryPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.QuestPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.ChestPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.AdPopup).gameObject.SetActive(false);
        GetObject((int)GameObjects.CashShopPopup).gameObject.SetActive(false);

        for (int i = 0; i < tabBgs.Count; i++)
        {
            tabBgs[i].color = Color.white;
        }
        DoTapSound();
    }

    void OnClickNextStageButton(PointerEventData data)
    {
        if(Managers.Game.StageData.currentStage < Managers.Game.StageData.clearStage)
        {
            if (Managers.Game.StageUp())
            {
                if (Managers.Game.eggTr.transform == null)
                {
                    Debug.LogWarning("NOT MONSTER");
                    return;
                }

                Managers.Game.eggTr.transform.DOScale(0f, 0f);
                Managers.Game.eggTr.transform.DOScale(1f, 0.5f);
                GetObject((int)GameObjects.BackGroundImage).transform.DOLocalMove(new Vector3(0, _backgroundRect.localPosition.y, 0), 0.5f).From(new Vector3(1200, _backgroundRect.localPosition.y, _backgroundRect.localPosition.z)).OnComplete(()=> {


                });
            }

        }
    }

    void OnClickNextStageButton()
    {
        if (Managers.Game.StageData.currentStage < Managers.Game.StageData.clearStage)
        {
            if (Managers.Game.StageUp())
            {
                if (Managers.Game.eggTr.transform == null)
                {
                    Debug.LogWarning("NOT MONSTER");
                    return;
                }

                Managers.Game.eggTr.transform.DOScale(0f, 0f);
                Managers.Game.eggTr.transform.DOScale(1f, 0.5f);
                GetObject((int)GameObjects.BackGroundImage).transform.DOLocalMove(new Vector3(0, _backgroundRect.localPosition.y, 0), 0.5f).From(new Vector3(1200, _backgroundRect.localPosition.y, _backgroundRect.localPosition.z)).OnComplete(() => {


                });
            }

        }
    }
    void OnClickPrevStageButton(PointerEventData data)
    {
        if (Managers.Game.StageData.clearStage >= 0 && Managers.Game.StageData.clearStage >= Managers.Game.StageData.currentStage)
        {
            if (Managers.Game.eggTr.transform == null)
            {
                Debug.LogWarning("NOT MONSTER");
                return;
            }


            if (Managers.Game.StageDown())
            {
                //MonsterController mc = Managers.Game.eggTr.parent.gameObject.GetComponent<MonsterController>();

                Managers.Game.eggTr.transform.DOScale(0f, 0f);
                Managers.Game.eggTr.transform.DOScale(1f, 0.5f);
                //mc.ChangeStageAndState();
                GetObject((int)GameObjects.BackGroundImage).transform.DOLocalMove(new Vector3(0, _backgroundRect.localPosition.y, 0), 0.5f).From(new Vector3(-1200, _backgroundRect.localPosition.y, _backgroundRect.localPosition.z)).OnComplete(()=> {

                });
            }

        }
    }

    #endregion

    void LevelUP()
    {
        GameObject go = Managers.Effect.LevelUpEffect();
        go.transform.SetParent(GetObject((int)GameObjects.LevelUp).transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = new Vector3(1, 1, 1);
        go.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 2,3);
        Managers.Sound.Play("Sounds/LevelUp04");
        _level = Managers.Game.Level;


    }

    string currentEquip;
    IEnumerator c_UpdateUI()
    {
        while (true)
        {
            _stageText.text = $" STAGE : {Managers.Game.StageData.currentStage}";
            _levelText.text = Managers.Game.Level.ToString();
            _moneyText.text = CUtil.LongFormatNumber(Managers.Game.Gold);
            _gemText.text = Managers.Game.Gem.ToString();
            _dpsTMP.text = "DPS : "+CUtil.LongFormatNumber(Managers.Stage.GetDPS());
            _clickDPS.text = "CLICK DMG : " + CUtil.LongFormatNumber((long)(Managers.Stage.GetCDmage())); ;
            _criDPS.text = "CRI DMG : " + CUtil.LongFormatNumber((long)(Managers.Stage.GetCriDMG())); ;





            if (currentEquip != Managers.Game.UseEquipment)
            {
                currentEquip = Managers.Game.UseEquipment;

                Sprite icon = Managers.Data.ItemDic[Managers.Game.UseEquipment].itemIcon;
                GetImage((int)Images.EquipItemIcon).sprite = icon;
                GetImage((int)Images.EquipFrame).sprite = Managers.Data.Frames[(int)Managers.Game.EquipItemData.itemGrade];
            }


            {
                if (Managers.Game.StageData.clearStage != Managers.Game.StageData.currentStage)
                {
                    _killCount.text = "CLEAR";
                }
                    
                else
                {
                    if (Managers.Game.StageData.currentStage % 5 == 0)
                        _killCount.text = "BOSS STAGE";

                    else
                        _killCount.text = $"{Managers.Game.StageData.killCount} / {Managers.Stage.ClearCount}";
                }

                if (currentStage != Managers.Game.StageData.currentStage)
                {
                    currentStage = Managers.Game.StageData.currentStage;
                    //TODO
                    if (Managers.Game.StageData.currentStage % 5 == 0)
                    {
                        _stageEggIcon.sprite = Managers.Stage.bossSprite;
                    }
                        
                    else
                        _stageEggIcon.sprite = Managers.Data.EggDic[Managers.Data.EggImageIndex].eggImage;

                }
            }



            if (Managers.Game.Level > _level)
            {
                //LevelUP();
            }

            if (_playTab == PlayTab.None)
            {
                //Managers.Loot.Root.SetActive(true);
                _lootCam.gameObject.SetActive(true);
                Managers.Effect.EffectCamOn();
            }

            else
            {
                //Managers.Loot.Root.SetActive(false);
                _lootCam.gameObject.SetActive(false);
                Managers.Effect.EffectCamOff();
            }

            yield return new WaitForSeconds(0.1f); // 일정 시간마다 업데이트
        }

        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }



    /*
    private void Update()
    {
        
        _stageText.text = $" Stage : {Managers.Game.StageData.currentStage}";
        _levelText.text = Managers.Game.Level.ToString();
        _moneyText.text = FormatNumber(Managers.Game.Gold);


        {
            if (Managers.Game.StageData.clearStage != Managers.Game.StageData.currentStage)
                _killCount.text = "Clear";
            else
            {
                if (Managers.Game.StageData.currentStage % 5 == 0)
                    _killCount.text = "Boss Stage";
                else
                    _killCount.text = $"{Managers.Game.StageData.killCount} / {Managers.Stage.ClearCount}";
            }

            if (currentStage != Managers.Game.StageData.currentStage)
            {
                currentStage = Managers.Game.StageData.currentStage;
                _stageEggIcon.sprite = Managers.Data.EggSO[currentStage].eggImage;
            }
        }



        if (Managers.Game.Level > _level)
        {
            LevelUP();
        }

        if (_playTab == PlayTab.None)
        {
            //Managers.Loot.Root.SetActive(true);
            _lootCam.gameObject.SetActive(true);
        }
        
        else
        {
            //Managers.Loot.Root.SetActive(false);
            _lootCam.gameObject.SetActive(false);
        }
        


    }
    */



}
