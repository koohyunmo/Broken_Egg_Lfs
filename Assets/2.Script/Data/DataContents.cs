using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


#region Stat
[Serializable]
public class StartStat
{
    public PlayerData playerData = new PlayerData();
    public InventoryData inventoryItem = new InventoryData();
    public StageData stageData = new StageData();
    public UpgadeData upgadeData = new UpgadeData();
    public QuestData questData = new QuestData();
    public MarketData marketData = new MarketData();
}
[Serializable]
public class PlayerData
{
    public int level;
    public float playerDamage;
    public long gold;
    public long gem;
    public long totalExp;
    public long currentExp;
    public long requireExp;
    public int clickPerGold;
    public int clickPerExp;
    public float criticalPercent;
    public float criticalPlusDamage;
    public float itemDropRate;
    public float goldBonus;
    public float xpBonus;
    public int clickCount;
    public long usingGold;
    public int reinforceCount;
    public long usingGem;
}

[Serializable]
public class UpgadeData
{
    public int damageUpLevel;
    public int itemDropRateLevel;
    public int goldBonusLevel;
    public int xpBonusLevel;

}
[SerializeField]
public class QuestData
{
    public int clickCountQuestLevel;
    public int usingGoldQuestLevel;
    public int reinforceCountGoldQuestLevel;
    public int usingGemQuestLevel;
    public string clickRewardId;
    public string usingGoldRewardId;
    public string reinforceRewardId;
    public string usingGemRewardId;
}
[Serializable]
public class StageData
{
    public long maxHp;
    public long currentHp;
    public string eggID;
    public int clearStage;
    public int currentStage;
    public int killCount;
}
[Serializable]
public class InventoryData
{
    //public Dictionary<string, int> _hasitemCount = new Dictionary<string, int>();
    public List<string> hasItemList = new List<string>();
    public Dictionary<string, ItemData> item = new Dictionary<string, ItemData>();
    public string equip;
}

[Serializable]
public class ItemData
{
    public string itemId;
    public int itemCount;
    public int reinforce;
    public int killedEgg;
    public long itemDamage;
    public float itemCriticalPercent;
    public float itemCriticalPlusDamage;
    public Define.ItemType itemType;
    public Define.Grade itemGrade;
    public string itemName;
    public int shieldAttack;

    public ItemData(string itemId , int itemCount = 1, int reinforce = 0, int killedEgg = 0, long itemdamage = 10, float itemCriticalPercent = 1, float itemCriticalPlusDamage = 2.5f, Define.ItemType itemType = Define.ItemType.Equip, Define.Grade itemGrade = Define.Grade.Common, string itemName = "Hit1")
    {
        this.itemId = itemId;
        this.itemCount = itemCount;
        this.reinforce = reinforce;
        this.killedEgg = killedEgg;
        this.itemDamage = itemdamage;
        this.itemCriticalPercent = itemCriticalPercent;
        this.itemCriticalPlusDamage = itemCriticalPlusDamage;
        this.itemType = itemType;
        this.itemGrade = itemGrade;
        this.itemName = itemName;
    }
}

[Serializable]
public class MarketData
{
    public Dictionary<string, int> currentItem = new Dictionary<string, int>();
    public Dictionary<string, int> nextItem = new Dictionary<string, int>();
    public Dictionary<string, int> addItem = new Dictionary<string, int>();
    public Dictionary<string, int> gemItem = new Dictionary<string, int>();
    public DateTime freeResetTime;
    public DateTime autoResetTime;
    public DateTime adResetTime;
}


[Serializable]
public class EquipmentData
{
    //public Dictionary<string, bool> _equip= new Dictionary<string, bool>();
}

#endregion


// 초기스탯 읽어오기
[Serializable]
public class StartStatData : ILoader<int, StartStat>
{
    public List<StartStat> startStats = new List<StartStat>();

    public Dictionary<int, StartStat> MakeDict()
    {
        Dictionary<int, StartStat> dict = new Dictionary<int, StartStat>();

        foreach (StartStat stat in startStats)
            dict.Add(stat.playerData.level, stat);
            

        return dict;
    }
}



