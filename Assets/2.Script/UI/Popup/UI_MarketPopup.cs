using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MarketPopup : UI_Popup
{
    enum GameObjects
    {
        MarketPanel,
        MarketContents,
        SpecialMarket,
        GemMarket,
        GemMarketContents,
        SpecialMarketPanel

    }
    enum Buttons
    {
        SpecialMarketButton,
        ItemMarketButton,
        AdResetButton,
        GemMarketButton,
        AdGemResetButton

    }
    enum Texts
    {
        MarketAutoResetTimer,
        CurrentItemText,
        NextItemText,
        GemTimer
    }

    private void Start()
    {
        Init();
    }

    enum CurrentTab
    {
        NONE,
        Item,
        Speical,
        END
    }

    [SerializeField] Dictionary<string, int> currentItem = new Dictionary<string, int>();
    [SerializeField] Dictionary<string, int> nextItem = new Dictionary<string, int>();

    enum ClickPanel
    {
        Item,
        Gem,
        Special
    }


    ClickPanel cp = ClickPanel.Item;


    public delegate void ItemUpdateDele();

    private ItemUpdateDele UpdateItems;
    private ItemUpdateDele UpdateItems2;
    

    public override void Init()
    {

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));


        UpdateTimerUI();

        GetButton((int)Buttons.AdResetButton).gameObject.BindEvent((PointerEventData) => {
            Action a = () =>
            {
                Managers.Market.ClickAdList(); 
                //MarketItemListUpdate();
                UpdateTimerUI();
                MarketItemUpdate();
                
            };

            Managers.AD.GetMaketAdResetReward(a);
            Managers.Game.SaveGame("AdButton");

        });

        GetButton((int)Buttons.AdGemResetButton).gameObject.BindEvent((PointerEventData) => {
            Action a = () =>
            {
                Managers.Market.ClickGemAdList(); 
                MarketGemItemUpdate();
                UpdateTimerUI();
                MarketItemUpdate();
            };

            Managers.AD.GetMaketAdResetReward(a);
            Managers.Game.SaveGame("AdButton");

        });



        //Shops
        GetObject((int)GameObjects.SpecialMarket).gameObject.SetActive(false);
        GetObject((int)GameObjects.GemMarket).gameObject.SetActive(false);



        // Button

        GetButton((int)Buttons.SpecialMarketButton).gameObject.BindEvent(OnClickSpecialMarketButton);
        GetButton((int)Buttons.ItemMarketButton).gameObject.BindEvent(OnClickItemMarketButton);
        GetButton((int)Buttons.GemMarketButton).gameObject.BindEvent(OnClickGemMarketButton);
        

        MarketItemUpdate();
        MarketGemItemUpdate();
        SpecialMarketItemUpdate();
        //MarketItemListUpdate();


        //StartCoroutine(c_UpdateTimerUI());

    }

    IEnumerator c_UpdateTimerUI()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (cp == ClickPanel.Item)
                GetText((int)Texts.MarketAutoResetTimer).text = Managers.Market.MarketTimeRemainStr(2);
            else if(cp == ClickPanel.Gem)
            {
                GetText((int)Texts.GemTimer).text = Managers.Market.MarketTimeRemainStr(3);
            }

            if (Managers.Market.MarketTimeCompare(Managers.Game.MarketData.autoResetTime) == true)
            {
                AutoReset();
            }

            if (Managers.Market.MarketTimeCompare(Managers.Game.MarketData.adResetTime) == true)
            {
                GemReset();
            }
        }

    }

    private void OnEnable()
    {
        StartCoroutine(c_UpdateTimerUI());
    }

    private void OnDisable()
    {
        StopCoroutine(c_UpdateTimerUI());
    }

    private void UpdateTimerUI()
    {
        GetText((int)Texts.MarketAutoResetTimer).text = Managers.Market.MarketTimeRemainStr(2);
    }

    void MarketItemUpdate()
    {


        //GameObject gridPanel = Get<GameObject>((int)GameObjects.MarketContents);

        //if (gridPanel == null)
        //    return;

        //foreach (Transform child in gridPanel.transform)
        //    Managers.Resource.Destroy(child.gameObject);


        //foreach (string keys in Managers.Game.MarketData.currentItem.Keys)
        //{
        //    GameObject item2 = Managers.UI.MakeSubItem<UI_Market_Item>(gridPanel.transform).gameObject;
        //    UI_Market_Item market_Item2 = item2.GetOrAddComponent<UI_Market_Item>();


        //    // ���������� ����
        //    if (Managers.Data.ItemDic.TryGetValue(keys, out ItemScriptbale itemSO))
        //        market_Item2.InitData(itemSO, ref UpdateItems, ItemUpdate);
        //    else
        //    {
        //        Managers.Resource.Destroy(item2);
        //        continue;
        //    }
        //}


        StartCoroutine(UpdateDelay());

    }

    IEnumerator UpdateDelay()
    {

        GameObject gridPanel = Get<GameObject>((int)GameObjects.MarketContents);

        if (gridPanel == null)
            yield return null;

        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        yield return new WaitForSeconds(0.1f);

        foreach (string keys in Managers.Game.MarketData.currentItem.Keys)
        {
            GameObject item2 = Managers.UI.MakeSubItem<UI_Market_Item>(gridPanel.transform).gameObject;
            UI_Market_Item market_Item2 = item2.GetOrAddComponent<UI_Market_Item>();


            // ���������� ����
            if (Managers.Data.ItemDic.TryGetValue(keys, out ItemScriptbale itemSO))
                market_Item2.InitData(itemSO, ref UpdateItems, ItemUpdate);
            else
            {
                Managers.Resource.Destroy(item2);
                continue;
            }
        }

    }

    void SpecialMarketItemUpdate()
    {
        GameObject gridPanel = Get<GameObject>((int)GameObjects.SpecialMarketPanel);

        if (gridPanel == null)
            return;

        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);


        foreach (string keys in Managers.Data.ChestList)
        {
            GameObject item2 = Managers.UI.MakeSubItem<UI_SpecialItem>(gridPanel.transform).gameObject;
            UI_SpecialItem market_Item2 = item2.GetOrAddComponent<UI_SpecialItem>();


            // ���������� ����
            if (Managers.Data.ItemDic.TryGetValue(keys, out ItemScriptbale itemSO))
                market_Item2.InitData(itemSO);
            else
            {
                Managers.Resource.Destroy(item2);
                continue;
            }
        }


    }


    IEnumerator UpdateDelay_Gem()
    {

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GemMarketContents);

        if (gridPanel == null)
            yield return null;

        yield return new WaitForSeconds(0.1f);

        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        foreach (string keys in Managers.Game.MarketData.gemItem.Keys)
        {
            GameObject item2 = Managers.UI.MakeSubItem<UI_Market_GemItem>(gridPanel.transform).gameObject;
            UI_Market_GemItem market_Item2 = item2.GetOrAddComponent<UI_Market_GemItem>();


            // ���������� ����
            if (Managers.Data.ItemDic.TryGetValue(keys, out ItemScriptbale itemSO))
                market_Item2.InitData(itemSO, ref UpdateItems2, ItemUpdate);
            else
            {
                Managers.Resource.Destroy(item2);
                continue;
            }
        }

    }


    void MarketGemItemUpdate()
    {

        StartCoroutine(UpdateDelay_Gem());

        //GameObject gridPanel = Get<GameObject>((int)GameObjects.GemMarketContents);

        //if (gridPanel == null)
        //    return;

        //foreach (Transform child in gridPanel.transform)
        //    Managers.Resource.Destroy(child.gameObject);

        //foreach (string keys in Managers.Game.MarketData.gemItem.Keys)
        //{
        //    GameObject item2 = Managers.UI.MakeSubItem<UI_Market_GemItem>(gridPanel.transform).gameObject;
        //    UI_Market_GemItem market_Item2 = item2.GetOrAddComponent<UI_Market_GemItem>();


        //    // ���������� ����
        //    if (Managers.Data.ItemDic.TryGetValue(keys, out ItemScriptbale itemSO))
        //        market_Item2.InitData(itemSO, ref UpdateItems2, ItemUpdate);
        //    else
        //    {
        //        Managers.Resource.Destroy(item2);
        //        continue;
        //    }
        //}

    }

    void ItemUpdate()
    {
        UpdateItems?.Invoke();
        UpdateItems2?.Invoke();
    }

    void OnClickMarketResetButton(PointerEventData data)
    {

        Managers.Market.ClickFreeReset();
        MarketItemListUpdate();
        UpdateTimerUI();
        MarketItemUpdate();

    }

    void OnClickItemMarketButton(PointerEventData data)
    {
        CloseAllPanel();
        cp = ClickPanel.Item;
        GetObject((int)GameObjects.MarketPanel).SetActive(true);

    }

    void OnClickGemMarketButton(PointerEventData data)
    {
        CloseAllPanel();
        cp = ClickPanel.Gem;
        GetObject((int)GameObjects.GemMarket).SetActive(true);

    }

    void OnClickSpecialMarketButton(PointerEventData data)
    {

        CloseAllPanel();
        cp = ClickPanel.Special;
        GetObject((int)GameObjects.SpecialMarket).SetActive(true);
    }


    void CloseAllPanel()
    {
        GetObject((int)GameObjects.SpecialMarket).gameObject.SetActive(false);
        GetObject((int)GameObjects.MarketPanel).gameObject.SetActive(false);
        GetObject((int)GameObjects.GemMarket).gameObject.SetActive(false);
    }

    void MarketItemListUpdate()
    {

        currentItem = Managers.Game.MarketData.currentItem;
        nextItem = Managers.Game.MarketData.nextItem;

        GetText((int)Texts.CurrentItemText).text = "";
        GetText((int)Texts.NextItemText).text = "";

        foreach (string s in currentItem.Keys)
        {
            GetText((int)Texts.CurrentItemText).text += s + " ";
        }
        foreach (string s in nextItem.Keys)
        {
            GetText((int)Texts.NextItemText).text += s + " ";
        }

    }

    void AutoReset()
    {

        Managers.Market.AutoReset();
        MarketItemListUpdate();
        UpdateTimerUI();
        MarketItemUpdate();

    }

    void GemReset()
    {
        Managers.Market.AutoGemReset();
        MarketGemItemUpdate();
        UpdateTimerUI();
        MarketGemItemUpdate();
    }


}
