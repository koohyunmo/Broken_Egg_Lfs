using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    // �÷��̾�(AudioSource) - ����(Audio Clip) - ��(AudioListener)

    public AudioSource[] AudioSource { get; private set; } = new AudioSource[(int)Define.Sound.MaxCount];

    /// <summary>
    /// ĳ�̿�Ȱ
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
    /// ��ȭ����, �����ֱ����
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
    /// �ּҸ� �޴� ����
    /// </summary>
    /// <param name="path">�ּ�</param>
    /// <param name="type">BGM or Effect</param>
    /// <param name="pitch">��ġ</param>
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path,type);
        Play(audioClip, type, pitch);
    }

    /// <summary>
    /// �÷�����
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
    /// AudioClip �޴� ����
    /// </summary>
    /// <param name="audioClip">�����Ŭ��</param>
    /// <param name="type">BGM or Effect</param>
    /// <param name="pitch">��ġ</param>
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
    /// Sounds �������� �����Ŭ�� ã��
    /// </summary>
    /// <param name="path">�ּ�</param>
    /// <param name="type">BGM or Effect</param>
    /// <returns></returns>

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
        {
            path = $"Sounds/{path}";
        }

        AudioClip audioClip = null;

        // BGm �϶�
        if (type == Define.Sound.Bgm)
        {
             audioClip = Managers.Resource.Load<AudioClip>(path);

        }
        // �ܹ߼� Effect Sound �϶�
        // ���� ����Ǵ� �κ� ĳ���� �ʿ���
        // �ѹ� ����ߴ� Ŭ���� ������ �� Ŭ���� ����
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
