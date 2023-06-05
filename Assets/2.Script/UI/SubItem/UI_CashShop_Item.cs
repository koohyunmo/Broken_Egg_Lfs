using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CashShop_Item : UI_Base
{
    public string _id;
    public IAPController _iap;

    enum Buttons
    {
        CashShop__Buy_Button
    }

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        BindButton(typeof(Buttons));

        transform.parent.TryGetComponent<IAPController>(out _iap);

        if (string.IsNullOrEmpty(_id))
            _id = "test_id";


        if (_iap == null)
            Debug.LogWarning("iap null");

        Action action = null;

        switch (_id)
        {
            case "eggpack3": action = () => { Managers.Game.AddGem(3000); }; break;
            case "eggpack2": action = () => { Managers.Game.AddGem(1000); }; break;
            case "eggpack1": action = () => { Managers.Game.AddGem(300); }; break;
        }

       

        //GetButton((int)Buttons.CashShop__Buy_Button).gameObject.BindEvent((PointerEventData) => { _iap.Purchase(_id, action); });
        GetButton((int)Buttons.CashShop__Buy_Button).gameObject.BindEvent((PointerEventData) => { _iap.Purchasing(_id,action); });
    }
}
