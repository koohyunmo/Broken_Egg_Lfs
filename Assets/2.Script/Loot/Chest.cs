using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Chest : DropItem
{
    //[SerializeField] RandomSelect randomBox;
    public string _id { get; set; } = "LB0001";

    public override void OnClickEvent(PointerEventData data)
    {
        base.OnClickEvent(data);
        //randomBox.ResultSelect();


        //TODO 인벤토리에 추가
        Managers.Game.Additem(_itemId);
        Managers.Loot.PopItem();
    }

    public void OnClickSpriteEvent()
    {
        GetItem();
        //TODO 인벤토리에 추가
        Managers.Game.Additem(_itemId);
        Managers.Loot.PopItem();
    }




    public override void OnDisable()
    {
        StopCoroutine(c_DropItemAnim());

    }

    public override void OnEnable()
    {
        StartCoroutine(c_DropItemAnim());
    }

    private void Start()
    {
        base.Init();
        InitData();
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();

    }

    protected override void InitData()
    {
        _itemId = "LB0001";
        Target = Managers.Loot.InventoryButton;
        targetVector = Target.position;
    }
}
