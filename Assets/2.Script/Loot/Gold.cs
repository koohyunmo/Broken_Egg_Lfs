using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gold : RiseItem
{
    [SerializeField] Transform Target1;

    public override void OnEnable()
    {
        base.OnEnable();
        Target = Managers.Loot.GoldIcon;
        targetVector = Target.position;
        GetGoldOrExp();
    }



    public override void OnDisable()
    {
        base.OnDisable();
        transform.DOMove(new Vector3(0, 0, targetVector.z), 0);
    }

    protected override void InitData()
    {

        Target = Managers.Loot.GoldIcon;
        targetVector = Target.position;

    }
}
