using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum WorldObject
    {
        None,
        Player,
        Monster,
        Item
    }

    public enum ItemType
    {
        None,
        Equip,
        Weapon,
        Ingredient,
        Chest,
        USE,
        ETC,
        BOX,
        GOLD,
        GEM
    }

    public enum EffectType
    {
        None,
        Hit,
        Fire,
        Water,
        Wind,
        Electricity,
    }

    public enum DamgeType
    {
        None,
        Basic,
        Dot
    }
    public enum GetRouteItemType
    {
        None,
        Craft,
        Market,
        OnlyEgg,
        All,
    }

    public enum Grade
    {
        None,
        Common, // White
        Rare, // Blue
        Unique, // Purple
        Legend, //  Orange
        Hero, // Red
    }


    public enum Scene
    {
        Default,
        Start,
        Login,
        Lobby,
        Game,

    }

    public enum EffectSound
    {
        NONE,
        Punch,
        Wood,
        ShortSword,
        Sword,
        Staff,
        Hammer,
        Bow,
        Claw,
        MAXCOUNT
    }
    public enum Sound
    {
        Bgm,
        Effect,
        Chest,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum TapType
    {
        None,
        Market,
        Craft,
        Inventory,
        Chest,
    }

    public enum QuestType
    {
        Click,
        Gold,
        Gem,
        Reinforce,
        End
    }

    // аж╪р
    public const string Prefabs_CARD_PATH = "Prefabs/Card/";
    public const string Prefabs_BAR_UI_PATH = "Prefabs/UI/BarUI/";
    public const string Prefabs_POPUP_UI_PATH = "Prefabs/UI/Popup/";
    public const string Prefabs_SUBITEM_U_PATHI = "Prefabs/UI/SubItem/";

    public const string CARD_PATH = "Card/";
    public const string BAR_UI_PATH = "UI/BarUI/";
    public const string POPUP_UI_PATH = "UI/Popup/";
    public const string SUBITEM_UI_PATH = "UI/SubItem/";
}
