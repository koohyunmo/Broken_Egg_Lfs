using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RiseItem : LootItems
{
    protected string itemId;
    protected Vector3[] wayPoints;

    public virtual void OnEnable()
    {
        _reward = Managers.Stage.StageReward();
    }
    public virtual void OnDisable()
    {
        _reward = 0;
    }
    protected abstract void InitData();


    IEnumerator c_GetItemAnim()
    {
        transform.DOMove(new Vector3(0, 0, targetVector.z), 0);
        if(targetVector == new Vector3(0,0,0))
            Debug.Log("Target Error");

        wayPoints = new Vector3[3];
        Vector3 middleVector = (targetVector - transform.position) / 2;

        wayPoints.SetValue(new Vector3(transform.position.x, transform.position.y + 2f, targetVector.z), 0);
        wayPoints.SetValue(new Vector3(middleVector.x, middleVector.y, targetVector.z) + new Vector3(-1,1,0).normalized, 1);
        wayPoints.SetValue(new Vector3(targetVector.x, targetVector.y, targetVector.z), 2);

        yield return new WaitForSeconds(0.02f);
        transform.DOPath(wayPoints, 2f, PathType.Linear, PathMode.Sidescroller2D).SetLoops(1);

        StartCoroutine(c_Destroy());
    }

    IEnumerator c_Destroy()
    {
        yield return new WaitForSeconds(2f);

        switch (gameObject.name)
        {
            case "Gold":
                Managers.Game.AddGold(_reward);
                break;
            case "EXP":
                Managers.Game.AddExp(_reward);
                break;

            default:
                break;
        }

        Managers.Game.AllDataRefresh();
        Destroy();
    }

    public void GetGoldOrExp()
    {
        StartCoroutine(c_GetItemAnim());
    }

     protected virtual void Start()
    {
        base.Init();
        InitData(); 
    }

}
