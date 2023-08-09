using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Market_Item : UI_Base
{
    [SerializeField] string _name;
    [SerializeField] string _id;
    [SerializeField] bool _hasEffect;
    [SerializeField] Image _image;
    [SerializeField] Sprite _icon;
    [SerializeField] int _price;
    [SerializeField] GameObject _parent;
    [SerializeField] Define.Grade _grade;

    enum GameObjects
    {
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
        GoldSlider,
    }

    enum TMPS
    {
        PriceText
    }

    Action updateAllItems;

    public void InitData(ItemScriptbale data, ref UI_MarketPopup.ItemUpdateDele Marketdelegate, Action action)
    {
        

        _name = data.itemName;
        _price = data.itemCost;
        _icon = data.itemIcon;
        _id = data.itemID;
        _grade = data.Grade;

        Init();

        Marketdelegate -= UpdateUI;
        Marketdelegate += UpdateUI;
        
        updateAllItems = null;
        updateAllItems = action;


        UpdateUI();

    }

    public void InitData(ItemScriptbale data)
    {
        

        _name = data.itemName;
        _price = data.itemCost;
        _icon = data.itemIcon;
        _id = data.itemID;
        _grade = data.Grade;

        Init();
        UpdateUI();

    }

    Image _frame;
    Slider _goldSlider;

    bool _init;

    Button button;


    private void OnEnable()
    {
        if (_init == true)
            UpdateUI();
    }
    

    public override void Init()
    {
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        Bind<Slider>(typeof(Sliders));
        Bind<TextMeshProUGUI>(typeof(TMPS));

       
        button = GetButton((int)Buttons.MarketBuyButton);
        button.gameObject.BindEvent(OnClickBuyButton); // extension 메소드
        _goldSlider = Get<Slider>((int)Sliders.GoldSlider); // extension 메소드
        GetParent();

        this.gameObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<RectTransform>().transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0);


        Action _goldBuyEvent = () =>
        {
            Managers.Market.BuyItem(_id);
            updateAllItems?.Invoke();
        };

        GetImage((int)Images.MarketItemFrame).gameObject.BindEvent((PointerEventData) => 
        {
            GameObject go = Managers.UI.ShowPopupUI<UI_ItemInformationPopup>(null, _parent.transform).gameObject;
            UI_ItemInformationPopup itemInformationPopup = go.GetOrAddComponent<UI_ItemInformationPopup>();
            itemInformationPopup.InitData(_id,Define.TapType.Market, _goldBuyEvent);
        });

        GetImage((int)Images.InformationButton).gameObject.BindEvent((PointerEventData) =>
        {
            GameObject go = Managers.UI.ShowPopupUI<UI_ItemInformationPopup>(null, _parent.transform).gameObject;
            UI_ItemInformationPopup itemInformationPopup = go.GetOrAddComponent<UI_ItemInformationPopup>();
            itemInformationPopup.InitData(_id, Define.TapType.Market, _goldBuyEvent);
        });

        gameObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);


        GetImage((int)Images.MarketItemIcon).sprite = _icon;
        GetImage((int)Images.MarketItemFrame).sprite = Managers.Data.Frames[(int)_grade];

        _init = true;
    }

    private void GetParent()
    {
        //_parent = GetObject((int)GameObjects.CraftPopup).gameObject;

        if (_parent == null)
            _parent = GameObject.Find("MarketPopup");
    }

    private void OnClickBuyButton(PointerEventData data)
    {
        Managers.Market.BuyItem(_id);
        updateAllItems?.Invoke();
    }

    private void UpdateUI()
    {

        if (_init == false)
            return;

        if (_goldSlider == null)
            return;


        float ratio = 0;

        if (Managers.Game.Gold >= Managers.Market.GetPriceInt(_id))
        {
            ratio = 1;
        }
        else
        {
            ratio = Managers.Game.Gold / (float)Managers.Market.GetPriceInt(_id);
        }
 
        _goldSlider.value = ratio;

        Get<TextMeshProUGUI>((int)TMPS.PriceText).text = Managers.Market.GetPriceString(_id);

        int count = Managers.Market.GetCount(_id);

        if (count > 0)
        {
            GetObject((int)GameObjects.CountText).GetComponent<TextMeshProUGUI>().text = "Count : "+Managers.Market.GetCount(_id).ToString();
            button.GetComponent<Image>().color = Color.white;
        } 
        else
        {
            GetObject((int)GameObjects.CountText).GetComponent<TextMeshProUGUI>().text = "Sold Out";
            _goldSlider.value = 0;
            button.GetComponent<Image>().color = Color.gray;
        }

        if (ratio < 1 )
            button.GetComponent<Image>().color = Color.gray;


    }
}
