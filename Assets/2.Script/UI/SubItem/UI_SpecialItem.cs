using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SpecialItem : UI_Base
{
    enum Images
    {
        Frame,
        Icon
    }
    enum Buttons
    {
        BuyButton
    }

    enum TMPS
    {
        PriceTMP,
        PercentTMP
    }

    string _id;
    Sprite _icon;
    Sprite _frame;
    int _gem;
    List<int> percent;

    public void InitData(ItemScriptbale itemData)
    {
        _id = itemData.itemID;
        _icon = itemData.itemIcon;
        _frame = Managers.Data.Frames[(int)itemData.Grade];
        _gem = itemData.gem;
        percent = itemData.chestPercent;

        Init();
    }

    public override void Init()
    {
        transform.GetComponent<Transform>().localPosition = Vector3.zero;
        transform.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TMPS));


        GetImage((int)Images.Icon).sprite = _icon;
        GetImage((int)Images.Frame).sprite = _frame;

        Get<TextMeshProUGUI>((int)TMPS.PriceTMP).text = _gem.ToString();
        Get<TextMeshProUGUI>((int)TMPS.PercentTMP).text = 
            $"<color=green> Common : {percent[0]}%</color>\n\n" +
            $"<color=blue> Rare : {percent[1]}%</color>\n\n" +
            $"<color=purple> Unique : {percent[2]}%</color>\n\n" +
            $"<color=orange> Legendary : {percent[3]}%</color>\n\n" +
            $"<color=red> Hero : {percent[4]}%</color>\n\n";

        GetButton((int)Buttons.BuyButton).gameObject.BindEvent((PointerEventData data) => { Managers.Market.BuyChest(_id); });

    }
}
