using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootManager
{

    public const int MAX_LOOT_ITEMS = 10;

    public GameObject Root;
    public GameObject GoldTarget { get; set; }
    public GameObject ExpTarget { get; set; }

    public Canvas _canvas;
    public CanvasScaler _canvasScaler;


    public int LootCount = 0;
    public List<GameObject> LootList = new List<GameObject>();
    public GameObject EggTransform { get; private set; }


    public const string pathCanvas = "Prefabs/Loot/Canvas/";
    public const string pathImage = "Prefabs/Loot/Canvas/Images/";
    public const string pathSprites = "Prefabs/Loot/Sprites/";
    public const string pathParticle = "Prefabs/Loot/Particle/";


    TargetTrasfrom targetTrasfrom = new TargetTrasfrom();

    public Camera lootCam { get; private set; }
    public Transform InventoryTap { get { return targetTrasfrom.InventoryTap; } private set { InventoryTap = targetTrasfrom.InventoryTap; } }
    public Transform InventoryButton { get { return targetTrasfrom.InventoryButton; } private set { InventoryButton = targetTrasfrom.InventoryButton; } }
    public Transform LevelIcon { get { return targetTrasfrom.LevelIcon; } private set { InventoryButton = targetTrasfrom.LevelIcon; } }
    public Transform GoldIcon { get { return targetTrasfrom.GoldIcon; } private set { InventoryButton = targetTrasfrom.GoldIcon; } }
    public Transform GemIcon { get { return targetTrasfrom.ZemIcon; } private set { InventoryButton = targetTrasfrom.ZemIcon; } }

    public ParticleSystem GoldEvent { get; set; }
    public ParticleSystem ExpEvent { get; set; }

    private const int _particleCount = 4;

    public void Init()
    {


        lootCam = GameObject.Find("LootCamera").GetComponent<Camera>();
        GameObject root = GameObject.Find("@Loot_Item");
        if (root == null)
        {
            root = new GameObject { name = "@Loot_Item" };
            Root = root;
            Root.AddComponent<Canvas>();
            Root.AddComponent<GraphicRaycaster>();
            Root.AddComponent<CanvasScaler>();
            Root.layer = 7;

            _canvas = Root.GetComponent<Canvas>();
            _canvasScaler = Root.GetComponent<CanvasScaler>();

            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.sortingOrder = 3;
            _canvas.worldCamera = lootCam;

            _canvas.planeDistance = 15f;

            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = new Vector2(1080, 1920);


        }

    }

    public void SetTransform()
    {
        targetTrasfrom.StartFindTargets();
    }

    #region Create Gold&Exp

    public void CreateEXP()
    {

        //GameObject go = Managers.Resource.Load<GameObject>(pathSprites + "Exp");
        //GameObject item = Managers.Resource.Instantiate(go);
        //item.transform.SetParent(Root.transform);


        if (ExpEvent == null)
        {
            ExpEvent = GameObject.FindGameObjectWithTag("EPS").GetComponentInChildren<ParticleSystem>();
            ExpEvent.Stop();
            ExpEvent.Play();
        }
        else
        {
            //ParticleSystem.EmissionModule emission = ExpEvent.emission;
            //emission.burstCount = _particleCount ;

            ExpEvent.Stop();
            ExpEvent.Play();
        }

    }

    public void CreateGold()
    {

        if (GoldEvent == null)
        {
            GoldEvent = GameObject.FindGameObjectWithTag("GPS").GetComponentInChildren<ParticleSystem>();
            GoldEvent.Stop();
            GoldEvent.Play();
        }
        else
        {
            //ParticleSystem.EmissionModule emission = GoldEvent.emission;
            //emission.burstCount = _particleCount;

            GoldEvent.Stop();
            GoldEvent.Play();

        }

        {
            //GameObject go = Managers.Resource.Load<GameObject>(pathSprites + "Gold");
            //GameObject item = Managers.Resource.Instantiate(go);
            //item.transform.SetParent(Root.transform);
        }

    }

    public void GetGoldParticle(ParticleSystem ps)
    {
        GoldEvent = ps;
        GoldEvent.Stop();
        //GoldEvent.loop = false;
    }


    public void GetExpParticle(ParticleSystem ps)
    {
        ExpEvent = ps;
        ExpEvent.Stop();
        //ExpEvent.loop = false;
    }

    #endregion


    #region LootItems
    public void DropChest()
    {


        if (LootCount < MAX_LOOT_ITEMS)
        {
            GameObject go = Managers.Resource.Load<GameObject>(pathSprites + "Chest");
            GameObject item = Managers.Resource.Instantiate(go);
            item.transform.SetParent(Root.transform);

            LootCount++;
        }
        else
            return;

    }

    public void DropKey()
    {


        if (LootCount < MAX_LOOT_ITEMS)
        {
            GameObject go = Managers.Resource.Load<GameObject>(pathSprites + "Key");
            GameObject item = Managers.Resource.Instantiate(go);
            item.transform.SetParent(Root.transform);

            LootCount++;
        }
        else
            return;


        //DroppedItemRandomPos(item);

    }

    public void DropIGItem()
    {
        if (LootCount < MAX_LOOT_ITEMS)
        {
            GameObject go = Managers.Resource.Load<GameObject>("Prefabs/Loot/Ingredient/Stone");
            GameObject item = Managers.Resource.Instantiate(go);
            item.AddComponent<SpriteTouch>();
            item.transform.SetParent(Root.transform);

            LootCount++;
        }
        else
            return;


        //DroppedItemRandomPos(item);

    }


    public void PopItem()
    {
        if (LootCount < 0)
            return;

        LootCount--;
    }

    public void OffItemLoot()
    {
        Root.gameObject.SetActive(false);
    }
    public void OnItemLoot()
    {
        Root.gameObject.SetActive(true);
    }

    #endregion

}

class TargetTrasfrom
{
    public Camera lootCam { get; private set; }
    public Transform InventoryTap { get; private set; }
    public Transform InventoryButton { get; private set; }
    public Transform LevelIcon { get; private set; }
    public Transform GoldIcon { get; private set; }
    public Transform ZemIcon { get; private set; }


    public void StartFindTargets()
    {
        InventoryTap = GameObject.Find("InventoryTap").transform;
        InventoryButton = GameObject.Find("InventoryButton").transform;
        LevelIcon = GameObject.Find("LevelIcon").transform;
        GoldIcon = GameObject.Find("GoldIcon").transform;
        ZemIcon = GameObject.Find("ZemIcon").transform;
    }
}
