using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using OfficeOpenXml;
using System;

public class ExcelToScriptableObject : EditorWindow
{
    [MenuItem("Tools/Excel To ScriptableObject")]
    private static void ShowWindow()
    {
        GetWindow<ExcelToScriptableObject>("Excel To ScriptableObject");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Convert"))
        {
            // Load the Excel file.
            //string excelPath = "D:/HYC/22-12-07/Hyper-Money-Games/Assets/Data/ExcelData.xlsx";
            //string fullPath = Path.Combine(Application.dataPath, excelPath);

            string filePath = Path.Combine(Application.dataPath, "Data/ExcelData.xlsx");
            FileInfo fileInfo = new FileInfo(filePath);


            if (fileInfo == null)
            {
                Debug.Log("No exist excel file");
                return;
            }
            else
            {
                List<ExcelItemData> excelItemList = LoadExcel(fileInfo);
                // Convert the Excel data to ScriptableObject.
                ConvertToScriptableObject(excelItemList);
            }
        }
    }

    private List<ExcelItemData> LoadExcel(FileInfo fileInfo)
    {
        //List<WeaponData> weaponList = new List<WeaponData>();
        List<ExcelItemData> itemList = new List<ExcelItemData>();

        // Check if the file exists.
        if (fileInfo == null || !fileInfo.Exists)
        {
            Debug.LogWarning("Excel file does not exist.");
            return itemList;
        }

        // Load the Excel package.
        using (var package = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];

            // Read the data from the Excel sheet.
            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {

                // code, name,damage,grade,itemType,itemCost,itemIconPath,itemObjectPath,itemDesc,itemObjectName
                // �ڵ�
                var code = worksheet.Cells[i, 1].Value?.ToString();
                // �̸�
                var name = worksheet.Cells[i, 2].Value?.ToString();
                // ������
                int.TryParse(worksheet.Cells[i, 3].Value?.ToString(), out var damage);

                // ���
                var grade = worksheet.Cells[i, 4].Value?.ToString();

                // ������ Ÿ��
                var itemType = worksheet.Cells[i, 5].Value?.ToString();

                // ������ ����
                int.TryParse(worksheet.Cells[i, 6].Value?.ToString(), out var itemCost);

                // ������ �ּ�
                var itemIconPath = worksheet.Cells[i, 7].Value?.ToString();

                // ����Ʈ ������Ʈ �ּ�
                var itemObjectPath = worksheet.Cells[i, 8].Value?.ToString();

                // ������ ����
                var itemDesc = worksheet.Cells[i, 9].Value?.ToString();
                //var itemDesc = "";

                var objName = worksheet.Cells[i, 10].Value?.ToString();

                // ����Ÿ��
                var soundType = worksheet.Cells[i, 11].Value?.ToString();

                if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(grade))
                {
                    var excelItem = new ExcelItemData(code, name, damage, grade, itemType, itemCost, itemIconPath, itemObjectPath, soundType);
                    itemList.Add(excelItem);
                }
            }
        }

        return itemList;
    }

    private void ConvertToScriptableObject(List<ExcelItemData> excelItemList)
    {
        foreach (var excelData in excelItemList)
        {
            var sObject = ScriptableObject.CreateInstance<ItemScriptbale>();
            sObject.itemID = excelData.itemID;
            sObject.itemName = excelData.itemName;
            sObject.damage = excelData.damage;
            sObject.Grade = excelData.grade;
            sObject.itemObject = excelData.itemObejct;
            sObject.itemType = excelData.itemType;
            sObject.itemIcon = excelData.itemIcon;
            sObject.itemCost = excelData.itemCost;
            sObject.itemObjectPath = excelData.itemObjectPath;
            sObject.itemIconPath = excelData.itemIconPath;
            sObject.soundType = excelData.soundType;
            sObject.criticalPercent = excelData.criticalPercent;
            sObject.criticalDamage = excelData.criticalDamage;



            // Save the ScriptableObject to the AssetDatabase.
            string input = sObject.itemID.Substring(0, 4);
            string folderPath = "";

            switch (input)
            {
                case "EF00":
                    Debug.Log("Category EF00");
                    folderPath = CreateFolde(input);
                    break;
                case "EF10":
                    Debug.Log("Category EF10");
                    folderPath = CreateFolde(input);
                    break;
                case "EF20":
                    Debug.Log("Category EF20");
                    folderPath = CreateFolde(input);
                    break;
                case "EF30":
                    Debug.Log("Category EF20");
                    folderPath = CreateFolde(input);
                    break;
                case "EF40":
                    Debug.Log("Category EF20");
                    folderPath = CreateFolde(input);
                    break;
                case "EF50":
                    Debug.Log("Category EF20");
                    folderPath = CreateFolde(input);
                    break;
                case "EF60":
                    Debug.Log("Category EF20");
                    folderPath = CreateFolde(input);
                    break;
                case "EF70":
                    Debug.Log("Category EF20");
                    folderPath = CreateFolde(input);
                    break;
                case "EF80":
                    Debug.Log("Category EF20");
                    folderPath = CreateFolde(input);
                    break;
                case "EF90":
                    Debug.Log("Category EF20");
                    folderPath = CreateFolde(input);
                    break;
                case "EF11":
                    Debug.Log("Category EF20");
                    folderPath = CreateFolde(input);
                    break;
                case "WF00":
                    Debug.Log("Category WF00");
                    folderPath = CreateFolde(input);
                    break;



            }





            var assetPath = $"{folderPath}/{sObject.itemID}.asset";

            Debug.Log(assetPath);
            AssetDatabase.CreateAsset(sObject, assetPath);
            AssetDatabase.SaveAssets();




            {
                /*
                var assetPath = "Assets/Data/Weapon/" + itemObject.code + ".asset";
                AssetDatabase.CreateAsset(itemObject, assetPath);
                AssetDatabase.SaveAssets();
                Debug.Log($"{assetPath} : Save");
                */
            }


        }


    }

    private string CreateFolde(string input)
    {
        var folderPath = $"Assets/Data/{input}";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/Data", $"{input}");
            Debug.Log($"Created {input} folder");
        }

        return folderPath;

    }
}

