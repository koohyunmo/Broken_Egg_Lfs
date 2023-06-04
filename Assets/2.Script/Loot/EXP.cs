using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EXP : RiseItem
{

    public override void OnEnable()
    {
        Target = Managers.Loot.LevelIcon;
        targetVector = Target.position;
        GetGoldOrExp();
    }



    public override void OnDisable()
    {
        transform.DOMove(new Vector3(0, 0, targetVector.z), 0);
    }

    protected override void InitData()
    {
        Target = Managers.Loot.LevelIcon;
        targetVector = Target.position;
    }
}
