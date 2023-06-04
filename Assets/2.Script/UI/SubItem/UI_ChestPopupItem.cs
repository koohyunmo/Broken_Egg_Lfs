using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChestPopupItem : UI_Base
{

    enum Images
    {
        ChestIcon,
        Chest_Item_Icon_Frame
    }


    enum Buttons
    {
        Chest_Open_Button
    }

    enum GameObjects
    {
        ChestButtonLock,
        ChestCountSlider,
        ChestCountTMP
    }

    [SerializeField] string _id;
    GameObject _parent;
    Image _chestIcon;
    Slider _chestCountSlider;
    TextMeshProUGUI _tmp;
    Image _chestOpenButtonImage;
    Image _frame;
    GameObject _lock;

    Action _caller;


    public void InitData(string id, GameObject parent)
    {
        _id = id;
        _parent = parent;

        Init();

    }

    public void InitData(string id, GameObject parent,ref UI_ChestPopup.ItemUIUpdate itemUpdate,Action action)
    {
        _id = id;
        _parent = parent;
        

        Init();

        itemUpdate -= ChestUIUpdate;
        itemUpdate += ChestUIUpdate;

        _caller = null;
        _caller = action;

        ChestUIUpdate();

    }

    ItemData _itemData;
    ItemScriptbale _itemSO;


    public override void Init()
    {
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        _chestIcon = GetImage((int)Images.ChestIcon).GetComponent<Image>();
        _frame = GetImage((int)Images.Chest_Item_Icon_Frame).GetComponent<Image>();
        _chestCountSlider = GetObject((int)GameObjects.ChestCountSlider).GetComponent<Slider>();
        _lock = GetObject((int)GameObjects.ChestButtonLock).gameObject;
        _tmp = GetObject((int)GameObjects.ChestCountTMP).GetComponent<TextMeshProUGUI>();


        

        _chestOpenButtonImage = GetButton((int)Buttons.Chest_Open_Button).gameObject.GetComponent<Image>();

        GetButton((int)Buttons.Chest_Open_Button).gameObject.BindEvent((PointerEventData) =>
        {
            if (_itemData != null && _itemData.itemCount > 0 )
            {
                UI_DynamicGacha dg = Managers.UI.ShowPopupUI<UI_DynamicGacha>().gameObject.GetComponent<UI_DynamicGacha>();
                dg.InitData(_id, _caller);
                ChestUIUpdate();
            }
            else
            {
                return;
            }
                

        });

       
    }

    private void OnEnable()
    {
        if (_chestCountSlider != null)
            ChestUIUpdate();
    }



    private void ChestUIUpdate()
    {

        Managers.Game.InventoryData.item.TryGetValue(_id, out ItemData invenData);
        Managers.Data.ItemDic.TryGetValue(_id, out ItemScriptbale itemSO);

        _chestIcon.sprite = itemSO.itemIcon;
        _itemData = invenData;
        _itemSO = itemSO;
        _frame.sprite = Managers.Data.Frames[(int)itemSO.Grade];
        float ratio = 0;

        if (invenData == null)
        {
            _tmp.text = 0.ToString() + "/" + "1";
            ratio = 0 / 1f;
        }
        else
        {
            _tmp.text = invenData.itemCount.ToString() + "/" + "1";
            ratio = invenData.itemCount / 1f;
        }

        if(ratio >= 1)
        {
            _chestOpenButtonImage.color = Color.green;
            _lock.SetActive(false);
        }
        else
        {
            _chestOpenButtonImage.color = Color.gray;
            _lock.SetActive(true);

        }

        _chestCountSlider.value = ratio;
    }

}