[Serializable]
public class TestItemData
{
    public string code;
    public string name;
    public int damage;
    public string rarity;
    public GameObject go;
    public Define.ItemType itemType;

    public TestItemData(string code, string name, int damage, string rarity, string itemType)
    {
        this.code = code;

        this.name = name;

        this.damage = damage;

        this.rarity = rarity;

        go = Resources.Load<GameObject>($"data/TEST");

        this.itemType = (Define.ItemType)Enum.Parse(typeof(Define.ItemType), itemType);
    }
}

[Serializable]
public class ExcelItemData
{
    public string itemID;
    public string itemName;
    public string itemDescription;
    public int itemCost;
    public string itemIconPath;
    public string itemBackGroundImagePath;
    public Define.ItemType itemType;
    public string getRouteItemTypeString;
    public List<string> recipies = new List<string>();
    public List<int> recipiesCount = new List<int>();
    public Define.Grade grade;
    public int canDropStage;
    public int cantDropStage;
    public int getItemStage;
    public string effectTypeString;
    public int damage;
    public float criticalPercent;
    public float criticalDamage;
    public string itemObjectPath;
    public GameObject itemObejct;
    public Sprite itemIcon;
    public Define.EffectSound soundType;


    // id �̸�  ������ ���� Ÿ�� �������ּ� ��� ������Ʈ�ּ�,����
    public ExcelItemData(string itemID, string itemName, int damage, string grade, string itemType, int itemCost, string itemIconPath, string itemObjectPath, string sound)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.itemDescription = itemName;
        this.itemCost = itemCost;
        this.itemIconPath = itemName;
        this.itemType = (Define.ItemType)Enum.Parse(typeof(Define.ItemType), itemType);
        this.grade = (Define.Grade)Enum.Parse(typeof(Define.Grade), grade);
        soundType = (Define.EffectSound)Enum.Parse(typeof(Define.EffectSound), sound);
        this.itemObjectPath = itemObjectPath;
        this.damage = damage;


        this.itemObejct = Resources.Load<GameObject>(itemObjectPath.ToString());
        this.itemIcon = Resources.Load<Sprite>(itemIconPath);

        if (itemObejct == null)
        {
            Debug.LogWarning($"itemObjectPath �߸��� ���{itemObjectPath}");
        }

        if (itemIcon == null)
        {
            Debug.LogWarning($"itemIconPath �߸��� ���{itemIconPath}");
        }

        switch (this.grade)
        {
            case Define.Grade.None:
                criticalPercent = 10;
                criticalDamage = 1.5f;
                break;
            case Define.Grade.Common:
                criticalPercent = 25;
                criticalDamage = 1.5f;
                break;
            case Define.Grade.Rare:
                criticalPercent = 30;
                criticalDamage = 2f;
                break;
            case Define.Grade.Unique:
                criticalPercent = 40;
                criticalDamage = 2.5f;
                break;
            case Define.Grade.Legend:
                criticalPercent = 45;
                criticalDamage = 3.25f;
                break;
            case Define.Grade.Hero:
                criticalPercent = 50;
                criticalDamage = 4f;
                break;
        }

    }
}
