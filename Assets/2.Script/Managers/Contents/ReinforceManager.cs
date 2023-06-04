using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceManager
{

    public GameObject UpgradeEffect { get; private set; }
    private AudioClip reinforceAudioClip;

    public void Init()
    {
        UpgradeEffect = Managers.Resource.Load<GameObject>("Prefabs/Effects/Reinforce/UpgradeEffect");
        reinforceAudioClip = Managers.Resource.Load<AudioClip>("Sounds/Blacksmith 1_3");
    }


    public bool CanReinforce(string id, int goldOrGem = 0)
    {
        ItemData Itemdata = null;


        if (Managers.Game.InventoryData.item.TryGetValue(id, out Itemdata))
        {

            int itemcount = Itemdata.itemCount - 1;
            int reinfoceCount = Itemdata.reinforce + 1;

            if (itemcount >= reinfoceCount)
            {
                int remianCount = itemcount - reinfoceCount;

                long addDmg = CalReinforceDMG(Itemdata);
                Managers.Game.DoReninforce(id, remianCount + 1, addDmg, goldOrGem);
                Managers.Sound.Play(reinforceAudioClip);
                return true;
            }
            else
                return false;

        }
        else
            return false;
    }

    public bool GoldUpgrade(string id)
    {
        long rg = RequrieGoldCheck(id);
        Managers.Game.InventoryData.item.TryGetValue(id, out ItemData Itemdata);

        if (Managers.Game.Gold >= rg)
        {
            Managers.Game.DoReinforceGold(id, CalReinforceDMG(Itemdata), rg);
            Managers.Sound.Play("Blacksmith 1_3");
            return true;
        }
        else
            return false;

    }


    public long Get10GoldUpgrade(string id)
    {
        Managers.Game.InventoryData.item.TryGetValue(id, out ItemData Itemdata);
        long rg = 0;
        for (int i = 0; i < 10; i++)
        {
            rg += RequrieGoldCheck(Itemdata.itemGrade, Itemdata.reinforce + i);
        }

        return rg;
    }

    public long Get10GemUpgrade(string id)
    {
        Managers.Game.InventoryData.item.TryGetValue(id, out ItemData Itemdata);
        long rg = 0;
        for (int i = 0; i < 10; i++)
        {
            rg += RequireGemCheck(Itemdata.itemGrade, Itemdata.reinforce + i);
        }
        return rg;
    }



    public bool Gold10Upgrade(string id)
    {
        Managers.Game.InventoryData.item.TryGetValue(id, out ItemData Itemdata);
        long rg = 0;
        for (int i = 0; i < 10; i++)
        {
            rg += RequrieGoldCheck(Itemdata.itemGrade, Itemdata.reinforce+i);
        } 

        if (Managers.Game.Gold >= rg)
        {
            for (int i = 0; i < 10; i++)
            {
                Managers.Game.DoReinforceGold(id, CalReinforceDMG(Itemdata));
            }
            Managers.Game.MinusGold(rg);
            Managers.Sound.Play("Blacksmith 1_3");
            return true;
        }
        else
            return false;

    }

    public bool Gem10Upgrade(string id)
    {
        Managers.Game.InventoryData.item.TryGetValue(id, out ItemData Itemdata);
        long rg = 0;
        for (int i = 0; i < 10; i++)
        {
            rg += RequireGemCheck(Itemdata.itemGrade, Itemdata.reinforce + i);
        }
        
        if (Managers.Game.Gem >= rg)
        {
            for (int i = 0; i < 10; i++)
            {
                Managers.Game.DoReinforceGem(id, CalReinforceDMG(Itemdata));
            }
            Managers.Game.MinusGem(rg);
            Managers.Sound.Play("Blacksmith 1_3");
            return true;
        }
        else
            return false;
    }

    public long CalShiedAttack(string id)
    {

        long sA = 0;

        int a = Managers.Data.ItemDic[id].shieldAttack;

        if (a > 0)
        {
            sA += (int)(Managers.Game.InventoryData.item[id].itemGrade) * 20;
        }

        return sA;
    }


    public bool GemUpgrade(string id)
    {
        long rg = RequireGemCheck(id);
        Managers.Game.InventoryData.item.TryGetValue(id, out ItemData Itemdata);

        if (Managers.Game.Gem >= rg)
        {
            Managers.Game.DoReinforceGem(id, CalReinforceDMG(Itemdata), rg);
            Managers.Sound.Play("Blacksmith 1_3");
            return true;
        }
        else
            return false;
    }

    public float CalCriPer(string id)
    {

        float c = 0;

        switch (Managers.Data.ItemDic[id].Grade)
        {
            case Define.Grade.None:
                c = 0.01f;
                break;
            case Define.Grade.Common:
                c = 0.01f;
                break;
            case Define.Grade.Rare:
                c = 0.02f;
                break;
            case Define.Grade.Unique:
                c = 0.03f;
                break;
            case Define.Grade.Legend:
                c = 0.04f;
                break;
            case Define.Grade.Hero:
                c = 0.05f;
                break;
        }

        return c;

    }

    public float CalCriDmg(string id)
    {

        float c = 0;

        switch (Managers.Data.ItemDic[id].Grade)
        {
            case Define.Grade.None:
                c = 0.05f;
                break;
            case Define.Grade.Common:
                c = 0.05f;
                break;
            case Define.Grade.Rare:
                c = 0.10f;
                break;
            case Define.Grade.Unique:
                c = 0.15f;
                break;
            case Define.Grade.Legend:
                c = 0.25f;
                break;
            case Define.Grade.Hero:
                c = 0.45f;
                break;
        }

        return c;
    }

    public long CalReinforceDMG(ItemData Itemdata)
    {

        Define.Grade gr = Define.Grade.None;
        long dmg = 0;

        switch (gr)
        {
            case Define.Grade.None:
                dmg = (long)(Itemdata.itemDamage * 0.2);
                break;
            case Define.Grade.Common:
                dmg = (long)(Itemdata.itemDamage * 0.3);
                //_maxUpgrade = 10;
                break;
            case Define.Grade.Rare:
                dmg = (long)(Itemdata.itemDamage * 0.4);
                //_maxUpgrade = 15;
                break;
            case Define.Grade.Unique:
                dmg = (long)(Itemdata.itemDamage * 0.5);
                //_maxUpgrade = 20;
                break;
            case Define.Grade.Legend:
                dmg = (long)(Itemdata.itemDamage * 0.7);
                //_maxUpgrade = 25;
                break;
            case Define.Grade.Hero:
                dmg = (long)(Itemdata.itemDamage * 0.9);
                //_maxUpgrade = 30;
                break;
            default:
                break;
        }

        return dmg;
    }


    public long RequrieGoldCheck(string id)
    {
        Managers.Game.InventoryData.item.TryGetValue(id, out ItemData itemData);
        long gold = 0;

        switch (itemData.itemGrade)
        {
            case Define.Grade.Common:
                gold = (long)(100 *Mathf.Pow(1.2f, itemData.reinforce));
                break;
            case Define.Grade.Rare:
                gold = (long)(1000 * Mathf.Pow(1.3f, itemData.reinforce));
                break;
            case Define.Grade.Unique:
                gold = (long)(10000*Mathf.Pow(1.4f, itemData.reinforce));
                break;
            case Define.Grade.Legend:
                gold = (long)(50000 *Mathf.Pow(1.4f, itemData.reinforce));
                break;
            case Define.Grade.Hero:
                gold = (long)(200000 *Mathf.Pow(1.4f, itemData.reinforce));
                break;
        }

        if (gold < 0)
            gold = long.MaxValue;

        return gold;
    }

    public long RequrieGoldCheck(Define.Grade grade, int level)
    {
        long gold = 0;

        switch (grade)
        {
            case Define.Grade.Common:
                gold = (long)(100 * Mathf.Pow(1.2f, level));
                break;
            case Define.Grade.Rare:
                gold = (long)(1000 * Mathf.Pow(1.3f, level));
                break;
            case Define.Grade.Unique:
                gold = (long)(10000 * Mathf.Pow(1.4f, level));
                break;
            case Define.Grade.Legend:
                gold = (long)(50000 * Mathf.Pow(1.4f, level));
                break;
            case Define.Grade.Hero:
                gold = (long)(200000 * Mathf.Pow(1.4f, level));
                break;
        }

        if (gold < 0)
            gold = long.MaxValue;

        return gold;
    }

    public long RequireGemCheck(Define.Grade grade, int level)
    {
        long gem = 0;

        switch (grade)
        {
            case Define.Grade.Common:
                gem = (long)(1 * Mathf.Pow(1.1f, level));
                break;
            case Define.Grade.Rare:
                gem = (long)(3 * Mathf.Pow(1.1f, level));
                break;
            case Define.Grade.Unique:
                gem = (long)(5 * Mathf.Pow(1.1f, level));
                break;
            case Define.Grade.Legend:
                gem = (long)(7 * Mathf.Pow(1.1f, level));
                break;
            case Define.Grade.Hero:
                gem = (long)(10 * Mathf.Pow(1.1f, level));
                break;
        }

        if (gem < 0)
            gem = long.MaxValue;


        return gem;
    }



    public long RequireGemCheck(string id)
    {
        Managers.Game.InventoryData.item.TryGetValue(id, out ItemData itemData);
        long gem = 0;

        switch (itemData.itemGrade)
        {
            case Define.Grade.Common:
                gem = (long)(1 * Mathf.Pow(1.1f, itemData.reinforce));
                break;
            case Define.Grade.Rare:
                gem = (long)(3 * Mathf.Pow(1.1f, itemData.reinforce));
                break;
            case Define.Grade.Unique:
                gem = (long)(5 * Mathf.Pow(1.1f, itemData.reinforce));
                break;
            case Define.Grade.Legend:
                gem = (long)(7 * Mathf.Pow(1.1f, itemData.reinforce));
                break;
            case Define.Grade.Hero:
                gem = (long)(10 * Mathf.Pow(1.1f, itemData.reinforce));
                break;
        }

        if (gem < 0)
            gem = long.MaxValue;


        return gem;
    }


    public int SheildAttack(Define.Grade grade)
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

    void c_MaxIdChecker(string id)
    {
        Define.Grade grade = Managers.Data.ItemDic[id].Grade;

        switch (grade)
        {
            case Define.Grade.None:
                //_maxUpgrade = 0;
                break;
            case Define.Grade.Common:
                //_maxUpgrade = 10;
                break;
            case Define.Grade.Rare:
                //_maxUpgrade = 15;
                break;
            case Define.Grade.Unique:
                //_maxUpgrade = 20;
                break;
            case Define.Grade.Legend:
                //_maxUpgrade = 25;
                break;
            case Define.Grade.Hero:
                //_maxUpgrade = 30;
                break;
            default:
                break;
        }
    }



}
