using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager
{
    public int ClearCount = 5;
    public int BoosStage = 5;

    public GameObject TimerSlider;


    public Sprite[] Maps { get; private set; }
    public GameObject[] aura { get; private set; } = new GameObject[8];
    public Sprite bossSprite { get; private set; }
    private Image mapImage;
    private AudioClip[] audioClips = new AudioClip[8];

    int soundindex = -1;
    public void Init()
    {

        Sprite[] maps = Resources.LoadAll<Sprite>("Maps/");
        aura = Resources.LoadAll<GameObject>("Aura/");
        bossSprite = Resources.Load<Sprite>("Images/BossImage/bossIcon");
        Maps = maps;

        audioClips[0] = Managers.Resource.Load<AudioClip>("Sounds/BGM/First_Steps");
        audioClips[1] = Managers.Resource.Load<AudioClip>("Sounds/BGM/First_Steps");
        audioClips[2] = Managers.Resource.Load<AudioClip>("Sounds/BGM/Forest LOOP");
        audioClips[3] = Managers.Resource.Load<AudioClip>("Sounds/BGM/Jungle 2 LOOP");
        audioClips[4] = Managers.Resource.Load<AudioClip>("Sounds/BGM/Pretty Dungeon LOOP");
        audioClips[5] = Managers.Resource.Load<AudioClip>("Sounds/BGM/Murky Dungeon LOOP");
        audioClips[6] = Managers.Resource.Load<AudioClip>("Sounds/BGM/Dark Cave");
        audioClips[7] = Managers.Resource.Load<AudioClip>("Sounds/BGM/Jungle 2 LOOP");

        aura[0] = Managers.Resource.Load<GameObject>("Aura/0_bossAura");
        aura[1] = Managers.Resource.Load<GameObject>("Aura/1_Ki aura rare");
        aura[2] = Managers.Resource.Load<GameObject>("Aura/3_Ki aura grand");
        aura[3] = Managers.Resource.Load<GameObject>("Aura/4_Ki aura basic");
        aura[4] = Managers.Resource.Load<GameObject>("Aura/5_Ki aura heroic");
        aura[5] = Managers.Resource.Load<GameObject>("Aura/6_Ki aura arcane");
        aura[6] = Managers.Resource.Load<GameObject>("Aura/7_Ki aura divine");
        aura[7] = Managers.Resource.Load<GameObject>("Aura/8_Ki aura epic");


        if (audioClips[0] == null)
        {
            Debug.Log("null sound");
        }
    }

    private float CalShield()
    {
        float shield = 0;

        if (Managers.Game.StageData.currentStage % 5 == 0)
        {
            shield = Managers.Game.StageData.currentStage * 10;
        }
        else
        {
            shield = Managers.Game.StageData.currentStage * 5;
        }

        if (Managers.Game.StageData.currentStage / 80 >= 1)
        {
            shield += 100;
        }
        if (Managers.Game.StageData.currentStage / 160 >= 1)
        {
            shield += 500;
        }

        if (Managers.Game.StageData.currentStage / 320 >= 1)
        {
            shield += 1000;
        }


        return shield;
    }

    public long GetDPS()
    {
        long d = 0 ;

        foreach (var item in Managers.Game.InventoryData.item.Values)
        {
            d += (long)(item.itemDamage * ((int)item.itemGrade * 0.1f));
            if (d <= 0)
                d = 1;
        }

        float _monsterShield = CalShield();

        if (Managers.Game.EquipItemData.shieldAttack < 0)
            Managers.Game.EquipItemData.shieldAttack = int.MaxValue;

        int sA = Managers.Game.EquipItemData.shieldAttack;

        float shield = (_monsterShield) / (100 + _monsterShield + sA);
        long dmg = 0;

        /*
        데미지 감소율 = (몬스터 방어력) / (100 + 몬스터 방어력)
        플레이어가 몬스터에게 가할 수 있는 실제 데미지 = (플레이어 공격력) x (1 - 데미지 감소율)
         */


        dmg = (long)((d) * (1 - shield));


        if (dmg >= long.MaxValue || dmg == long.MaxValue * -1)
        {
            dmg = long.MaxValue;
        }

        if (dmg < 0)
            dmg *= -1;


        if (dmg == 0)
            dmg = 1;

        return dmg;

    }

    public long GetCriDMG()
    {

        long d = (long)(Managers.Game.TotalDamage * Managers.Game.EquipItemData.itemCriticalPlusDamage  );;

        float _monsterShield = CalShield();

        if (Managers.Game.EquipItemData.shieldAttack < 0)
            Managers.Game.EquipItemData.shieldAttack = int.MaxValue;

        int sA = Managers.Game.EquipItemData.shieldAttack;

        float shield = (_monsterShield) / (100 + _monsterShield + sA);
        long dmg = 0;

        dmg = (long)((d) * (1 - shield));


        if (dmg >= long.MaxValue || dmg == long.MaxValue * -1)
        {
            dmg = long.MaxValue;
        }

        if (dmg < 0)
            dmg *= -1;

        if (dmg == 1 || dmg == 0)
            dmg = 2;

        return dmg;
    }

    public long GetCDmage()
    {
        long d = Managers.Game.TotalDamage;

        float _monsterShield = CalShield();

        if (Managers.Game.EquipItemData.shieldAttack < 0)
            Managers.Game.EquipItemData.shieldAttack = int.MaxValue;

        int sA = Managers.Game.EquipItemData.shieldAttack;

        float shield = (_monsterShield) / (100 + _monsterShield + sA);
        long dmg = 0;

        /*
        데미지 감소율 = (몬스터 방어력) / (100 + 몬스터 방어력)
        플레이어가 몬스터에게 가할 수 있는 실제 데미지 = (플레이어 공격력) x (1 - 데미지 감소율)
         */


        dmg = (long)((d) * (1 - shield));


        if (dmg >= long.MaxValue || dmg == long.MaxValue * -1)
        {
            dmg = long.MaxValue;
        }

        if (dmg < 0)
            dmg *= -1;


        if (dmg == 0)
            dmg = 1;

        return dmg;

    }

    public void GetTimerSlider(GameObject go)
    {
        TimerSlider = go;
    }

    public void SetImage(Image image)
    {
        mapImage = image;
    }

    public void StageMapCheck()
    {
        int currentStage = (Managers.Game.StageData.currentStage % 40);


        int index = (int)(currentStage / 5f);


        index = Mathf.Clamp(index, 0, Maps.Length - 1);

        if (index != soundindex)
        {
            soundindex = index;
            Managers.Sound.Play(audioClips[soundindex], Define.Sound.Bgm);
        }
            


        mapImage.sprite = Maps[index];
        
    }

    public void PrevStageMapCheck()
    {
        int currentStage = ((Managers.Game.StageData.currentStage-2) % 40);


        int index = (int)(currentStage / 5f);

        index = Mathf.Clamp(index, 0, Maps.Length - 1);

        if (index != soundindex)
        {
            soundindex = index;
            Managers.Sound.Play(audioClips[soundindex], Define.Sound.Bgm);
        }



        mapImage.sprite = Maps[index];

    }

    

    public GameObject GetAura()
    {
        int currentStage = ((Managers.Game.StageData.currentStage));

        int index = (int)(currentStage / 20f);

        if (index <= 0)
        {
                return null;
        }
        else
        {
            index = Mathf.Clamp(index, 2, aura.Length - 1);
            
        }

        return aura[index];

    }

    public GameObject GetBossAura()
    {
        if (Managers.Game.StageData.currentStage % 5 == 0)
        {
            return aura[0];
            //boss
        }
        else
        {
            int currentStage = ((Managers.Game.StageData.currentStage));

            if (currentStage / 40 < 0)
            {
                return null;
            }


            int index = (int)(currentStage / 40f);
            index = Mathf.Clamp(index, 1, aura.Length - 1);
            return aura[index];

        }
            

    }

    public int StageReward()
    {
        int current = Managers.Game.StageData.currentStage;

        int reward = (int)(Mathf.Pow(1.3f, current) * 2);

        return reward;
    }

}
