using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class LootScriptable : ScriptableObject
{
    public Sprite _lootItemIcon;
    public string _lootName;
    public int _dropWeight;

    public LootScriptable(GameObject lootItemObject, Sprite lootItemIcon, string lootName, int dropChance)
    {
        this._lootItemIcon = lootItemIcon;
        this._lootName = lootName;
        this._dropWeight = dropChance;
    }

    public LootScriptable(LootScriptable lootSO)
    {
        this._lootItemIcon = lootSO._lootItemIcon;
        this._lootName = lootSO._lootName;
        this._dropWeight = lootSO._dropWeight;
    }
}
