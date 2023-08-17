using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Reward_Popup : UI_Popup
{

    public Image _icon;
    public Image _frame;
    public Button _okButton;
    public Button _closeButton;
    public GameObject _chestInfo;



    public void UpdateChestRewardPopup(string id)
    {
        int index = (int)Managers.Data.ItemDic[id].Grade;
        _icon.sprite = Managers.Data.ItemDic[id].itemIcon;
        _frame.sprite = Managers.Data.Frames[index];
        _okButton.onClick.AddListener(() => { ClosePopupUI(); });
        _closeButton.onClick.AddListener(() => { ClosePopupUI(); });

        _chestInfo.gameObject.SetActive(true);
    }


    public void UpdateUI(string id)
    {
        int index = (int)Managers.Data.ItemDic[id].Grade;
        _icon.sprite = Managers.Data.ItemDic[id].itemIcon;
        _frame.sprite = Managers.Data.Frames[index];
        _okButton.onClick.AddListener(() => { ClosePopupUI(); });
        _closeButton.onClick.AddListener(() => { ClosePopupUI(); });

        _chestInfo.gameObject.SetActive(false);
    }
}

