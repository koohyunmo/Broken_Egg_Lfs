using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Key : DropItem
{
    public string _id { get; set; } = "KEY0001";


    public override void OnClickEvent(PointerEventData data)
    {
        base.OnClickEvent(data);
        //randomBox.ResultSelect();


        //TODO 인벤토리에 추가
        Managers.Game.Additem(_id);
        Managers.Loot.PopItem();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopCoroutine(c_DropItemAnim());
    }

    private void Start()
    {
        base.Init();
        InitData();
    }

    public override void OnEnable()
    {
        StartCoroutine(c_DropItemAnim());
    }

    protected override void InitData()
    {
        _itemId = "KEY0001";
        Target = Managers.Loot.InventoryButton;
        targetVector = Target.position;
    }
}
