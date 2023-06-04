using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu (fileName="New Item", menuName = "ScriptableObject/Item")]
public class ItemScriptbale : ScriptableObject
{
    [Header("������ ID")]
    public string itemID;
    [Header("������ �̸�")]
    public string itemName;
    [Header("������ ����")]
    [TextArea]
    public string itemDescription;
    [Header("����")]
    public int itemCost;
    [Header("������")]
    public Sprite itemIcon;
    [Header("��׶��� �̹���")]
    public Sprite itemBackGroundImage;
    [Header("������ Ÿ��")]
    public Define.ItemType itemType;
    [Header("������ ��� ��Ʈ")]
    public Define.GetRouteItemType getRouteItemType;
    [Header("���")]
    public Define.Grade Grade;
    [Header("������ ��� �������� �̻�")]
    public int canDropStage;
    [Header("������ ��� �������� ����")]
    public int cantDropStage;
    [Header("����/���� �ر� ��������")]
    public int getItemStage;
    [Header("����Ʈ Ÿ��")]
    public Define.EffectType effectType;
    [Header("������")]
    public int damage;
    [Header("ũ��Ƽ�� Ȯ��")]
    public float criticalPercent;
    [Header("ũ��Ƽ�� ������ ����")]
    public float criticalDamage;
    [Header("����Ʈ ������Ʈ")]
    public GameObject itemObject;
    [Header("����Ʈ ������Ʈ ��ġ")]
    public string itemObjectPath;
    [Header("������ ��ġ")]
    public string itemIconPath;
    [Header("���� Ȯ��")]
    public List<int> chestPercent;
    [Header("���� ���� �̹���")]
    public Sprite chesOpenImage;
    [Header("���� ����")]
    public int shieldAttack;
    [Header("��� ����")]
    public int rewardGold;
    [Header("���� ����")]
    public int rewardGem;
    [Header("����")]
    public int gem;
    [Header("���� Ÿ��")]
    public Define.EffectSound soundType;


    public List<string> FirstItemData()
    {
        List<string> list = new List<string>();

        list.Add("ID : " + itemID.ToString());
        list.Add("Name : " + itemName.ToString());
        list.Add("Description : " + itemDescription.ToString());
        list.Add("Price : " + itemCost.ToString());
        list.Add("Icon : " + itemIcon.ToString());
        list.Add("Type : " + itemType.ToString());
        list.Add("Route : " + getRouteItemType.ToString());
        list.Add("Grade : " + Grade.ToString());
        list.Add("Damage : " + damage.ToString());

        return list;

    }

    public GameObject GetClickEffect()
    {
        return Managers.Resource.Load<GameObject>(itemObjectPath+$"{itemID}");
    }
}



#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(ItemScriptbale))]
public class AssetPreviewer : Editor
{
    ItemScriptbale item;
    Texture2D icon;

    public override void OnInspectorGUI()
    {
        item = (ItemScriptbale)target;

        GUILayout.BeginHorizontal();
        icon = AssetPreview.GetAssetPreview(item.itemIcon);

        GUILayout.Label(icon);

        GUILayout.EndHorizontal();
        DrawDefaultInspector();

    }
}

#endif
