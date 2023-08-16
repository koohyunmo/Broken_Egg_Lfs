using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Ad_Bonus : UI_Base
{
    [SerializeField] AdPopupController _adPopupController;

    enum ChestTMP
    {
        ChestAdTimer,
    }

    enum GoldTMP
    {
        GoldAdTimer,
    }

    enum GemTMP
    {
        GemAdTimer
    }

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

    TextMeshProUGUI _timerTMP;

    Coroutine co_updateUI;

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
                Bind<TextMeshProUGUI>(typeof(GemTMP));
                break;
            case ADType.Gold:
                Bind<TextMeshProUGUI>(typeof(GoldTMP));
                break;
            case ADType.Chest:
                Bind<TextMeshProUGUI>(typeof(ChestTMP));
                break;
        }

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        

        switch (_type)
        {
            case ADType.Gem:
                rewardId = "GM0003";
                _timerTMP = Get<TextMeshProUGUI>((int)GemTMP.GemAdTimer);
                break;
            case ADType.Gold:
                rewardId = "GD0002";
                _timerTMP = Get<TextMeshProUGUI>((int)GoldTMP.GoldAdTimer);
                break;
            case ADType.Chest:
                rewardId = "CH0004";
                _timerTMP = Get<TextMeshProUGUI>((int)ChestTMP.ChestAdTimer);
                break;
        }


        int index = (int)Managers.Data.ItemDic[rewardId].Grade;
        GetImage((int)Images.Ad_Item_Icon).sprite = Managers.Data.ItemDic[rewardId].itemIcon;
        GetImage((int)Images.Ad_Item_Icon_Frame).sprite = Managers.Data.Frames[index];

        Action a = () => {
            Managers.Game.Additem(rewardId);
            var popup = Managers.UI.ShowPopupUI<UI_Reward_Popup>();
            //popup.Init();
            popup.UpdateUI(rewardId);
        };

        
        GetButton((int)Buttons.Ad_Front_Button).gameObject.BindEvent((PointerEventData data) => {
            
            Managers.AD.GetMaketAdResetReward(a);
            //a.Invoke();
            Managers.Game.SaveGame("AdButton");

        });


        /*
        GetButton((int)Buttons.Ad_Front_Button).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Ad_Front_Button).onClick.AddListener(() =>
        {

            _adPopupController.GetReward((int)_type);
            a.Invoke();
            Managers.Game.SaveGame("AdButton");
        });

        GetButton((int)Buttons.Ad_Front_Button).interactable = false;
        */


        co_updateUI = StartCoroutine(c_UpdateUI());

    }

    private void OnEnable()
    {
        if(_timerTMP != null)
        {
            if(co_updateUI != null)
            {
                StopCoroutine(co_updateUI);
                co_updateUI = StartCoroutine(c_UpdateUI());
            }

        }
    }


    IEnumerator c_UpdateUI()
    {
        while (true)
        {
            _timerTMP.text = Managers.AD.GetRemainingAdTime();
            yield return new WaitForSeconds(0.5f);
        }
    }

 
}