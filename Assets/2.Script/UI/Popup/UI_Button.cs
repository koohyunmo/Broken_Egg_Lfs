using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    enum Buttons
    {
        SaveTestButton,
        LoadSaveTestButton,
        StartStatTestButton,
        DTButton,
        GetAllButton,
        ClearAllStage,
        ClearAllStage2,
        AddGold,
        AddGem,
        AddUnique,
        AddLegend,
        AddHero,
        AddRare,
        AddCommon,
        ClearText,
        ClearButtons,
        COUNT,
    }

    enum Texts
    {
        StatTest,
        EggStat,
        FPSText,
    }
    Text playerData;
    Text eggData;
    Text fpsText;


    private void Start()
    {
        Init();
    }


    public override void Init()
    {
        base.Init();

        // c# ���÷���
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        //Test
        GetButton((int)Buttons.SaveTestButton).gameObject.BindEvent(OnClickSaveTestButton); // extension �޼ҵ�
        GetButton((int)Buttons.LoadSaveTestButton).gameObject.BindEvent(OnClickLoadSaveTestButton); // extension �޼ҵ�
        GetButton((int)Buttons.StartStatTestButton).gameObject.BindEvent(OnClickStartStatTestButton); // extension �޼ҵ�
        GetButton((int)Buttons.DTButton).gameObject.BindEvent(OnClickDamageTestButton); // extension �޼ҵ�
        GetButton((int)Buttons.GetAllButton).gameObject.BindEvent(OnClickAllItemAddButton); // extension �޼ҵ�
        GetButton((int)Buttons.ClearAllStage).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.Game.Clear50Stage();
            Managers.Game.SaveGame("Clear50Stage");
            //
        }); // extension �޼ҵ�

        GetButton((int)Buttons.ClearAllStage2).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.Game.ClearAllStage();
            Managers.Game.SaveGame("ClearAllStage");
            //
        }); // extension �޼ҵ�

        GetButton((int)Buttons.AddGold).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.Game.Add1MGold();
            Managers.Game.SaveGame("AddGold");
            //
        }); // extension �޼ҵ�
        GetButton((int)Buttons.AddGem).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.Game.Add1MGem();
            Managers.Game.SaveGame("AddGem");
            //
        }); // extension �޼ҵ�


        GetButton((int)Buttons.AddHero).gameObject.BindEvent((PointerEventData data) => { Managers.Game.AddHere(); }); // extension �޼ҵ�
        GetButton((int)Buttons.AddLegend).gameObject.BindEvent((PointerEventData data) => { Managers.Game.AddLegend(); }); // extension �޼ҵ�
        GetButton((int)Buttons.AddRare).gameObject.BindEvent((PointerEventData data) => { Managers.Game.AddRare(); }); // extension �޼ҵ�
        GetButton((int)Buttons.AddUnique).gameObject.BindEvent((PointerEventData data) => { Managers.Game.AddUnique(); }); // extension �޼ҵ�
        GetButton((int)Buttons.AddCommon).gameObject.BindEvent((PointerEventData data) => { Managers.Game.AddCommon(); }); // extension �޼ҵ�

        GetButton((int)Buttons.ClearButtons).gameObject.BindEvent((PointerEventData) =>
        {
            for (int i = 0; i < (int)Buttons.COUNT; i++)
            {
                if (((int)Buttons.SaveTestButton + i) == (int)Buttons.ClearButtons || ((int)Buttons.SaveTestButton + i) == (int)Buttons.ClearText)
                    continue;

                if (GetButton((int)Buttons.SaveTestButton + i).gameObject.activeSelf)
                {
                    GetButton((int)Buttons.SaveTestButton + i).gameObject.SetActive(false);
                }
                else
                {
                    GetButton((int)Buttons.SaveTestButton + i).gameObject.SetActive(true);
                }
            }
            //
        }); // extension �޼ҵ�




        playerData = GetText((int)Texts.StatTest);
        eggData = GetText((int)Texts.EggStat);
        fpsText = GetText((int)Texts.FPSText);

        //BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag); // extension �޼ҵ�X

        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        gameObject.GetComponent<Canvas>().planeDistance = 15;


        StartCoroutine(c_UpdateUI());

        GetButton((int)Buttons.ClearText).gameObject.BindEvent((PointerEventData) =>
        {
            if (eggData.gameObject.activeSelf) // ���� ������Ʈ�� �����ִٸ�
            {
                eggData.gameObject.SetActive(false); // ����
                playerData.gameObject.SetActive(false); // ����
            }
            else // ���� ������Ʈ�� �����ִٸ�
            {
                eggData.gameObject.SetActive(true); // �Ҵ�
                playerData.gameObject.SetActive(true); // �Ҵ�
            }

            //
        }); //

    }

    public void OnClickSaveTestButton(PointerEventData data)
    {
        Managers.Game.AllDataRefresh();
        Managers.Game.SaveGame("OnClickSaveTestButton");
    }
    public async void OnClickLoadSaveTestButton(PointerEventData data)
    {
        Managers.Game.AllDataRefresh();
        await Managers.Game.LoadGame();
    }
    public void OnClickStartStatTestButton(PointerEventData data)
    {
        Managers.Game.FirstStartStat();
        Managers.Game.AllDataRefresh();
        Managers.Stage.StageMapCheck();
    }
    public void OnClickDamageTestButton(PointerEventData data)
    {
        Managers.Game.TestDamageUp();
        Managers.Game.AllDataRefresh();
    }

    public void OnClickAllItemAddButton(PointerEventData data)
    {
        foreach (string key in Managers.Data.ItemDic.Keys)
        {
            Managers.Game.Additem(Managers.Data.ItemDic[key].itemID);
        }
    }

    private void Update()
    {
        UpdateFPS();
    }

    int index = 0;

    IEnumerator c_UpdateUI()
    {


        while (true)
        {
            yield return new WaitForSeconds(0.02f);

            if (Managers.Game.StageData.currentStage > 50)
            {
                index = Managers.Data.EggImageIndex;
            }
            else
            {
                index = Managers.Game.StageData.currentStage;
            }


            eggData.text = $"EGG DATA" +
            $"\nSTAGE : {Managers.Game.StageData.clearStage}" +
            $"\nCURRENTSTAGE : {Managers.Game.StageData.currentStage}" +
            $"\nNAME: {Managers.Data.EggDic[index].eggName}" +
            $"\nHP : {Managers.Game.StageData.currentHp}/{Managers.Game.StageData.maxHp}" +
            $"\nID : {index}" +
            $"\nKILLCOUNT : {Managers.Game.StageData.killCount}";
            playerData.text = $"PLAYER DATA " +
            $"\nPLAYER_LELVEL : {Managers.Game.Level} " +
            $"\nPLAYER_EXP : {Managers.Game.EXP} " +
            $"\nPLAYER_CURRENTEXP : {Managers.Game.CurrentExp} " +
            $"\nPLAYER_MAXEXP : {Managers.Game.MaxExp} " +
            $"\nPLAYER_DAMAGE : {(long)(Managers.Game.PlayerDamage * Managers.Game.EquipItemData.itemDamage)} " +
            $"\nPLAYER_PLAYERDAMAGE : {Managers.Game.PlayerDamage} " +
            $"\nPLAYER_GOLD : {Managers.Game.Gold}" +
            $"\nEQUIP_ITEM_ID : {Managers.Game.UseEquipment}" +
            $"\nITEM_DAMAGE : {Managers.Game.TotalDamage}" +
            $"\nEQUIP_ITEM_RF : { Managers.Game.EquipItemData.reinforce}" +
            $"\nEQUIP_ITEM_itemDamage : { Managers.Game.EquipItemData.itemDamage}" +
            $"\nEQUIP_ITEM_itemType : { Managers.Game.EquipItemData.itemType}" +
            $"\nEQUIP_ITEM_itemCriticalPercent : { Managers.Game.EquipItemData.itemCriticalPercent}" +
            $"\nEQUIP_ITEM_itemCriticalPlusDamage : { Managers.Game.EquipItemData.itemCriticalPlusDamage}" +
            $"\nEQUIP_ITEM_itemCount : { Managers.Game.EquipItemData.itemCount}" +
            $"\nCalculateDamage : { Managers.Game.CalculateDamage()}" +
            $"\nGoldBonus : { Managers.Game.PlayerData.goldBonus}" +
            $"\nXpBonus : { Managers.Game.PlayerData.xpBonus}" +
            $"\nItemDropRate : { Managers.Game.PlayerData.itemDropRate}" +
            $"\nUpgradedamageUpLevel : { Managers.Game.PlayerUpgradeData.damageUpLevel}" +
            $"\ngoldBonusLevel : { Managers.Game.PlayerUpgradeData.goldBonusLevel}" +
            $"\nxpBonusLevel : { Managers.Game.PlayerUpgradeData.xpBonusLevel}" +
            $"\nitemDropRateLevel : { Managers.Game.PlayerUpgradeData.itemDropRateLevel}";


        }


    }

    private float deltaTime = 0f;
    private void UpdateFPS()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        float ms = deltaTime * 1000f;
        float fps = 1.0f / deltaTime;

        fpsText.text = $"FPS : {(int)fps} MS : {(int)ms}";

    }


}
