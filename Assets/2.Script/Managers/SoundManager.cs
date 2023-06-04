using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    // 플레이어(AudioSource) - 음원(Audio Clip) - 귀(AudioListener)

    public AudioSource[] AudioSource { get; private set; } = new AudioSource[(int)Define.Sound.MaxCount];

    /// <summary>
    /// 캐싱역활
    /// </summary>
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();


    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");

        if(root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));

            for(int i = 0; i < soundNames.Length -1; i++)
            {
               GameObject go = new GameObject { name = soundNames[i] };
               AudioSource[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            AudioSource[(int)Define.Sound.Bgm].loop = true;

        }
    }

    /// <summary>
    /// 부화방지, 생명주기관리
    /// </summary>
    public void Clear()
    {
        foreach(AudioSource audioSource in AudioSource)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }
    /// <summary>
    /// 주소를 받는 버전
    /// </summary>
    /// <param name="path">주소</param>
    /// <param name="type">BGM or Effect</param>
    /// <param name="pitch">피치</param>
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path,type);
        Play(audioClip, type, pitch);
    }

    /// <summary>
    /// 플레이중
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying()
    {
        AudioSource audioSource = AudioSource[(int)Define.Sound.Effect];
        
        return audioSource.isPlaying;

    }

    public void IsStop()
    {
        AudioSource audioSource = AudioSource[(int)Define.Sound.Effect];

        audioSource.Stop();

    }

    public void SetBGMVolume(float volume)
    {
        AudioSource[(int)Define.Sound.Bgm].volume = volume;
    }

    public float GetBGMVolume()
    {
        return AudioSource[(int)Define.Sound.Bgm].volume;
    }

    public void SetEFVolume(float volume)
    {
        AudioSource[(int)Define.Sound.Effect].volume = volume;
    }

    public void SetCHVolume(float volume)
    {
        AudioSource[(int)Define.Sound.Chest].volume = volume;
    }

    public float GetEFVolume()
    {
        return AudioSource[(int)Define.Sound.Effect].volume;
    }

    /// <summary>
    /// AudioClip 받는 버전
    /// </summary>
    /// <param name="audioClip">오디오클립</param>
    /// <param name="type">BGM or Effect</param>
    /// <param name="pitch">피치</param>
    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;


        if (type == Define.Sound.Bgm)
        {


            AudioSource audioSource = AudioSource[(int)Define.Sound.Bgm];

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();

        }
        else if(type == Define.Sound.Chest)
        {
            AudioSource audioSource = AudioSource[(int)Define.Sound.Chest];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            AudioSource audioSource = AudioSource[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }




    }/// <summary>
    /// Sounds 폴더에서 오디오클립 찾기
    /// </summary>
    /// <param name="path">주소</param>
    /// <param name="type">BGM or Effect</param>
    /// <returns></returns>

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
        {
            path = $"Sounds/{path}";
        }

        AudioClip audioClip = null;

        // BGm 일때
        if (type == Define.Sound.Bgm)
        {
             audioClip = Managers.Resource.Load<AudioClip>(path);

        }
        // 단발성 Effect Sound 일때
        // 자주 실행되는 부분 캐싱이 필요함
        // 한번 사용했던 클립이 있으면 그 클립을 재사용
        else
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }

        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing !{path}");


        return audioClip;
    }

}
