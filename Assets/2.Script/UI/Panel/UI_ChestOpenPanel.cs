using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ChestOpenPanel : UI_Base
{
    enum Buttons
    {
        GotChaOpenButton,
        CloseGotChaButton,
        ReOpenButton,
        CloseGotChaButton2,

    }

    enum Panels
    {
        ReOpenPanel,
    }

    UI_Popup _parent;
    GotchaTest _gatcha;


    public override void Init()
    {

        _parent = transform.parent.GetComponent<UI_Popup>();
        _gatcha = gameObject.GetComponent<GotchaTest>();

        BindButton(typeof(Buttons));
        BindObject(typeof(Panels));


        GetObject((int)Panels.ReOpenPanel).gameObject.SetActive(false);

        GetButton((int)Buttons.CloseGotChaButton).gameObject.BindEvent((PointerEventData) => {

            Managers.UI.ClosePopupUI(_parent);
        
        });

        GetButton((int)Buttons.CloseGotChaButton2).gameObject.BindEvent((PointerEventData) => {

            Managers.UI.ClosePopupUI(_parent);

        });


        GetButton((int)Buttons.GotChaOpenButton).gameObject.BindEvent((PointerEventData) =>
        {

            _gatcha.CardOpen();
            GetObject((int)Panels.ReOpenPanel).gameObject.SetActive(true);
            GetButton((int)Buttons.GotChaOpenButton).gameObject.SetActive(false);
            
        });



    }

    private void Start()
    {
        Init();
    }

}
