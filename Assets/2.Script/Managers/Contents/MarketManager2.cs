using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarketManager2 
{

    /// <summary>
    /// 생성용 읽기 ㄴㄴ
    /// </summary>
    private Dictionary<string, int> _currentItemList = new Dictionary<string, int>();
    /// <summary>
    /// 생성용 읽기 ㄴㄴ
    /// </summary>
    private Dictionary<string, int> _nextItemList = new Dictionary<string, int>();
    /// <summary>
    /// 생성용 읽기 ㄴㄴ
    /// </summary>
    private Dictionary<string, int> _addItemList = new Dictionary<string, int>();
    private Dictionary<string, int> _gemItemList = new Dictionary<string, int>();






    public void Init()
    {
        if((Managers.Game.MarketData.autoResetTime == DateTime.MinValue || Managers.Game.MarketData.autoResetTime == null) || (Managers.Game.MarketData.adResetTime == DateTime.MinValue || Managers.Game.MarketData.adResetTime == null))
        { 
            //TODO SetData

            MakeMarketItemDic();
            FirstTimeSetting();

            Managers.Game.SetCurrentAndNextMarketList(_currentItemList, _nextItemList, _addItemList,_gemItemList);    
        }
        else
        {
            _currentItemList = Managers.Game.MarketData.currentItem;
            _nextItemList = Managers.Game.MarketData.nextItem;
            _addItemList = Managers.Game.MarketData.addItem;
            _gemItemList = Managers.Game.MarketData.gemItem;

        }

    }


    private void MakeMarketItemDic()
    {
       
        SetMarketData(ref _currentItemList);
        SetMarketData(ref _nextItemList);
        SetMarketData(ref _addItemList,2);
        SetMarketData(ref _gemItemList, 3);

    }

    public void ClickFreeReset()
    {
        DateTime dt = Managers.Game.MarketData.freeResetTime;

        if (MarketTimeCompare(dt) == true)
        {
            SetFreeResetTime();
            SwapAndMakeDic();
        }
        else
            return;
    }

    public string MarketTimeRemainStr(int type = 1)
    {
        DateTime dt;
        if (type == 1)
            dt = Managers.Game.MarketData.freeResetTime;
        else if (type == 2)
            dt = Managers.Game.MarketData.autoResetTime;
        else if (type == 3)
            dt = Managers.Game.MarketData.adResetTime;
        else
            return "-1";
        


        DateTime resetTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        if (MarketTimeCompare(dt) == true)
            return "00:00:00";

        string s = (resetTime - now).ToString();

        return s;
    }

    public void AutoReset()
    {
        DateTime dt = Managers.Game.MarketData.autoResetTime;

        if (MarketTimeCompare(dt))
        {
            SetAutoResetTime();
            SwapAndMakeDic();
        }
        else
            return;
    }

    public void AutoGemReset()
    {
        DateTime dt = Managers.Game.MarketData.adResetTime;

        if (MarketTimeCompare(dt))
        {
            SetAdReset();
            SetMarketData(ref _gemItemList, 3);
        }
        else
            return;
    }
    public void ClickAdList()
    {
        Managers.Game.SetCurrentMarketList(_addItemList);
        _currentItemList = _addItemList;
        SetMarketData(ref _addItemList, 2);
    }
    public void ClickGemAdList()
    {
        SetMarketData(ref _gemItemList, 3);
        Managers.Game.SetCurrentGemMarketList(_gemItemList);
    }

    public void ADReset()
    {
        Managers.Game.SetCurrentMarketList(_addItemList);
        _currentItemList = _addItemList;
        SetMarketData(ref _addItemList, 2);
    }

    private void FirstTimeSetting()
    {
        DateTime autoReset = DateTime.Now.AddHours(1);
        DateTime freeReset = DateTime.Now.AddMinutes(10);
        DateTime adResetTime = DateTime.Now.AddMinutes(30);

        Managers.Game.SetMarektFirstTime(autoReset, freeReset,adResetTime);
    }

    private void SetAutoResetTime()
    {
        DateTime autoReset = DateTime.Now.AddHours(1);
        Managers.Game.SetAutoResetTime(autoReset);
    }

    private void SetFreeResetTime()
    {
        DateTime freeReset = DateTime.Now.AddMinutes(10);
        Managers.Game.SetFreeResetTime(freeReset);
    }
    private void SetAdReset()
    {
        DateTime adReset = DateTime.Now.AddMinutes(30);
        Managers.Game.SetAdResetTime(adReset);
    }
    private void SwapAndMakeDic()
    {
        Managers.Game.SetCurrentMarketList(_nextItemList);
        _currentItemList = _nextItemList;

        SetMarketData(ref _nextItemList);
        Managers.Game.SetNextMarketList(_nextItemList);
    }


    public void BuyItem(string id)
    {


        if (Managers.Game.MarketData.currentItem.TryGetValue(id, out int count))
        {
            if (count <= 0)
                return;
            else
                count = 5 - count;

        }
        else
            return;


        if (Managers.Data.ItemDic.TryGetValue(id, out ItemScriptbale itemData))
        {

        }
        else
            return;



        int price = (int)(itemData.itemCost * (count * 0.2) + itemData.itemCost);

        if (Managers.Game.Gold >= price)
        {
            Managers.Game.MinusGold(price);
            Managers.Game.Additem(id);
            Managers.Game.MinusMarketItemCount(id);
        }
        else
            return;

    }

    public string GetPriceString(string id)
    {

        if (Managers.Game.MarketData.currentItem.TryGetValue(id, out int count))
        {
            if (count <= 0)
                return "Sold Out";
            else
                count = 5 - count;
        }
        else
            return "-1";

        if (Managers.Data.ItemDic.TryGetValue(id, out ItemScriptbale itemData))
        {

        }
        else
            return "-1";




        int price = (int)(itemData.itemCost * (count * 0.2) + itemData.itemCost);

        return CUtil.LongFormatNumber(price);

    }

    public int GetPriceInt(string id)
    {

        if (Managers.Data.ItemDic.TryGetValue(id, out ItemScriptbale itemData))
        {

        }
        else
            return -1;


        if (Managers.Game.MarketData.currentItem.TryGetValue(id, out int count))
        {
            count = 5 - count;
        }
        else
            return -1;

        int price = (int)(itemData.itemCost * (count * 0.2) + itemData.itemCost);


        return price;

    }

    public int GetCount(string id)
    {


        if (Managers.Game.MarketData.currentItem.TryGetValue(id, out int count))
        {
            if (count <= 0)
                return 0;
        }
        else
            return 999;

        return count;

    }

    public int GetGemCount(string id)
    {


        if (Managers.Game.MarketData.gemItem.TryGetValue(id, out int count))
        {
            if (count <= 0)
                return 0;
        }
        else
            return 999;

        return count;

    }


    public int GetGemPrice(string id)
    {

        if (Managers.Data.ItemDic.TryGetValue(id, out ItemScriptbale iss))
        {
            switch (iss.Grade)
            {
                case Define.Grade.None:
                    return 3;
                    break;
                case Define.Grade.Common:
                    return 3;
                    break;
                case Define.Grade.Rare:
                    return 5;
                    break;
                case Define.Grade.Unique:
                    return 10;
                    break;
                case Define.Grade.Legend:
                    return 20;
                    break;
                case Define.Grade.Hero:
                    return 30;
                    break;
            }
        }

        return -1;
    }

    public void BuyGemItem(string id)
    {


        if (Managers.Game.MarketData.gemItem.TryGetValue(id, out int count))
        {
            if (count <= 0)
                return;
            else
                count = 5 - count;

        }
        else
            return;


        if (Managers.Data.ItemDic.TryGetValue(id, out ItemScriptbale itemData))
        {

        }
        else
            return;



        int price = GetGemPrice(id);

        if (Managers.Game.Gem >= price)
        {
            Managers.Game.MinusGem(price);
            Managers.Game.Additem(id);
            Managers.Game.MinusMarketGemItemCount(id);
        }
        else
            return;

    }

    public void BuyChest(string id)
    {
        int price = Managers.Data.ItemDic[id].gem;

        if (Managers.Game.Gem >= price)
        {
            Managers.Game.MinusGem(price);
            Managers.Game.Additem(id);
        }
        else
            return;

    }


    public bool MarketTimeCompare(DateTime dt)
    {

        DateTime resetTime = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        bool b = resetTime < now;

        return b;
    }

    #region ItemList

    List<Define.Grade> grades = new List<Define.Grade>();
    List<Define.Grade> chooseGrades = new List<Define.Grade>();
    List<string> resultList = new List<string>();
    public void SetMarketData(ref Dictionary<string, int> dic, int type = 1)
    {
        dic.Clear();
        grades.Clear();
        chooseGrades.Clear();
        resultList.Clear();

        MarketListPercent(ref grades,type);

        for (int i = 0; i < 8; i++)
        {
            int rand = UnityEngine.Random.Range(0, grades.Count - 1);
            chooseGrades.Add(grades[rand]);
        }

        for (int i = 0; i < chooseGrades.Count; i++)
        {
            switch (chooseGrades[i])
            {
                case Define.Grade.None:
                    break;
                case Define.Grade.Common:
                    dic.Add(ItemGetID(Managers.Data.CommonLIST,dic),5);
                    break;
                case Define.Grade.Rare:
                    dic.Add(ItemGetID(Managers.Data.RareLIST,dic, "Rare"),4);
                    break;
                case Define.Grade.Unique:
                    dic.Add(ItemGetID(Managers.Data.UniqueLIST,dic, "Unique"),3);
                    break;
                case Define.Grade.Legend:
                    dic.Add(ItemGetID(Managers.Data.LegendLIST,dic, "Legend"),2);
                    break;
                case Define.Grade.Hero:
                    dic.Add(ItemGetID(Managers.Data.HerorLIST,dic, "Hero"),1);
                    break;
            }
        }

        /*
        // LINQ를 사용하여 Dictionary 내림차순 정렬
        var sortedDict = from entry in dic orderby (int)Managers.Data.ItemSO[entry.Key].Grade descending select entry;

        // 결과 출력
        foreach (KeyValuePair<string, int> item in sortedDict)
        {
            Debug.Log($"{item.Key}: {item.Value}");
        }
        */

        var sortedDict = dic.OrderBy(entry => entry.Value);
        dic = sortedDict.ToDictionary(entry => entry.Key, entry => entry.Value);

    }


    string ItemGetID(List<string> list,Dictionary<string,int> dic, string check = "Common")
    {
        int index = UnityEngine.Random.Range(0, list.Count - 1);
        index = Mathf.Clamp(index, 0, list.Count - 1);
        string s = list[index];

        Managers.Data.ItemDic.TryGetValue(s, out ItemScriptbale iso);


        while (dic.ContainsKey(s) || iso.itemType == Define.ItemType.GOLD || iso.itemType == Define.ItemType.GEM || iso.itemType == Define.ItemType.Chest)
        {
           
            if (dic.ContainsKey(s) == false && iso.itemType != Define.ItemType.GOLD && iso.itemType != Define.ItemType.GEM && iso.itemType != Define.ItemType.Chest)
                break;
            else
            {
                index = UnityEngine.Random.Range(0, list.Count);
                index = Mathf.Clamp(index, 0, list.Count - 1);
                s = list[index];
            }

            Managers.Data.ItemDic.TryGetValue(s, out iso);

        }

        return s;

    }

    private void MarketListPercent(ref List<Define.Grade> gr,int type = 1)
    {
        gr.Clear();

        if(type == 1)
        {
            for (int i = 0; i < 65; i++)
            {
                gr.Add(Define.Grade.Common);
            }
            for (int i = 0; i < 30; i++)
            {
                gr.Add(Define.Grade.Rare);
            }
            for (int i = 0; i < 5; i++)
            {
                gr.Add(Define.Grade.Unique);
            }
            for (int i = 0; i < 0; i++)
            {
                gr.Add(Define.Grade.Legend);
            }
            for (int i = 0; i < 0; i++)
            {
                gr.Add(Define.Grade.Hero);
            }
        }
        else if(type == 2)
        {
            for (int i = 0; i < 50; i++)
            {
                gr.Add(Define.Grade.Common);
            }
            for (int i = 0; i < 30; i++)
            {
                gr.Add(Define.Grade.Rare);
            }
            for (int i = 0; i < 16; i++)
            {
                gr.Add(Define.Grade.Unique);
            }
            for (int i = 0; i < 3; i++)
            {
                gr.Add(Define.Grade.Legend);
            }
            for (int i = 0; i < 1; i++)
            {
                gr.Add(Define.Grade.Hero);
            }
        }

        else if (type == 3)
        {
            for (int i = 0; i < 0; i++)
            {
                gr.Add(Define.Grade.Common);
            }
            for (int i = 0; i < 50; i++)
            {
                gr.Add(Define.Grade.Rare);
            }
            for (int i = 0; i < 35; i++)
            {
                gr.Add(Define.Grade.Unique);
            }
            for (int i = 0; i < 13; i++)
            {
                gr.Add(Define.Grade.Legend);
            }
            for (int i = 0; i < 2; i++)
            {
                gr.Add(Define.Grade.Hero);
            }
        }


    }
    #endregion
}
