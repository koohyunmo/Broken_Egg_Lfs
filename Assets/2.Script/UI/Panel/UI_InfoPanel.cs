using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InfoPanel : UI_Base
{

    enum Images
    {
        ClickItemIcon
    }

    enum GameObjects
    {
        ClickItemInfoTMP
    }

    enum Buttons
    {
        ItemEquipButton,
        ReinforceButton

    }

    string _id;
    Image _icon; 
    TextMeshProUGUI _infoTMP;
    bool _init;
    Button _euipButton;


    private void OnEnable()
    {
        if (_init == false)
            Init();
        else
        {
            _id = Managers.Game.UseEquipment;

            if (_id == null)
                _icon.gameObject.SetActive(false);
            else
                GetID(_id);
        }
    }

    private void OnDisable()
    {
        _id = null;
        _icon.sprite = null;
        _infoTMP.text = null;

        OffButtons();
    }

    public override void Init()
    {
        _id = Managers.Game.UseEquipment;

        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        _infoTMP = GetObject((int)GameObjects.ClickItemInfoTMP).GetComponent<TextMeshProUGUI>();
        _icon = GetImage((int)Images.ClickItemIcon);

        _icon.gameObject.SetActive(false);

         _euipButton = GetButton((int)Buttons.ItemEquipButton);
        // 장착
        GetButton((int)Buttons.ItemEquipButton).gameObject.BindEvent((PointerEventData) => 
        {
            Managers.Game.ChangeEquipEffect(_id);

            string soundName = "Weapon_Draw_Metal_10";

            if (_id == Managers.Game.UseEquipment)
                ChangeAlpha();
            else
                ChangeAlpha(1);

            Managers.Sound.Play(soundName);

        });

        // 강화
        GetButton((int)Buttons.ReinforceButton).gameObject.BindEvent((PointerEventData) =>
        {

            GameObject go = Managers.UI.ShowPopupUI<UI_Upgrade_Popup>().gameObject;
            UI_Upgrade_Popup up = go.GetComponent<UI_Upgrade_Popup>();
            up.InitData(_id, UpdateText);

            //TODO
            Debug.Log("강화버튼");
        });


        GetID(Managers.Game.UseEquipment);

        _init = true;
    }

    public void GetID(string id)
    {
        _id = id;
        _icon.sprite = Managers.Data.ItemDic[_id].itemIcon;
        _icon.gameObject.SetActive(true);

        if (_id == Managers.Game.UseEquipment)
            ChangeAlpha();
        else
            ChangeAlpha(1);




        _infoTMP.text = $"{Managers.Game.InventoryData.item[_id].itemName} + {Managers.Game.InventoryData.item[_id].reinforce} \n" +
            $"Damage : {Managers.Game.InventoryData.item[_id].itemDamage}";

        OffButtons();

        switch (Managers.Data.ItemDic[id].itemType)
        {
            case Define.ItemType.None:
                break;
            case Define.ItemType.Equip:
                GetButton((int)Buttons.ItemEquipButton).gameObject.SetActive(true);
                GetButton((int)Buttons.ReinforceButton).gameObject.SetActive(true);
                break;
            case Define.ItemType.Weapon:
                GetButton((int)Buttons.ItemEquipButton).gameObject.SetActive(true);
                break;
            case Define.ItemType.Ingredient:
                break;
            case Define.ItemType.Chest:
                break;
            case Define.ItemType.USE:
                break;
            case Define.ItemType.ETC:
                break;
        }
    }

    void ChangeAlpha(float alpha = 0.5f)
    {
        Color oldColor = _euipButton.GetComponent<Image>().color;
        _euipButton.GetComponent<Image>().color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
    }


    void OffButtons()
    {
        GetButton((int)Buttons.ItemEquipButton).gameObject.SetActive(false);
        GetButton((int)Buttons.ReinforceButton).gameObject.SetActive(false);
    }

    void UpdateText()
    {
        _infoTMP.text = $"{Managers.Game.InventoryData.item[_id].itemName} + {Managers.Game.InventoryData.item[_id].reinforce} \n" +
    $"Damage : {Managers.Game.InventoryData.item[_id].itemDamage}";
    }

}
