using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


class path
{
     public string pt = "";
     public path()
    {
        pt = Application.persistentDataPath + "/SaveData.json";
    }
}

public class StartScene : MonoBehaviour
{
    // Start is called before the first frame update

    GameData _gameData = new GameData(); // 플레이어 데이터
    string _path;
    
    public Dictionary<int, StartStat> _startStatDic { get; private set; } = new Dictionary<int, StartStat>();

    public GameObject errorPopup;
    void Start()
    {

        Init();


    }

    public void Init()
    {
        errorPopup.SetActive(false);

        path a = new path();
        _path = a.pt;


        if (File.Exists(_path) == false)
        {
            StartStatLoadTest();
            FirstStartStat();
        }
        else
        {
            if (LoadGame() == false)
            {
                Time.timeScale = 0f;
                Debug.LogError("Save File Error");

                errorPopup.SetActive(true);
                errorPopup.GetComponent<UI_SaveFileErrorPopup>().InitData(_path, () =>
                {
                    StartStatLoadTest();
                    FirstStartStat();
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("Login");
                });

            }
            else
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("Login");
            }
        }
    }


    public bool LoadGame()
    {
        GameData data = null;

        try
        {
            string fileStr = File.ReadAllText(_path);
            byte[] bytes = null;
            bytes = System.Convert.FromBase64String(fileStr);
            string jdata = System.Text.Encoding.UTF8.GetString(bytes);
            data = JsonConvert.DeserializeObject<GameData>(jdata);
            Debug.Log(jdata);

            if (data != null)
            {
                if (data.playerData.level == 0)
                    return false;
                if (data.stageData.maxHp == 0)
                    return false;
                if (string.IsNullOrEmpty(data.inventoryItem.equip))
                    return false;

                return true;
                Debug.Log($"Save Game Loaded : {_path}");
                _gameData = data;
            }

        }
        catch (Exception e)
        {
            errorPopup.SetActive(true);
            errorPopup.GetComponent<UI_SaveFileErrorPopup>().InitData(_path, () =>
            {
                
                FirstStartStat();
                SaveGame("StartScene");
                SceneManager.LoadScene("Login");
            });

            return false;

        }

        return false;
    }

    public void FirstStartStat()
    {
        Debug.Log($"Start Data init");
        StartStat startStat = _startStatDic[1];

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
    }

    public void StartStatLoadTest()
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/StartData");
        StartStatData data = JsonConvert.DeserializeObject<StartStatData>(textAsset.text);
        Debug.Log(data.ToString());


        foreach (StartStat stat in data.startStats)
        {
            _startStatDic.Add(stat.playerData.level, stat);
        }

    }


    public void SaveGame(string requestFunc)
    {

        //string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
        string jsonSt = JsonConvert.SerializeObject(Managers.Game.SaveData);

        //암호화
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonSt);
        string code = System.Convert.ToBase64String(bytes);

        File.WriteAllText(_path, code);
        //Debug.Log($" request : {requestFunc} \n Save Game Completed : {_path}");
    }

}