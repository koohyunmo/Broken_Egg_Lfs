using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UI_InventoryPopup : UI_Base
{
    public enum GameObjects
    {
        InventoryPanel,
        Scrollbar,
        ItemInfoPanel
    }
    public enum Buttons
    {
        InventoryQuitButton,
        DamageFilterButton,
        GradeFilterButton,
        ReinFilterButton,
        ReinforceAllButton,
    }

    Define.ItemType _inventoryFilter = Define.ItemType.Equip;

    const string _itemName = "UI_Inven_Item";
    const string _itemName2 = "UI_Inven_Item_(CARD_NO_BG)";
    const string _itemName3 = "UI_Inven_Item_CARD2";
    const string _testItemName = "UI_Inven_Item_2";
    const string _inventoryItem = "Card(Mini)";

    public Scrollbar _inventoryScrollbar;


    bool isRecycle;

    [SerializeField]List<UI_Item> invenItemList = new List<UI_Item>();

    int count;
    GameObject gridPanel;

    enum SubFilter
    {
        Grade,
        Dmg,
        Rein,
        Name,
    }

    
    private void OnEnable()
    {
        if (isRecycle)
            MakeNoneImageInventoryItem(_inventoryFilter);
        //InventoryItemFilterUpdate(_inventoryFilter);
        else
        {
            Init();
            MakeNoneImageInventoryItem(_inventoryFilter);
            // InventoryItemFilterUpdate(_inventoryFilter);
            //LoadItemTest();
        }
    }

    private void OnDisable()
    {
        _inventoryFilter = Define.ItemType.Equip;
    }


    public override void Init()
    {
        //base.Init();
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        // category filter
        //GetButton((int)Buttons.EquipFilterButton).gameObject.BindEvent(EquipFilterButton_Click);
        //GetButton((int)Buttons.UseFilterButton).gameObject.BindEvent(UseFilterButton_Click);
        //GetButton((int)Buttons.EtcFilterButton).gameObject.BindEvent(EtcFilterButton);
        //GetButton((int)Buttons.BoxFilterButton).gameObject.BindEvent(BoxFilterButton_Click);

        // sub filter
        GetButton((int)Buttons.GradeFilterButton).gameObject.BindEvent(GradeSubFilterButton_Click);
        GetButton((int)Buttons.DamageFilterButton).gameObject.BindEvent(DMGSubFilterButton_Click);
        GetButton((int)Buttons.ReinFilterButton).gameObject.BindEvent(ReinSubFilterButton_Click);
        //GetButton((int)Buttons.NameFilterButton).gameObject.BindEvent(NameSubFilterButton_Click);

        GetButton((int)Buttons.ReinforceAllButton).gameObject.BindEvent(ReinforceAll);

        // quit
        GetButton((int)Buttons.InventoryQuitButton).gameObject.BindEvent(OnClickPopupClose); // extension 메소드

        count = Managers.Game.InventoryData.item.Count;
        gridPanel = GetObject((int)GameObjects.InventoryPanel);

        isRecycle = true;
    }


    void EquipFilterButton_Click(PointerEventData data)
    {
        _inventoryFilter = Define.ItemType.Equip;
        //InventoryItemFilterUpdate(_inventoryFilter);
        MakeNoneImageInventoryItem(_inventoryFilter);

    }
    void UseFilterButton_Click(PointerEventData data)
    {
        _inventoryFilter = Define.ItemType.USE;
        MakeNoneImageInventoryItem(_inventoryFilter);
    }
    void EtcFilterButton(PointerEventData data)
    {
        _inventoryFilter = Define.ItemType.ETC;
        MakeNoneImageInventoryItem(_inventoryFilter);
    }
    void BoxFilterButton_Click(PointerEventData data)
    {
        _inventoryFilter = Define.ItemType.Chest;
        MakeNoneImageInventoryItem(_inventoryFilter);
    }

    void GradeSubFilterButton_Click(PointerEventData data)
    {
        StartCoroutine(InventoryItemSubFilterUpdate(SubFilter.Grade));
    }

    void DMGSubFilterButton_Click(PointerEventData data)
    {
        StartCoroutine(InventoryItemSubFilterUpdate(SubFilter.Dmg));
    }

    void ReinSubFilterButton_Click(PointerEventData data)
    {
        StartCoroutine(InventoryItemSubFilterUpdate(SubFilter.Rein));
    }

    void NameSubFilterButton_Click(PointerEventData data)
    {
        StartCoroutine(InventoryItemSubFilterUpdate(SubFilter.Name));
    }


    public void OnClickPopupClose(PointerEventData data)
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator InventoryItemSubFilterUpdate(SubFilter SF)
    {
        float startTime = Time.realtimeSinceStartup; // 시작 시간 기록

        //GameObject gridPanel = GetObject((int)GameObjects.InventoryPanel);
        GameObject inventoryItemPrefab = Managers.Resource.Load<GameObject>($"UI/SubItem/{_itemName3}");
        IOrderedEnumerable<string> filterItems;

        // 그리드 비우기
        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        GameObject infoPanel = GetObject((int)GameObjects.ItemInfoPanel).gameObject;

        switch (SF)
        {

            case SubFilter.Grade:
                var Gradeitems =
                from i in Managers.Game.InventoryData.hasItemList
                where Managers.Data.ItemDic[i].itemType == _inventoryFilter
                orderby (int)Managers.Game.InventoryData.item[i].itemGrade descending
                select i;
                filterItems = Gradeitems;
                break;
            case SubFilter.Dmg:
                var Dmgitems =
                from i in Managers.Game.InventoryData.hasItemList
                where Managers.Data.ItemDic[i].itemType == _inventoryFilter
                orderby Managers.Game.InventoryData.item[i].itemDamage descending
                select i;
                filterItems = Dmgitems;
                break;
            case SubFilter.Rein:
                var ReinItems =
                from i in Managers.Game.InventoryData.hasItemList
                where Managers.Data.ItemDic[i].itemType == _inventoryFilter
                orderby Managers.Game.InventoryData.item[i].reinforce descending
                select i;
                filterItems = ReinItems;
                break;
            case SubFilter.Name:
                var NameItems =
                from i in Managers.Game.InventoryData.hasItemList
                where Managers.Data.ItemDic[i].itemType == _inventoryFilter
                orderby Managers.Game.InventoryData.item[i].itemName ascending, StringComparer.CurrentCultureIgnoreCase
                select i;
                filterItems = NameItems;
                break;

            default: yield break;
        }

        invenItemList.Clear();

        foreach (string i in filterItems)
        {

            //GameObject item = Managers.UI.MakeSubItem<UI_Item>(gridPanel.transform).gameObject;
            //UI_Item inven_Item = item.GetOrAddComponent<UI_Item>();

            // 타입이 같으면 생성
            //ItemScriptbale itemSO = Managers.Data.ItemSO[i];
            //inven_Item.InitData(itemSO.itemID, infoPanel);
            //SetSize(inven_Item.transform);

            GameObject item = Instantiate(invenItem, gridPanel.transform);
            UI_Item inven_Item = item.GetOrAddComponent<UI_Item>();
            inven_Item.InitData(i, infoPanel);
            SetSize(gridPanel.transform);
            invenItemList.Add(inven_Item);

        }


        float endTime = Time.realtimeSinceStartup; // 종료 시간 기록
        float elapsedTime = endTime - startTime; // 실행 시간 계산
        Debug.Log("Coroutine elapsed time: " + elapsedTime + "s");

        yield return null;
    }


    IEnumerator MyCoroutine()
    {
        float startTime = Time.realtimeSinceStartup; // 시작 시간 기록

        // 실행할 코드 작성
        yield return null;

        float endTime = Time.realtimeSinceStartup; // 종료 시간 기록
        float elapsedTime = endTime - startTime; // 실행 시간 계산
        Debug.Log("Coroutine elapsed time: " + elapsedTime + "s");
    }

    void InventoryItemFilterUpdate(Define.ItemType inventoryType = Define.ItemType.Equip)
    {


        // 그리드 비우기
        GameObject gridPanel = GetObject((int)GameObjects.InventoryPanel);
        if (gridPanel == null)
            return;

        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);


        {

            var items =
                from i in Managers.Game.InventoryData.hasItemList
                where Managers.Data.ItemDic[i].itemType == inventoryType && i != Managers.Game.UseEquipment
                orderby Managers.Game.InventoryData.item[i].itemDamage descending
                select i;

            {
                if(inventoryType == Define.ItemType.Equip)
                {
                    GameObject item = Managers.UI.MakeSubItem<UI_Item>(gridPanel.transform).gameObject;
                    UI_Item inven_Item = item.GetOrAddComponent<UI_Item>();
                    ItemScriptbale itemSO = Managers.Data.ItemDic[Managers.Game.UseEquipment];
                    //inven_Item.InitData(itemSO);
                    
                }
            }
       
            foreach (string i in items)
            {

                    GameObject item = Managers.UI.MakeSubItem<UI_Item>(gridPanel.transform, _itemName3).gameObject;
                    UI_Item inven_Item = item.GetOrAddComponent<UI_Item>();

                    // 타입이 같으면 생성
                    ItemScriptbale itemSO = Managers.Data.ItemDic[i];
                    //inven_Item.InitData(itemSO);
                    

            }
            
        }



    }


    GameObject invenItem;
     async void MakeNoneImageInventoryItem(Define.ItemType inventoryType = Define.ItemType.Equip)
    {
        float startTime = Time.realtimeSinceStartup; // 시작 시간 기록

        GameObject infoPanel = GetObject((int)GameObjects.ItemInfoPanel).gameObject;


       

        // 그리드 비우기
        //GameObject gridPanel = GetObject((int)GameObjects.InventoryPanel);
        if (gridPanel == null)
            return;

        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        invenItemList.Clear();

        //23-03-21
        //card = Managers.Resource.Load<GameObject>("Prefabs/UI/SubItem/Card(Frame)");
        invenItem = Managers.Resource.Load<GameObject>("Prefabs/Item/UI_Item(180x180)");

        {

            /*
            var items =
                from i in Managers.Game.InventoryData.hasItemList
                where Managers.Data.ItemSO[i].itemType == _inventoryFilter && i != Managers.Game.UseEquipment
                orderby Managers.Game.InventoryData.item[i].itemDamage descending
                select i;
            */

            // 비동기
            var items = await GetFilteredInventoryAsync(_inventoryFilter);

            {
                if (_inventoryFilter == Define.ItemType.Equip)
                {
                    GameObject item = Instantiate(invenItem, gridPanel.transform);
                    UI_Item inven_Item = item.GetOrAddComponent<UI_Item>();
                    inven_Item.InitData(Managers.Game.UseEquipment, infoPanel);
                    SetSize(gridPanel.transform);
                    invenItemList.Add(inven_Item);

                    //GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item_Mini>(gridPanel.transform, _inventoryItem).gameObject;
                    //UI_Inven_Item_Mini inven_Item_Mini = item.GetOrAddComponent<UI_Inven_Item_Mini>();
                    //inven_Item_Mini.InitData(Managers.Game.UseEquipment,infoPanel);
                    //SetSize(item.transform);


                    {
                        //GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item_Sprite>(gridPanel.transform, _inventoryItem).gameObject;
                        //UI_Inven_Item_Sprite inven_Item = item.GetOrAddComponent<UI_Inven_Item_Sprite>();
                        //ItemScriptbale itemSO = Managers.Data.ItemSO[Managers.Game.UseEquipment];
                        //inven_Item.InitData(itemSO.name);
                        //SetSize(inven_Item.transform);
                    }

                }
            }

            foreach (string id in items)
            {
                //GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item_Mini>(gridPanel.transform, _inventoryItem).gameObject;
                //UI_Inven_Item_Mini inven_Item_Mini = item.GetOrAddComponent<UI_Inven_Item_Mini>();
                //inven_Item_Mini.InitData(id, infoPanel);
                //SetSize(item.transform);

                GameObject item = Instantiate(invenItem, gridPanel.transform);
                UI_Item inven_Item = item.GetOrAddComponent<UI_Item>();
                inven_Item.InitData(id, infoPanel);
                invenItemList.Add(inven_Item);

                SetSize(gridPanel.transform);

                {
                    //GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item_Sprite>(gridPanel.transform, _inventoryItem).gameObject;
                    //UI_Inven_Item_Sprite inven_Item = item.GetOrAddComponent<UI_Inven_Item_Sprite>();
                    //
                    //// 타입이 같으면 생성
                    //ItemScriptbale itemSO = Managers.Data.ItemSO[i];
                    //inven_Item.InitData(itemSO.name);
                    //
                    //SetSize(inven_Item.transform);

                    //inven_Item.transform.SetParent(gridPanel.transform);
                    //inven_Item.GetComponent<Transform>().localPosition = Vector3.zero;
                    ////go.GetComponent<Transform>().localScale = new Vector3(250, 400, 0);
                    //inven_Item.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
                }

            }

        }
        float endTime = Time.realtimeSinceStartup; // 종료 시간 기록
        float elapsedTime = endTime - startTime; // 실행 시간 계산
        Debug.Log("Coroutine elapsed time: " + elapsedTime + "s");

    }

    async Task<IEnumerable<string>> GetFilteredInventoryAsync(Define.ItemType _inventoryFilter)
    {
        return await Task.Run(() =>
        {
            return from i in Managers.Game.InventoryData.hasItemList
                   where Managers.Data.ItemDic[i].itemType == _inventoryFilter && i != Managers.Game.UseEquipment
                   orderby Managers.Game.InventoryData.item[i].itemDamage descending
                   select i;
        });
    }



    void SetSize(Transform tr)
    {
        tr.transform.SetParent(gridPanel.transform);
        tr.GetComponent<Transform>().localPosition = Vector3.zero;
        //go.GetComponent<Transform>().localScale = new Vector3(250, 400, 0);
        tr.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
    }

    void ReinforceAll(PointerEventData data)
    {
        foreach(UI_Item i in invenItemList )
        {
            bool ok = Managers.Reinforce.CanReinforce(i.Id);

            if(ok)
            {
                i.ReinforceEffect();
                ReinforceEffect(i.Id);
            }
        }

    }

    void ReinforceEffect(string id)
    {
        Debug.Log($"강화성공 : {id}");
    }


}
