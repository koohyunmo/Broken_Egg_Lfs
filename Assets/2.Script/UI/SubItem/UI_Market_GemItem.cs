using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Market_GemItem : UI_Base
{

    bool _isInit;
    string _id;
    ItemScriptbale _itemData;

    enum GameObjects
    {
        GemPriceText,
        CountText,

    }

    enum Images
    {
        MarketItemIcon,
        MarketItemFrame,
        InformationButton
    }

    enum Buttons
    {
        MarketBuyButton,
    }

    enum Sliders
    {
        GemSlider,
    }

    TextMeshProUGUI _countTMP;
    TextMeshProUGUI _priceTMP;

    Slider _gemSlider;

    Action _action;

    public void InitData(ItemScriptbale itemSO,ref UI_MarketPopup.ItemUpdateDele Marketdelegate, Action action)
    {
        _id = itemSO.itemID;
        _itemData = itemSO;
        _action = action;
        Marketdelegate -= GemItemUpdate;
        Marketdelegate += GemItemUpdate;
        Init();
    }
    public override void Init()
    {
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        Bind<Slider>(typeof(Sliders));

        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);

        GetImage((int)Images.MarketItemIcon).sprite = _itemData.itemIcon;
        GetImage((int)Images.MarketItemFrame).sprite = Managers.Data.Frames[(int)_itemData.Grade];


        Action _gemEvent = () =>
        {
            Managers.Market.BuyGemItem(_id);
            GemItemUpdate();
            _action?.Invoke();
        };


        GetImage((int)Images.MarketItemFrame).gameObject.BindEvent((PointerEventData) =>
        {
            GameObject go = Managers.UI.ShowPopupUI<UI_ItemInformationPopup>(null, transform.parent.parent).gameObject;
            UI_ItemInformationPopup itemInformationPopup = go.GetOrAddComponent<UI_ItemInformationPopup>();
            itemInformationPopup.InitData(_id, Define.TapType.Market, _gemEvent);
        });

        GetImage((int)Images.InformationButton).gameObject.BindEvent((PointerEventData) =>
        {
            GameObject go = Managers.UI.ShowPopupUI<UI_ItemInformationPopup>(null, transform.parent.parent).gameObject;
            UI_ItemInformationPopup itemInformationPopup = go.GetOrAddComponent<UI_ItemInformationPopup>();
            itemInformationPopup.InitData(_id, Define.TapType.Market, _gemEvent);
        });

        GetButton((int)Buttons.MarketBuyButton).gameObject.BindEvent((PointerEventData) => {
            Managers.Market.BuyGemItem(_id);
            GemItemUpdate(); 
            _action?.Invoke(); 
        });

        _gemSlider = Get<Slider>((int)Sliders.GemSlider);


        _countTMP = GetObject((int)GameObjects.CountText).GetComponent<TextMeshProUGUI>();
        _priceTMP = GetObject((int)GameObjects.GemPriceText).GetComponent<TextMeshProUGUI>();

        GemItemUpdate();

        

        _isInit = true;
    }

    private void GemItemUpdate()
    {

        int count = Managers.Market.GetGemCount(_id);
        if (count == 0)
            _countTMP.text = " Sold Out ";
        else
            _countTMP.text = "Count : " + count;

        _priceTMP.text = Managers.Market.GetGemPrice(_id).ToString();

        float ratio = Managers.Game.Gem / (float)Managers.Market.GetGemPrice(_id);

        ratio = Mathf.Clamp(ratio, 0.0f, 1.0f);

        _gemSlider.value = ratio;

    }




}
