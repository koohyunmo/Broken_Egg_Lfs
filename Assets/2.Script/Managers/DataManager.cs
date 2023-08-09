using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;

// 원본은 xml 관리? 게임에서는 json 사용?
// xml -> json 
// 데이터를 로딩하는 인터페이스

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}



public class DataManager
{
    public Dictionary<int, StartStat> _startStatDic { get; private set; } = new Dictionary<int, StartStat>();
    public Dictionary<string, ItemScriptbale> ItemDic { get; private set; } = new Dictionary<string, ItemScriptbale>();
    public Dictionary<int, EggScriptableObject> EggDic { get; private set; } = new Dictionary<int, EggScriptableObject>();
    //public List<string> itemSOKey { get; private set; } = new List<string>();
    public List<string> CommonLIST { get; private set; } = new List<string>();
    public List<string> RareLIST { get; private set; } = new List<string>();
    public List<string> UniqueLIST { get; private set; } = new List<string>();
    public List<string> LegendLIST { get; private set; } = new List<string>();
    public List<string> HerorLIST { get; private set; } = new List<string>();
    public List<string> GoldLIST { get; private set; } = new List<string>();
    public List<string> GemLIST { get; private set; } = new List<string>();
    public List<string> ChestList { get; private set; } = new List<string>();
    
    public List<string> WeaponQuestList { get; private set; } = new List<string>();


    /// <summary>
    /// 0 NONE 1 COMMON 2 RARE 3 EPIC 4 LEGENDARY 5 HERO
    /// </summary>
    public List<Sprite> Frames { get; private set; } = new List<Sprite>();

    public int EggImageIndex { get; private set; }

    public bool INIT { get; private set; } = false;

    public async void Init()
    {
        //Start Stat
        _startStatDic = LoadJson<StartStatData, int, StartStat>("StartData").MakeDict();

        Debug.Log("로딩");

        //Scriptable Object
        ItemScriptbale[] itemSO = Resources.LoadAll<ItemScriptbale>("Prefabs/SO/ItemSO");
        EggScriptableObject[] eggSO = Resources.LoadAll<EggScriptableObject>("Prefabs/SO/EggSO/");


        Sprite[] frames = Resources.LoadAll<Sprite>("Images/Frames/");
        Frames.AddRange(frames);

        Debug.Log("egg Data: " + eggSO.Length);
        Debug.Log("Item Data : " + itemSO.Length);

        ItemDic.Clear();
        //itemSOKey.Clear();
        EggDic.Clear();

       await InitAsync(itemSO, eggSO);

    }

    async Task InitAsync(ItemScriptbale[] itemSO, EggScriptableObject[] eggSO)
    {
        List<string> WeaponList = new List<string>();

        for (int i = 0; i < itemSO.Length; i++)
        {
            this.ItemDic.Add(itemSO[i].itemID, itemSO[i]);
            //itemSOKey.Add(itemSO[i].itemID);

            if (itemSO[i].itemType != Define.ItemType.Chest)
            {
                switch (itemSO[i].Grade)
                {
                    case Define.Grade.None:
                        CommonLIST.Add(itemSO[i].itemID);
                        break;
                    case Define.Grade.Common:
                        CommonLIST.Add(itemSO[i].itemID);
                        break;
                    case Define.Grade.Rare:
                        RareLIST.Add(itemSO[i].itemID);
                        break;
                    case Define.Grade.Unique:
                        UniqueLIST.Add(itemSO[i].itemID);
                        break;
                    case Define.Grade.Legend:
                        LegendLIST.Add(itemSO[i].itemID);
                        break;
                    case Define.Grade.Hero:
                        HerorLIST.Add(itemSO[i].itemID);
                        break;
                }
            }
            switch (itemSO[i].itemType)
            {
                case Define.ItemType.Chest:
                    ChestList.Add(itemSO[i].itemID);
                    break;
                case Define.ItemType.GOLD:
                    GoldLIST.Add(itemSO[i].itemID);
                    break;
                case Define.ItemType.GEM:
                    GemLIST.Add(itemSO[i].itemID);
                    break;
                case Define.ItemType.Equip:
                    WeaponList.Add(itemSO[i].itemID);
                    break;
            }
        }

        WeaponQuestList.Add(WeaponList[10]);
        WeaponQuestList.Add(WeaponList[20]);
        WeaponQuestList.Add(WeaponList[30]);
        WeaponQuestList.Add(WeaponList[40]);
        WeaponQuestList.Add(WeaponList[44]);




        WeaponList.Clear();

        for (int i = 0; i < eggSO.Length; i++)
        {
            EggDic.Add(eggSO[i].level, eggSO[i]);
        }

        Debug.Log("로딩끝");

        INIT = true;

    }
    public void StartStatLoadTest()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/StartData");
        //StartStatData data = JsonUtility.FromJson<StartStatData>(textAsset.text);
        StartStatData data = JsonConvert.DeserializeObject<StartStatData>(textAsset.text);
        Debug.Log(data.ToString());


        foreach (StartStat stat in data.startStats)
        {
            _startStatDic.Add(stat.playerData.level, stat);
        }

    }

    Loader LoadJson<Loader, Key, value>(string path) where Loader : ILoader<Key, value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }

    public void EggImageIndexSetter(int index)
    {
        EggImageIndex = index;
    }




}
