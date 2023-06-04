using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectDurationDestory : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isEnd;
    void Start()
    {
        init();
        StartCoroutine(c_CheckEggDie());
    }

    void init()
    {
        isEnd = false;
        ParticleSystem ps = GetComponent<ParticleSystem>();
        float duration = ps.main.duration;
        //Destroy(this.gameObject, duration);
        StartCoroutine(c_Destory(duration));

    }

    IEnumerator c_Destory(float time)
    {
        yield return new WaitForSeconds(time);
        Managers.Effect.PopEffect();
        Managers.Resource.Destroy(gameObject);
        isEnd = true;
    }

    IEnumerator c_CheckEggDie()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            if (Managers.Game.EggDieAlarm == true)
            {
                Managers.Resource.Destroy(gameObject);
                Managers.Effect.PopEffect();
            }
                
        }

        
    }

}
