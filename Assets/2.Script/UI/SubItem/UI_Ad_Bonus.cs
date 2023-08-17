using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Ad_Bonus : UI_Base
{

    

    public enum ADType
    {
        Gem,
        Gold,
        Chest
    }

    enum Images
    {
        Ad_Item_Icon,
        Ad_Item_Icon_Frame

    }

    enum Buttons
    {
        Ad_Front_Button
    }

    string rewardId;

    public ADType _type;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        switch (_type)
        {
            case ADType.Gem:
                rewardId = "GM0003";
                break;
            case ADType.Gold:
                rewardId = "GD0002";
                break;
            case ADType.Chest:
                rewardId = "CH0004";
                break;
        }

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        int index = (int)Managers.Data.ItemDic[rewardId].Grade;
        GetImage((int)Images.Ad_Item_Icon).sprite = Managers.Data.ItemDic[rewardId].itemIcon;
        GetImage((int)Images.Ad_Item_Icon_Frame).sprite = Managers.Data.Frames[index];

        Action a = () => {
            Managers.Game.Additem(rewardId);
            StartCoroutine(enumerator(rewardId));
        };

        GetButton((int)Buttons.Ad_Front_Button).gameObject.BindEvent((PointerEventData data) => {

            Managers.AD.GetMaketAdResetReward(a);
        });
    }

    IEnumerator enumerator(string id)
    {
        yield return new WaitForSeconds(1f);
        var popup = Managers.UI.ShowPopupUI<UI_Reward_Popup>();

        if (id == "CH0004")
            popup.UpdateChestRewardPopup(rewardId);
        else
            popup.UpdateUI(rewardId);

        yield return null;
        Debug.Log($"Get a {rewardId}");
    }
}