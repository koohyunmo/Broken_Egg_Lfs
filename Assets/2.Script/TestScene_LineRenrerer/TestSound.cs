using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    [SerializeField]
    string clipPath1 = "TestSound/DM-CGS-06";
    [SerializeField]
    AudioClip clipPath2;

    private void Start()
    {
        SoundTest();
    }

    private void SoundTest()
    {
        
        //AudioSource audio = GetComponent<AudioSource>();
        /*
        audio.PlayOneShot(clip1);
        audio.PlayOneShot(clip2);
        float lifeTime = Mathf.Max(clip1.length, clip2.length);
        GameObject.Destroy(gameObject, lifeTime);
        */

        Managers.Sound.Play( clipPath1, Define.Sound.Effect);
        Managers.Sound.Play(clipPath2, Define.Sound.Bgm);
    }

}
