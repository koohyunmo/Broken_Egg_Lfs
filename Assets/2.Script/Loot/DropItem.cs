using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class DropItem : LootItems
{

    [Header("DropItem Class")]
    protected string itemId;
    protected Vector3[] wayPoints;
    protected BoxCollider2D boxCollider2D;
    bool _isGet;

    protected abstract void InitData();


    public virtual void OnEnable()
    {

    }

    public virtual void OnDisable()
    {

    }

    #region Animation

    /// <summary>
    /// 아이템 떨어지는 코드
    /// </summary>
    /// <returns></returns>
    public IEnumerator c_DropItemAnim()
    {
        transform.DOLocalMove(new Vector3(0, 0, 0), 0);
        
        int i = Random.Range(-185, 185);

        int j = 1;

        if (i > 0)
            j = 1;
        else
            j = -1;

        if (boxCollider2D == null)
            boxCollider2D = gameObject.GetComponent<BoxCollider2D>();

        boxCollider2D.enabled = false;
        transform.DOLocalMove(new Vector3(i, Random.Range(600, 700), 0), 1f);
        yield return new WaitForSeconds(1f);
        transform.DOLocalMove(new Vector3(i + (Random.Range(0, 400) * j), -500, 0), 0.5f);
        yield return new WaitForSeconds(0.5f);
        boxCollider2D.enabled = true;

        yield return new WaitForSeconds(3f);

        while (true)
        {
            if (_isGet == true)
                break;
            transform.DOPunchPosition(new Vector3(1, 30, 1), 1, 3, 1);
            yield return new WaitForSeconds(2f);
        }
    }
    /// <summary>
    /// 누르면 인벤토리 들어가는 코드
    /// </summary>
    /// <returns></returns>
    IEnumerator c_GetItemAnim()
    {
        wayPoints = new Vector3[2];


        //Vector3 midle = (targetVector - transform.position) / 2;


        wayPoints.SetValue(new Vector3(transform.position.x, transform.position.y + 2f, targetVector.z), 0);
        //wayPoints.SetValue(new Vector3(midle.x, midle.y, targetVector.z), 1);
        wayPoints.SetValue(new Vector3(targetVector.x, targetVector.y, targetVector.z), 1);

        yield return new WaitForSeconds(0.02f);
        transform.DOPath(wayPoints, 2f, PathType.Linear, PathMode.Sidescroller2D).SetLoops(1);

        boxCollider2D.enabled = false;

        StartCoroutine(c_Destroy());
    }


    #endregion

    /// <summary>
    /// 아이템줍기 실행코드
    /// </summary>
    public void GetItem()
    {
        _isGet = true;
        StopCoroutine(c_DropItemAnim());

        if (boxCollider2D == null)
            boxCollider2D = gameObject.GetComponent<BoxCollider2D>();

        StartCoroutine(c_GetItemAnim());

    }

    IEnumerator c_Destroy()
    {

        Managers.Game.Additem(_itemId);
        Managers.Loot.PopItem();
        yield return new WaitForSeconds(2f);
        _isGet = false;
        boxCollider2D.enabled = true;
        Destroy();
    }

}




