using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemInformationPopup : UI_Popup
{

    enum Buttons
    {
        CloseInformationButton,
        InformationBuyButton,
    }

    enum GameObjects
    {
        InfromationGrid,
    }

    enum TMPS
    {
        ItemNameText,
        CountTMP,
        PriceTMP,
    }

    enum Images
    {
        ItemFrame,
        ItemIcon
    }

    private void Start()
    {
        Init();
    }

    [SerializeField]string _id;
    string _name;
    Define.Grade _grade;
    Define.ItemType _itemType = Define.ItemType.None;
    Define.GetRouteItemType _getType = Define.GetRouteItemType.None;

    Define.TapType _tapType;

    Action _buyEvent;


    List<string> _itemDataList = new List<string>();
    public Action ParentClicked;

    public void InitData(string id, Define.TapType taptype, Action buyEvent)
    {
        _id = id;
        _name = Managers.Data.ItemDic[id].itemName;
        _grade = Managers.Data.ItemDic[id].Grade;
        _itemType = Managers.Data.ItemDic[id].itemType;
        _getType = Managers.Data.ItemDic[id].getRouteItemType;
        _tapType = taptype;

        _buyEvent = buyEvent;

        if (Managers.Data.ItemDic[_id].itemType == Define.ItemType.ETC)
            _tapType = Define.TapType.None;



        _itemDataList.Add(_id);
        _itemDataList.Add(_name);
        _itemDataList.Add(_grade.ToString());
        _itemDataList.Add(_itemType.ToString());
        _itemDataList.Add(_getType.ToString());
    }

    public override void Init()
    {
        BindButton(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TMPS));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));
        GetButton((int)Buttons.CloseInformationButton).gameObject.BindEvent((PoninterEventData) => {
            Managers.UI.ClosePopupUI();
        });


        GetButton((int)Buttons.InformationBuyButton).gameObject.BindEvent((PoninterEventData) => {
            _buyEvent?.Invoke();
            UpdateIFP();
        });

        //_text.text = $"<color=white>-{numStr}</color> <color=white>{code}</color>";
        Get<TextMeshProUGUI>((int)TMPS.ItemNameText).text = _name;
        Get<TextMeshProUGUI>((int)TMPS.ItemNameText).color = CUtil.GetGradeColor(_id);
        GetImage((int)Images.ItemFrame).sprite = Managers.Data.Frames[(int)_grade];
        GetImage((int)Images.ItemIcon).sprite = Managers.Data.ItemDic[_id].itemIcon;

        gameObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<RectTransform>().transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0);
        gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(2, 2, 1);

        SetButton();
        SetInformationText();
        UpdateIFP();
    }


    private void UpdateIFP()
    {
        Get<TextMeshProUGUI>((int)TMPS.PriceTMP).text = Managers.Market.GetPriceString(_id);

        int count = Managers.Market.GetCount(_id);

        if (count > 0)
        {
            Get<TextMeshProUGUI>((int)TMPS.CountTMP).text = "Count : " + Managers.Market.GetCount(_id).ToString();
            GetButton((int)Buttons.InformationBuyButton).gameObject.GetComponent<Image>().color = Color.white;
        }
        else
        {
            Get<TextMeshProUGUI>((int)TMPS.CountTMP).text = "Sold Out";
            GetButton((int)Buttons.InformationBuyButton).gameObject.GetComponent<Image>().color = Color.gray;
        }
    }

    public List<string> MakeInfoData(string id)
    {
        List<string> list = new List<string>();

        if(Managers.Data.ItemDic.TryGetValue(id ,out ItemScriptbale itemData))
        {
            list.Add("Grade : " + itemData.Grade.ToString());     
        }

        switch (_itemType)
        {
            case Define.ItemType.None:
                break;
            case Define.ItemType.Equip:
                list.Add("Damage : " + itemData.damage.ToString());
                list.Add("Cri : " + itemData.criticalPercent.ToString() + "%");
                list.Add("Cri Dmg : " + (itemData.criticalDamage * 100).ToString("F2") + "%");
                list.Add("Ammor Reduction : " + Managers.Reinforce.CalShiedAttack(_id).ToString("F2") + "%");
                break;
            case Define.ItemType.Weapon:
                list.Add("Damage : " + itemData.damage.ToString());
                list.Add("Cri : " + itemData.criticalPercent.ToString()+"%");
                list.Add("Ammor Reduction : " + Managers.Reinforce.CalShiedAttack(_id).ToString("F2") + "%");
                break;
            case Define.ItemType.Ingredient:
                break;
            case Define.ItemType.Chest:
                break;
            case Define.ItemType.USE:
                break;
            case Define.ItemType.ETC:
                break;
            case Define.ItemType.BOX:
                break;
            case Define.ItemType.GOLD:
                break;
            case Define.ItemType.GEM:
                break;

        }

        switch (_tapType)
        {
            case Define.TapType.None:
                break;
            case Define.TapType.Market:
                list.Add("Price : " + itemData.itemCost.ToString());
                break;
            case Define.TapType.Craft:
                break;
            case Define.TapType.Inventory:
                break;
            case Define.TapType.Chest:
                break;
        }


        return list;

    }

    private void SetInformationText()
    {
        GameObject gridPanel = GetObject((int)GameObjects.InfromationGrid);
        _itemDataList = MakeInfoData(_id);

        if (gridPanel == null)
            return;

        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        for(int i =0; i < _itemDataList.Count; i++)
        {
            GameObject go = Managers.UI.MakeSubItem<UI_InformationText>(gridPanel.transform).gameObject;
            UI_InformationText uI_InformationText = go.GetOrAddComponent<UI_InformationText>();
            if (_itemDataList[i] == "" || _itemDataList[i] == null)
                continue;
            uI_InformationText.InitData(_itemDataList[i]);
        }

    }

    private void SetButton()
    {
        GetButton((int)Buttons.InformationBuyButton).gameObject.SetActive(false);

        switch (_tapType)
        {
            case Define.TapType.None:
                break;
            case Define.TapType.Market:
                GetButton((int)Buttons.InformationBuyButton).gameObject.SetActive(true);
                break;
        }

        if(_itemType == Define.ItemType.Chest)
        {
            GetButton((int)Buttons.InformationBuyButton).gameObject.SetActive(false);
        }
    }
}