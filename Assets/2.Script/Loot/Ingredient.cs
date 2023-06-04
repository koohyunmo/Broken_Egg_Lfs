using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ingredient : DropItem
{
    public string _id { get; set; } = "IG0001";
    public string _name { get; set; } = "IG0001";

    public Vector2 _start;




    public override void OnClickEvent(PointerEventData data)
    {
        base.OnClickEvent(data);
        Managers.Game.Additem(_id);
        Managers.Loot.PopItem();
    }

    public void InitData(string id)
    {
        _id = id;
        _name = _id;
    }


    public override void OnDisable()
    {
    }

    public override void OnEnable()
    {
        base.Init();
        //StartCoroutine(c_ItemDrop());
    }

    protected override void InitData()
    {
        //
    }
}
