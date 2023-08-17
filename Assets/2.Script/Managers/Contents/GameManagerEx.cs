using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Threading.Tasks;

[Serializable]
public class GameData
{
    public PlayerData playerData = new PlayerData();
    public InventoryData inventoryItem = new InventoryData();
    public StageData stageData = new StageData();
    public EquipmentData equipmentData = new EquipmentData();
    public UpgadeData upgadeData = new UpgadeData();
    public QuestData questData = new QuestData();
    public MarketData marketData = new MarketData();

}

public class GameManagerEx
{
    public string UseEquipment { get { return _gameData.inventoryItem.equip; } private set { } } // 현재 사용중인 이펙트 ID

    GameData _gameData = new GameData(); // 플레이어 데이터
    public GameData SaveData { get { return _gameData; } private set { _gameData = value; } }
    public long Gold
    {
        get
        {
            if (_gameData.playerData.gold < 0)
            {
                _gameData.playerData.gold = long.MaxValue;
            }

            return _gameData.playerData.gold;

        }
        private set
        {
            _gameData.playerData.gold = value;/*UI 초기화*/
        }
    }

    public long Gem
    {
        get { return _gameData.playerData.gem; }
        set { _gameData.playerData.gem = value; }
    }

    public long EXP { get { return _gameData.playerData.currentExp; } private set { _gameData.playerData.currentExp = value; /*GetLevel()*/; } }
    public int Level
    {
        get { return _gameData.playerData.level; }
        private set
        {
            if (EXP >= MaxExp)
            {
                _gameData.playerData.level = value;
                LevelAction?.Invoke();
            }
            /*UI 초기화*/
        }
    }
    public InventoryData InventoryData { get { return _gameData.inventoryItem; } private set { _gameData.inventoryItem = value;  /*UI 초기화*/} }
    public StageData StageData { get { return _gameData.stageData; } private set { _gameData.stageData = value;  /*UI 초기화*/} }
    public QuestData QuestData { get { return _gameData.questData; } private set { _gameData.questData = value;  /*UI 초기화*/} }
    //public EquipmentData EquipmentData { get { return _gameData._equipmentData; } private set { _gameData._equipmentData = value;  /*UI 초기화*/} }
    public float PlayerDamage { get { return _gameData.playerData.playerDamage; } private set { _gameData.playerData.playerDamage = value; } }
    public PlayerData PlayerData { get { return _gameData.playerData; } }
    public UpgadeData PlayerUpgradeData { get { return _gameData.upgadeData; } private set { _gameData.upgadeData = value; } }
    public long TotalDamage
    {
        get { return (long)(EquipItemData.itemDamage * PlayerDamage); }
    }
    //public long Damage { get { return CalculateDamage(); } /*private set { _gameData.damage = value; } */}
    public long CurrentExp { get; private set; }
    public long MaxExp { get { return (long)(2 * Mathf.Pow(3, Level)); } }

    /*
             int current = Managers.Game.StageData.currentStage;

        int reward = (int)(Mathf.Pow(2f, current) * 2);

        return reward;
     */

    public ItemData EquipItemData { get { return _gameData.inventoryItem.item[UseEquipment]; } private set { _gameData.inventoryItem.item[UseEquipment] = value; } }
    public MarketData MarketData { get { return _gameData.marketData; } private set { _gameData.marketData = value; } }



    public bool _eggDie;

    public Action LevelAction { get; private set; }






    #region 스폰 변수[안씀]
    GameObject _players;
    string eggPath = "/Eggs/Egg";
    HashSet<GameObject> _eggs = new HashSet<GameObject>();
    #endregion
    #region 만약을 위해 만든 스폰
    /// <summary>
    /// 게임 오브젝트 스폰생성
    /// </summary>
    /// <param name="type"></param>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {

