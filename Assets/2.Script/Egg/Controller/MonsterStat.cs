using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    public long HP { get { return Managers.Game.StageData.currentHp; }}
    public long MaxHP { get { return Managers.Game.StageData.maxHp; }}

    public delegate void DeathEvent();
    public event DeathEvent Died;

    public delegate void DamageTextEvent(long a, bool b);
    public event DamageTextEvent MakeText;
    /*
    데미지 감소율 = (몬스터 방어력) / (100 + 몬스터 방어력)
    플레이어가 몬스터에게 가할 수 있는 실제 데미지 = (플레이어 공격력) x (1 - 데미지 감소율)
     */

    float _monsterShield = 1000;
    public void OnDamaged(long playerDamage, bool isCritical = false)
    {
        
        _monsterShield = CalShield();
        playerDamage = CalDmg(playerDamage);

        if (playerDamage == 0)
            playerDamage = 1;


        Managers.Game.StageData.currentHp -= (playerDamage);

        if (MakeText != null && isCritical == false)
        {
            MakeText(playerDamage,isCritical);
        }
        else if(MakeText != null && isCritical == true)
        {
            MakeText(playerDamage, isCritical);
            //Debug.Log("치명타");
        }

        if (Managers.Game.StageData.currentHp <= 0)
        {
            if (Died != null)
            {
                Managers.Sound.Play("VOICE_Girl_2yo_Growl_01_mono");
                Died?.Invoke();
            }
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

    private long CalDmg(long pDmg)
    {


        if (Managers.Game.EquipItemData.shieldAttack < 0)
            Managers.Game.EquipItemData.shieldAttack = int.MaxValue;

        int sA = Managers.Game.EquipItemData.shieldAttack;

        float shield = (_monsterShield ) /(100 +_monsterShield + sA);
        long dmg = 0;

        /*
        데미지 감소율 = (몬스터 방어력) / (100 + 몬스터 방어력)
        플레이어가 몬스터에게 가할 수 있는 실제 데미지 = (플레이어 공격력) x (1 - 데미지 감소율)
         */


        dmg = (long)((pDmg) * (1 - shield));


        if (dmg >= long.MaxValue || dmg == long.MaxValue*-1)
        {
            dmg = long.MaxValue;
        }

        if (dmg < 0)
            dmg *= -1;

        
       
        return dmg;
    }

}
