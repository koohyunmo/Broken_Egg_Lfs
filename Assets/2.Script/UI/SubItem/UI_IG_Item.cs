using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_IG_Item : UI_Base
{
    enum Images
    {
        IGIcon,
    }
    enum Texts
    {
        IGCountText,
    }

    [SerializeField] Sprite _icon;
    [SerializeField] string _igId;
    [SerializeField] string _itemID;
    [SerializeField] int _count;
    [SerializeField] int _itemCount;
    [SerializeField] Text _countText;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        BindImage(typeof(Images));
        BindText(typeof(Texts));


        GetImage((int)Images.IGIcon).GetComponent<Image>().sprite = _icon;

        gameObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<RectTransform>().transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0);


        SetText();
    }


    public void InitData(string igID, int count)
    {
        _igId = igID;
        _count = count;

        if(Managers.Data.ItemDic.TryGetValue(_igId, out ItemScriptbale item))
        {
            _icon = item.itemIcon;
        }



    }

    private void Update()
    {
        UpdateIgData();
    }

    void UpdateIgData()
    {
       if (Managers.Game.InventoryData.item.TryGetValue(_igId, out ItemData itemData))
        {
            _itemCount = itemData.itemCount;
            SetText();
        }
        else
        {
            _itemCount = 0;
            SetText();
        }

    }

    void SetText()
    {
        _countText = GetText((int)Texts.IGCountText);


        if (Managers.Game.InventoryData.item.TryGetValue(_igId, out ItemData itemData))
        {
            _countText.text = $"{itemData.itemCount}/{_count}";
        }
        else
        {
            _countText.text = $"{0}/{_count}";
        }
    }

}
