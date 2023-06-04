using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }

    public void PopupSetSize()
    {
        gameObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<RectTransform>().transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