        path = eggPath;

        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                _eggs.Add(go);
                break;
            case Define.WorldObject.Player:
                _players = go;
                break;
            case Define.WorldObject.Item:
                //_items = go;
                break;
        }
        return go;
    }

    /// <summary>
    /// 해당 게임오브젝트의 Define.WorldObjects 타입을 확인.
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        Base.Stat bs = go.GetComponent<Base.Stat>();

        if (bs == null)
            return Define.WorldObject.None;

        return bs._contentType;
    }
    /// <summary>
    /// 스폰생성된 오브젝트 Destroy
    /// </summary>
    /// <param name="go"></param>
    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);
        switch (type)
        {
            case Define.WorldObject.Monster:
                if (_eggs.Contains(go))
                {
                    _eggs.Remove(go);
                }
                break;
            case Define.WorldObject.None:
                break;
            case Define.WorldObject.Player:
                if (_players == go)
                    _players = null;
                break;
            case Define.WorldObject.Item:
                break;
        }
        Managers.Resource.Destroy(go);
    }
    #endregion


    public void Init()
    {
        if (File.Exists(_path) == true)
        {
            LoadGame();
        }
        else
        {
            FirstStartStat();
        }
            NullEquipSetting();

    }


    #region Stage Option

    //const int HUNDRED = 100;
    //[SerializeField] float _maxExpMultiplier = 1.15f; // 1.07 - 1.15
    //[SerializeField] float _rewardMultiplier = 1.1f; // 1.07 - 1.15

    public int StageReward(int CurrentStage)
    {
        int current = Managers.Game.StageData.currentStage / 5;

        int reward = (int)(Managers.Game.StageData.currentStage * Mathf.Pow(10, current));

        return reward;
    }

    #endregion

    #region Stage
    public Transform eggTr;
    public Transform shadowTr;
    public void SetEggTransform(Transform egg)
    {
        eggTr = egg;
    }

    public void SetShadowTransform(Transform shadow)
    {
        shadowTr = shadow;
    }
    public bool StageUp()
    {

        if (_eggDie == true)
            return false;

        if (StageData.currentStage > StageData.clearStage)
        {
            StageData.currentStage = StageData.clearStage;
            SetStageData(StageData.currentStage);
            return false;
        }

        else
        {
            Managers.Stage.StageMapCheck();
            StageData.currentStage++;
            SetStageData(StageData.currentStage);
            return true;
        }

    }
    public bool StageDown()
    {

        if (_eggDie == true)
            return false;

        if (StageData.currentStage <= 1)
        {
            StageData.currentStage = 1;
            SetStageData(StageData.currentStage);

            return false;
        }

        else
        {
            StageData.currentStage--;
            SetStageData(StageData.currentStage);
            Managers.Stage.PrevStageMapCheck();
            return true;
        }

    }

    public void EggKill()
    {

        if (StageData.currentStage % 5 == 0) // 5스테이지마다 보스 스테이지로 체크
        {
            if (StageData.clearStage == StageData.currentStage)
            {
                StageClear();
                Debug.Log("Boss Kill");
            }
        }
        else
        {
            if (StageData.clearStage == StageData.currentStage)
            {
                Managers.Game.StageData.killCount += 1;

                if (Managers.Game.StageData.killCount > 4)
                {
                    StageClear();
                }
            }

        }





    }

    public void StageClear()
    {
        /*
        if (StageData.clearStage >= Managers.Data.EggSO.Count)
            return;
        */

        if (StageData.clearStage == StageData.currentStage)
        {
            StageData.clearStage++;
            Managers.Game.StageData.killCount = 0;
        }
    }

    private void SetStageData(int id)
    {

        StageData.currentStage = id;
        StageData.eggID = Managers.Data.EggDic[Managers.Data.EggImageIndex].eggID;

        /*
        if (id > 50)
        {
            //long maxHp = Managers.Data.EggSO[Managers.Data.EggSO.Count - 1].MaxHp * (long)Math.Pow(2, id);

            if (maxHp <= 0)
                maxHp = long.MaxValue;

            StageData.maxHp = maxHp >= long.MaxValue ? long.MaxValue : maxHp;
        }
        else
            StageData.maxHp = Managers.Data.EggSO[id].MaxHp;
        */

        long maxHp = 20 * (long)Math.Pow(1.3, id);

        if (maxHp <= 0)
            maxHp = long.MaxValue;

        StageData.maxHp = maxHp >= long.MaxValue ? long.MaxValue : maxHp;

        Managers.Game.EggHPCharge();
        Managers.Game.StageDataRefresh();
    }

    public void EggHPCharge()
    {
        StageData.currentHp = StageData.maxHp;
    }

    #endregion

    #region Effect
    public void NullEquipSetting()
    {
        if (_gameData.inventoryItem.equip == null)
        {
            for (int i = 0; i < _gameData.inventoryItem.hasItemList.Count; i++)
            {
                if (_gameData.inventoryItem.item.TryGetValue(_gameData.inventoryItem.hasItemList[i], out ItemData itemData))
                {
                    if (itemData.itemType == Define.ItemType.Equip)
                    {
                        _gameData.inventoryItem.equip = itemData.itemId;
                        break;
                    }
                    else
                        continue;

                }
            }
        }


        // null check
        if (UseEquipment == null)
            UseEquipment = _gameData.inventoryItem.equip;
        GetLevel();
    }


    public void ChangeEquipEffect(string id)
    {
        if (UseEquipment != id)
        {
            InventoryData.equip = id;
            UseEquipment = id;
        }
        else
            UseEquipment = id;
    }

    public void SetLevelAction(Action ac)
    {
        LevelAction = ac;
    }
    #endregion

    #region Test

    void GetLevel()
    {
        CurrentExp = EXP;

        if (CurrentExp >= MaxExp)
        {
            CurrentExp -= MaxExp;
            Level++;

            _gameData.playerData.playerDamage += 0.1f;


            if (CurrentExp < 0)
                CurrentExp *= -1;

            EXP = CurrentExp;
        }

    }
    public void TestDamageUp()
    {
        _gameData.playerData.playerDamage += 55550;
    }

    public void Clear50Stage()
    {
        _gameData.stageData.clearStage = Managers.Data.EggDic.Count;
    }

    public void ClearAllStage()
    {
        _gameData.stageData.clearStage = 1000;
    }

    public void Add1MGold()
    {
        Gold += 1000000;
    }
    public void Add1MGem()
    {
        Gem += 1000000;
    }

    public void AddHere()
    {
        foreach (string item in Managers.Data.HerorLIST)
        {
            Additem(item);
        }
    }
    public void AddLegend()
    {
        foreach (string item in Managers.Data.LegendLIST)
        {
            Additem(item);
        }
    }
    public void AddUnique()
    {
        foreach (string item in Managers.Data.UniqueLIST)
        {
            Additem(item);
        }
    }

    public void AddRare()
    {
        foreach (string item in Managers.Data.RareLIST)
        {
            Additem(item);
        }
    }

    public void AddCommon()
    {
        foreach (string item in Managers.Data.CommonLIST)
        {
            Additem(item);
        }
    }

    #endregion

    void BuySound()
    {
        Managers.Sound.Play("Sounds/Coins_01");
    }

    #region plus & minus

    private int SheildAttack(Define.Grade grade)
    {
        int shieldAttack = 0;
        switch (grade)
        {
            case Define.Grade.None:
                shieldAttack = 0;
                break;
            case Define.Grade.Common:
                shieldAttack = 0;
                break;
            case Define.Grade.Rare:
                shieldAttack = 10;
                break;
            case Define.Grade.Unique:
                shieldAttack = 20;
                break;
            case Define.Grade.Legend:
                shieldAttack = 30;
                break;
            case Define.Grade.Hero:
                shieldAttack = 40;
                break;
        }

        return shieldAttack;
    }
    public void Additem(string id, int count = 1)
    {
        Managers.Data.ItemDic.TryGetValue(id, out ItemScriptbale iSO);

        if (iSO.itemType == Define.ItemType.GEM)
        {
            AddGem(iSO.rewardGem);
        }
        else if (iSO.itemType == Define.ItemType.GOLD)
        {
            AddGold(iSO.rewardGold);
        }
        else
        {

            if (_gameData.inventoryItem.item.TryGetValue(id, out ItemData itemData))
            {
                _gameData.inventoryItem.item[id].itemCount += count;
                //Debug.Log($"{id} : (중복!)갯수 추가 총 갯수 : {_gameData.inventoryItem.item[id].itemCount}");

            }
            else
            {
                _gameData.inventoryItem.item.Add(id, new ItemData(id, count));
                _gameData.inventoryItem.hasItemList.Add(id);

                _gameData.inventoryItem.item[id].itemDamage = Managers.Data.ItemDic[id].damage;
                _gameData.inventoryItem.item[id].itemCriticalPercent = Managers.Data.ItemDic[id].criticalPercent;
                _gameData.inventoryItem.item[id].itemCriticalPlusDamage = Managers.Data.ItemDic[id].criticalDamage;
                _gameData.inventoryItem.item[id].reinforce = 0;
                _gameData.inventoryItem.item[id].itemType = Managers.Data.ItemDic[id].itemType;
                _gameData.inventoryItem.item[id].itemId = id;
                _gameData.inventoryItem.item[id].itemName = Managers.Data.ItemDic[id].itemName;
                _gameData.inventoryItem.item[id].itemGrade = Managers.Data.ItemDic[id].Grade;
                _gameData.inventoryItem.item[id].shieldAttack = SheildAttack(Managers.Data.ItemDic[id].Grade);
                //Debug.Log($"{id} : 아이템 추가");
            }
        }

        Managers.Game.SaveGame("Additem");
        NotifiedInventory();

    }
    public void AddGold(int gold)
    {
        _gameData.playerData.gold += gold;
        Managers.Game.SaveGame("AddGold");
    }

    public void AddGem(int gem)
    {
        _gameData.playerData.gem += gem;
        Managers.Game.SaveGame("AddGem");
    }
    public void AddExp(int exp)
    {
        _gameData.playerData.currentExp += exp;
        _gameData.playerData.totalExp += exp;
        GetLevel();
        Managers.Game.SaveGame("AddExp");
    }

    public void MinusGold(long gold)
    {
        BuySound();
        _gameData.playerData.usingGold += gold;
        _gameData.playerData.gold -= gold;
        Managers.Game.SaveGame("MinusGold");
    }

    public void MinusGem(long gem) 
    {
        BuySound();
        _gameData.playerData.usingGem += gem;
        _gameData.playerData.gem -= gem;
        Managers.Game.SaveGame("MinusGold");
    }

    public void MinusItem(string id, int count = 1)
    {
        if (Managers.Game.InventoryData.item.TryGetValue(id, out ItemData iData))
        {
            iData.itemCount -= count;
            if (iData.itemCount < 0)
                iData.itemCount = 0;
        }
        else
            return;


    }
    public void AddWeapon(string id)
    {

        if (_gameData.inventoryItem.item.TryGetValue(id, out ItemData getsu))
        {
            _gameData.inventoryItem.item[id].itemCount++;
            Debug.Log($"{id} : (중복!)갯수 추가 총 갯수 : {_gameData.inventoryItem.item[id].itemCount}");

        }
        else
        {
            _gameData.inventoryItem.item.Add(id, new ItemData(id));
            _gameData.inventoryItem.hasItemList.Add(id);


            {
                _gameData.inventoryItem.item[id].itemDamage = Managers.Data.ItemDic[id].damage;
                _gameData.inventoryItem.item[id].itemCriticalPercent = Managers.Data.ItemDic[id].criticalPercent;
                _gameData.inventoryItem.item[id].itemCriticalPlusDamage = Managers.Data.ItemDic[id].criticalDamage;
                _gameData.inventoryItem.item[id].reinforce = 0;
                _gameData.inventoryItem.item[id].itemType = Managers.Data.ItemDic[id].itemType;
                _gameData.inventoryItem.item[id].itemId = id;
                _gameData.inventoryItem.item[id].itemName = Managers.Data.ItemDic[id].itemName;
                _gameData.inventoryItem.item[id].itemGrade = Managers.Data.ItemDic[id].Grade;
                _gameData.inventoryItem.item[id].shieldAttack = SheildAttack(Managers.Data.ItemDic[id].Grade);
            }

            Debug.Log($"{id} : Weapon 추가");
        }

    }

    public void MinusMarketItemCount(string id)
    {
        _gameData.marketData.currentItem[id]--;

        SaveGame("MinusMarketItemCount");
    }

    public void MinusMarketGemItemCount(string id)
    {
        _gameData.marketData.gemItem[id]--;

        SaveGame("MinusMarketItemCount");
    }

    public void UpgradeItem(string id)
    {
        if (_gameData.inventoryItem.item.TryGetValue(id, out ItemData data))
        {
            data.reinforce++;
            _gameData.inventoryItem.item[id].reinforce = data.reinforce;

        }
        else
            return;

        CalculateUpgrade(id);
        AllDataRefreshAndSaveGame();

    }

    // 플레이어 데이터 추가
    public void UpgradePlayerDamageUp()
    {
        _gameData.playerData.playerDamage += 0.1f;
        _gameData.upgadeData.damageUpLevel++;
    }
    public void UpgradePlayerGoldBonus()
    {
        _gameData.playerData.goldBonus += 0.1f;
        _gameData.upgadeData.goldBonusLevel++;
    }
    public void UpgradePlayerXpBonus()
    {
        _gameData.playerData.xpBonus += 0.1f;
        _gameData.upgadeData.xpBonusLevel++;
    }
    public void UpgradePlayerItemDropRate()
    {
        _gameData.playerData.itemDropRate += 0.1f;
        _gameData.upgadeData.itemDropRateLevel++;
    }

    public void UpgradePlayerData(string s)
    {
        switch (s)
        {
            case "Gold_per_click":
                _gameData.playerData.clickPerGold += 1;
                break;
            case "XP_per_click":
                _gameData.playerData.clickPerExp += 1;
                break;
        }
    }
    #endregion

    #region Quest

    public void ClickCount()
    {
        Managers.Game.PlayerData.clickCount++;
    }
    public void Reinforce(int count)
    {
        Managers.Game.PlayerData.usingGold += count;
    }
    public void Reinforce()
    {
        Managers.Game.PlayerData.usingGold++;
    }

    public void ClickQuestLevelUP(int _hasPoint, string rewardId = null)
    {

        int count = Managers.Game.PlayerData.clickCount - _hasPoint;

        if (count < 0 || rewardId == null)
            return;

        Managers.Game.PlayerData.clickCount = count;
        Managers.Game.QuestData.clickCountQuestLevel++;

        Managers.Game.Additem(rewardId);

        int level = Managers.Game.QuestData.clickCountQuestLevel;
        Managers.Quest.RewardCheck(level, Define.QuestType.Click);

    }
    public void GoldQuestLevelUP(int _hasPoint, string rewardId = null)
    {
        long count = Managers.Game.PlayerData.usingGold - _hasPoint;

        if ((count) < 0 || rewardId == null)
            return;

        Managers.Game.PlayerData.usingGold = count;
        Managers.Game.QuestData.usingGoldQuestLevel++;
        Managers.Game.Additem(rewardId);

        int level = Managers.Game.QuestData.usingGoldQuestLevel;
        Managers.Quest.RewardCheck(level, Define.QuestType.Gold);
    }

    public void GemQuestLevelUP(int _hasPoint, string rewardId = null)
    {
        long count = Managers.Game.PlayerData.usingGem - _hasPoint;

        if ((count) < 0 || rewardId == null)
            return;

        Managers.Game.PlayerData.usingGem = count;
        Managers.Game.QuestData.usingGemQuestLevel++;
        Managers.Game.Additem(rewardId);

        int level = Managers.Game.QuestData.usingGemQuestLevel;
        Managers.Quest.RewardCheck(level, Define.QuestType.Gem);
    }


    public void ReinforceQuestLevelUP(int _hasPoint, string rewardId = null)
    {

        int count = Managers.Game.PlayerData.reinforceCount - _hasPoint;

        if ((count) < 0 || rewardId == null)
            return;

        Managers.Game.PlayerData.reinforceCount = count;
        Managers.Game.QuestData.reinforceCountGoldQuestLevel++;
        Managers.Game.Additem(rewardId);

        int level = Managers.Game.QuestData.reinforceCountGoldQuestLevel;
        Managers.Quest.RewardCheck(level, Define.QuestType.Reinforce);
    }
    #endregion

    #region Reinforce

    public void DoReninforce(string id, int count, long addDmg, int goldOrGem)
    {
        if(addDmg == 0)
        {
            addDmg = 1;
        }

        _gameData.inventoryItem.item[id].itemCount = count;
        _gameData.inventoryItem.item[id].reinforce++;
        _gameData.inventoryItem.item[id].itemDamage += addDmg;
        _gameData.inventoryItem.item[id].shieldAttack += (int)Managers.Reinforce.CalShiedAttack(id);
        _gameData.inventoryItem.item[id].itemCriticalPercent += _gameData.inventoryItem.item[id].itemCriticalPercent * Managers.Reinforce.CalCriPer(id);
        _gameData.inventoryItem.item[id].itemCriticalPlusDamage += _gameData.inventoryItem.item[id].itemCriticalPlusDamage * Managers.Reinforce.CalCriDmg(id);
        _gameData.playerData.reinforceCount++;

        if (_gameData.inventoryItem.item[id].itemCriticalPlusDamage * Managers.Reinforce.CalCriDmg(id) == 0)
        {
            _gameData.inventoryItem.item[id].itemCriticalPlusDamage += 0.1f;
        }




        Managers.Game.SaveGame("DoReninforce");
    }

    public void DoReinforceGold(string id, long addDmg, long gold=0)
    {

        if (addDmg == 0)
        {
            addDmg = 1;
        }

        _gameData.inventoryItem.item[id].reinforce++;
        _gameData.inventoryItem.item[id].itemDamage += addDmg;
        _gameData.inventoryItem.item[id].shieldAttack += (int)Managers.Reinforce.CalShiedAttack(id);
        _gameData.inventoryItem.item[id].itemCriticalPercent += _gameData.inventoryItem.item[id].itemCriticalPercent * Managers.Reinforce.CalCriPer(id);
        _gameData.inventoryItem.item[id].itemCriticalPlusDamage += _gameData.inventoryItem.item[id].itemCriticalPlusDamage * Managers.Reinforce.CalCriDmg(id);
        _gameData.playerData.reinforceCount++;

        if (_gameData.inventoryItem.item[id].itemCriticalPlusDamage * Managers.Reinforce.CalCriDmg(id) == 0)
        {
            _gameData.inventoryItem.item[id].itemCriticalPlusDamage += 0.1f;
        }

        MinusGold(gold);

        Managers.Game.SaveGame("DoReinforceGold");
    }

    public void DoReinforceGem(string id, long addDmg, long gem=0)
    {

        if (addDmg == 0)
        {
            addDmg = 1;
        }


        _gameData.inventoryItem.item[id].reinforce++;
        _gameData.inventoryItem.item[id].itemDamage += addDmg;
        _gameData.inventoryItem.item[id].shieldAttack += (int)Managers.Reinforce.CalShiedAttack(id);
        _gameData.inventoryItem.item[id].itemCriticalPercent += _gameData.inventoryItem.item[id].itemCriticalPercent * Managers.Reinforce.CalCriPer(id);
        _gameData.inventoryItem.item[id].itemCriticalPlusDamage += _gameData.inventoryItem.item[id].itemCriticalPlusDamage * Managers.Reinforce.CalCriDmg(id);
        _gameData.playerData.reinforceCount++;

        if (_gameData.inventoryItem.item[id].itemCriticalPlusDamage * Managers.Reinforce.CalCriDmg(id) == 0)
        {
            _gameData.inventoryItem.item[id].itemCriticalPlusDamage += 0.1f;
        }

        MinusGem(gem);


        Managers.Game.SaveGame("DoReinforceGem");
    }


    public void DoGoldReninforce(string id, int gold)
    {
        MinusGold((int)gold);
        _gameData.playerData.gold -= gold;
        _gameData.inventoryItem.item[id].reinforce++;
        Managers.Game.SaveGame("DoGoldReninforce");
    }

    public void DoGemReninforce(string id, long gold)
    {
        _gameData.inventoryItem.item[id].reinforce++;
        Managers.Game.SaveGame("DoGemReninforce");
    }


    #endregion

    #region Notified
    public bool InventoryNotified { get; private set; }
    public bool StageNotified { get; private set; }
    public bool EggDieAlarm { get; private set; }

    public void NotifiedInventory()
    {
        InventoryNotified = true;
    }

    public bool CheckInventory()
    {
        InventoryNotified = false;

        return InventoryNotified;
    }

    public void NotifiedStage()
    {
        StageNotified = true;
    }

    public bool CheckStage()
    {
        StageNotified = false;

        return StageNotified;
    }

    public void NotifiedEggDie()
    {
        EggDieAlarm = true;
    }

    public void NotifiedEggReborn()
    {
        EggDieAlarm = false;
    }


    #endregion

    #region Market

    public void SetCurrentMarketList(Dictionary<string, int> dic)
    {
        Managers.Game.MarketData.currentItem.Clear();
       var sortedDict = dic.OrderBy(entry => entry.Value);
        dic = sortedDict.ToDictionary(entry => entry.Key, entry => entry.Value);

        Managers.Game.MarketData.currentItem = dic;
    }

    public void SetCurrentGemMarketList(Dictionary<string, int> dic)
    {
        Managers.Game.MarketData.gemItem.Clear();
        var sortedDict = dic.OrderBy(entry => entry.Value);
        dic = sortedDict.ToDictionary(entry => entry.Key, entry => entry.Value);

        Managers.Game.MarketData.gemItem = dic;
    }

    public void SetNextMarketList(Dictionary<string, int> dic)
    {
        var sortedDict = dic.OrderBy(entry => entry.Value);
        dic = sortedDict.ToDictionary(entry => entry.Key, entry => entry.Value);

        Managers.Game.MarketData.nextItem = dic;
        Managers.Game.SaveGame("SetNextMarketList");
    }

    public void SetCurrentAndNextMarketList(Dictionary<string, int> curDic, Dictionary<string, int> nextDic, Dictionary<string, int> adDic, Dictionary<string, int> gemDic)
    {
        Managers.Game.MarketData.currentItem = curDic;
        Managers.Game.MarketData.nextItem = nextDic;
        Managers.Game.MarketData.addItem = adDic;
        Managers.Game.MarketData.gemItem = gemDic;

        SaveGame("SetCurrentAndNextMarketList");
    }
    public void SetMarektFirstTime(DateTime dt1, DateTime dt2, DateTime dt3)
    {
        Managers.Game.MarketData.autoResetTime = dt1;
        Managers.Game.MarketData.freeResetTime = dt2;
        Managers.Game.MarketData.adResetTime = dt3;

        SaveGame("SetMarektFirstTime");

    }

    public void SetAutoResetTime(DateTime dt)
    {
        Managers.Game.MarketData.autoResetTime = dt;
    }
    public void SetFreeResetTime(DateTime dt)
    {
        Managers.Game.MarketData.freeResetTime = dt;
    }
    public void SetAdResetTime(DateTime dt)
    {
        Managers.Game.MarketData.adResetTime = dt;
    }

    #endregion

    #region Data


    // 시작 데이터
    public void FirstStartStat()
    {
        Debug.Log($"Start Data init");
        StartStat startStat = Managers.Data._startStatDic[1];

        // 플레이어 데이터
        _gameData.playerData = startStat.playerData;

        // 인벤토리
        _gameData.inventoryItem = startStat.inventoryItem;

        // 스테이지 정보
        _gameData.stageData = startStat.stageData;

        // 이펙트 정보
        _gameData.inventoryItem.equip = startStat.inventoryItem.equip;

        // 업그레이드 데이터
        _gameData.upgadeData = startStat.upgadeData;

        // 퀘스트 데이터
        _gameData.questData = startStat.questData;

        // 퀘스트 데이터
        _gameData.marketData = startStat.marketData;


        GameDataRefresh();
        StageDataRefresh();
        InventoryDataRefresh();
        PlayerUpgradeDataRefresh();

    }

    // 모든 데이터 업데이트후 저장
    public void AllDataRefreshAndSaveGame()
    {
        GameDataRefresh();
        StageDataRefresh();
        InventoryDataRefresh();
        PlayerUpgradeDataRefresh();
        GetLevel();

        Managers.Game.SaveGame("AllDataRefreshAndSaveGame()");
    }
    // 모든 데이터 업데이트
    public void AllDataRefresh()
    {
        GameDataRefresh();
        StageDataRefresh();
        InventoryDataRefresh();
        PlayerUpgradeDataRefresh();
        GetLevel();

        //Managers.Game.SaveGame("AllDataRefresh");
    }

    public void PlayerUpgradeDataRefresh()
    {
        PlayerUpgradeData = _gameData.upgadeData;
    }

    // 플레이어 데이터 업데이트
    public void GameDataRefresh()
    {
        Level = _gameData.playerData.level;
        Gold = _gameData.playerData.gold;
        EXP = _gameData.playerData.currentExp;
        //Managers.Game.SaveGame("GameDataRefresh");
    }
    // 스테이지 데이터 업데이트
    public void StageDataRefresh()
    {
        StageData = _gameData.stageData;
        //Managers.Game.SaveGame("StageDataRefresh");
    }
    // 인벤토리 데이터 업데이트
    public void InventoryDataRefresh()
    {
        // 인벤토리
        InventoryData = _gameData.inventoryItem;
        //Managers.Game.SaveGame("InventoryDataRefresh");
    }

    // 장착 이펙트 업데이트
    #endregion


    #region Damage

    private void CalculateUpgrade(string id)
    {
        _gameData.inventoryItem.item[id].itemDamage = _gameData.inventoryItem.item[id].itemDamage + (long)((0.2 * Managers.Game.InventoryData.item[id].reinforce) * Managers.Game.InventoryData.item[id].itemDamage);
        _gameData.inventoryItem.item[id].itemCriticalPercent += 0.1f;
        _gameData.inventoryItem.item[id].itemCriticalPlusDamage += 0.2f;

        GameDataRefresh();
    }

    public long CalculateDamage()
    {

        int isCiritical = UnityEngine.Random.Range(0, 100);
        float randomDamage = 0;

        if (Managers.Game.EquipItemData.itemCriticalPercent >= isCiritical)
        {
            randomDamage = UnityEngine.Random.Range(PlayerDamage + 2f, PlayerDamage + Managers.Game.EquipItemData.itemCriticalPlusDamage);
        }
        else
        {
            randomDamage = UnityEngine.Random.Range(PlayerDamage, PlayerDamage + 0.3f);
        }



        long totalDamage = (long)(randomDamage * TotalDamage);


        int i = UnityEngine.Random.Range(0, 100);

        return totalDamage;


    }

    #endregion


    #region Save & Load Data
    public string _path = Application.persistentDataPath + "/SaveData.json";
    public string _startPath = "Resources/Data/StartData.json";


    async Task SaveGameAsync(string requestFunc)
    {
        //string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
        string jsonSt = JsonConvert.SerializeObject(Managers.Game.SaveData);

        //암호화
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonSt);
        string code = System.Convert.ToBase64String(bytes);

        /*
         using (StreamWriter streamWriter = new StreamWriter(new FileStream(_path, FileMode.Create, FileAccess.Write, FileShare.None), Encoding.UTF8))
{
    await streamWriter.WriteAsync(code);
}
         */

        File.WriteAllText(_path, code);
        //Debug.Log($" request : {requestFunc} \n Save Game Completed : {_path}");
    }


    public async void SaveGame(string requestFunc)
    {
        await SaveGameAsync(requestFunc);
    }

    public void SaveGameOriginal(string requestFunc)
    {

        //string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
        string jsonSt = JsonConvert.SerializeObject(Managers.Game.SaveData);

        //암호화
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonSt);
        string code = System.Convert.ToBase64String(bytes);

        File.WriteAllText(_path, code);
        //Debug.Log($" request : {requestFunc} \n Save Game Completed : {_path}");
    }

    public async Task<bool> LoadGame()
    {

        bool b = await LoadGameAsync();

        return b;
    }

    async Task<bool> LoadGameAsync()
    {
        if (File.Exists(_path) == false)
        {
            Debug.Log("There is no save data.");
            return false;
        }

        GameData data = null;
        try
        {
            string fileStr = File.ReadAllText(_path);
            byte[] bytes = null;
            bytes = System.Convert.FromBase64String(fileStr);
            string jdata = System.Text.Encoding.UTF8.GetString(bytes);
            data = JsonConvert.DeserializeObject<GameData>(jdata);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }

        if (data != null)
        {
            Managers.Game.SaveData = data;
        }

        //Debug.Log($"Save Game Loaded : {_path}");

        return true;
    }


    public bool LoadGameOriginal()
    {

        if (File.Exists(_path) == false)
        {
            Debug.Log("There is no save data.");
            return false;
        }

        GameData data = null;
        try
        {
            string fileStr = File.ReadAllText(_path);
            byte[] bytes = null;
            bytes = System.Convert.FromBase64String(fileStr);
            string jdata = System.Text.Encoding.UTF8.GetString(bytes);
            data = JsonConvert.DeserializeObject<GameData>(jdata);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }

        if (data != null)
        {
            Managers.Game.SaveData = data;
        }

        Debug.Log($"Save Game Loaded : {_path}");


        return true;
    }
    #endregion

}



