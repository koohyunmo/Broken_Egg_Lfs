using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_InformationText : UI_Base
{
    enum TMPS
    {
        UI_InformationText,
    }

    private void Start()
    {
        Init();
    }
    string _id;

    public void InitData(string id)
    {
        _id = id;
    }

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(TMPS));
        Get< TextMeshProUGUI>((int)TMPS.UI_InformationText).text = _id;

        gameObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<RectTransform>().transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0);
        gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(1, 1, 1);

    }
}
