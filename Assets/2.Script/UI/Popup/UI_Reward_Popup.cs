using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UI_PlayPopup;

public class UI_Reward_Popup : UI_Popup
{
    enum Images
    {
       Ad_Item_Icon,
       Ad_Item_Icon_Frame
    }

    enum TMPS
    {
        Ad_Item_Grade,
    }

    enum Buttons
    {
        OkButton,
        CloseInformationButton
    }

    public void Init()
    {
        BindImage(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(TMPS));
        BindButton(typeof(Buttons));


    }

    public void UpdateUI(string id)
    {
        int index = (int)Managers.Data.ItemDic[id].Grade;
        GetImage((int)Images.Ad_Item_Icon).sprite = Managers.Data.ItemDic[id].itemIcon;
        GetImage((int)Images.Ad_Item_Icon_Frame).sprite = Managers.Data.Frames[index];
        GetButton((int)Buttons.OkButton).gameObject.BindEvent((PointerEventData data) => { ClosePopupUI(); });
        GetButton((int)Buttons.CloseInformationButton).gameObject.BindEvent((PointerEventData data) => { ClosePopupUI(); });
    }
}
