using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Define;

public class Managers : MonoBehaviour
{

    // �̱���
    public static Managers s_instance = null; // ���ϼ� ����
    public static Managers Instance { get { return s_instance; } } // ������ �Ŵ����� ����´�. ������Ƽ

    // ������ �Ŵ���(�� ���ӿ��� ��� ����)
    private static GameManagerEx s_gameManager = new GameManagerEx();
    private static LootManager s_lootManager = new LootManager();
    private static MarketManager2 s_maketManger2 = new MarketManager2();
    private static EffectManager s_effectManager = new EffectManager();
    private static StageManager s_stageManager = new StageManager();
    private static QuestManager s_questManager = new QuestManager();
    private static ReinforceManager s_reinforceManager = new ReinforceManager();
    private static ChestShopManager s_chestShopManager = new ChestShopManager();

    // �ھ� �Ŵ���(���� ��������)
    private static DataManager s_dataManager = new DataManager();
    private static ResourceManger s_resourceManger = new ResourceManger();
    private static UIManager s_uiManger = new UIManager();
    private static SoundManager s_soundManger = new SoundManager();
    private static SceneMangerEx s_sceneManger = new SceneMangerEx();
    private static PoolMnagner s_poolManger = new PoolMnagner();
    private static AdManager s_adManager = new AdManager();


    // UI ���ӵ����� API DB �� ���� �Ŵ���
    public static DataManager Data { get { Init(); return s_dataManager; } }
    public static ResourceManger Resource { get { Init(); return s_resourceManger; } }
    public static UIManager UI { get { Init(); return s_uiManger; } }
    public static SceneMangerEx Scene { get { Init(); return s_sceneManger; } }
    public static SoundManager Sound { get { Init(); return s_soundManger; } }
    public static PoolMnagner Pool { get { Init(); return s_poolManger; } }
    public static GameManagerEx Game { get { Init(); return s_gameManager; } }
    public static LootManager Loot { get { Init(); return s_lootManager; } }
    //public static MarketManager Market { get { Init(); return s_maketManger; } }
    public static MarketManager2 Market { get { Init(); return s_maketManger2; } }
    public static EffectManager Effect { get { Init(); return s_effectManager; } }
    public static StageManager Stage { get { Init(); return s_stageManager; } }
    public static QuestManager Quest { get { Init(); return s_questManager; } }
    public static ReinforceManager Reinforce { get { Init(); return s_reinforceManager; } }
    public static ChestShopManager Chest { get { Init(); return s_chestShopManager; } }
    public static AdManager AD { get { Init(); return s_adManager; } }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {

        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_dataManager.Init();
            s_gameManager.Init();
            s_soundManger.Init();
            s_poolManger.Init();
            s_lootManager.Init();
            s_maketManger2.Init();
            s_effectManager.Init();
            s_stageManager.Init();
            s_questManager.Init();
            s_reinforceManager.Init();
            s_chestShopManager.Init();
            s_adManager.Init();

        }

        // UI ���ӵ����� API DB �� ���� �Ŵ���.init()

    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }

   

}
