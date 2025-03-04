using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundClip
{
    BGM,
    SemiBossBGM,
    BossBGM, 

    ClearButtonSFX,
    LightSliceSFX,
    PotionSFX,
    SpecialAttackSFX,
    AttackSFX,
    StatUIButtonSFX,
    StartButtonSFX,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<AudioClip> clips;

    public AudioSource bgmSound;
    public AudioSource sfxSound;

    public Transform sfxParent;

    public Queue<AudioSource> pool;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        pool = new Queue<AudioSource>();

        PlayBGM(SoundClip.BGM);
    }

    // SFX ==================================================================================
    public void PlaySFX(SoundClip clip, float volume)
    {
        AudioSource audioSource;

        if (pool.Count == 0)
        {
            audioSource = Instantiate(sfxSound, sfxParent);
        }
        else
        {
            audioSource = pool.Dequeue();
            audioSource.transform.parent = sfxParent;
        }

        audioSource.clip = clips[(int)clip];
        audioSource.volume = volume;
        audioSource.Play();

        StartCoroutine(SoundStop(audioSource));
    }

    IEnumerator SoundStop(AudioSource audioSource)
    {
        while (audioSource.isPlaying)
            yield return null;

        pool.Enqueue(audioSource);
    }

    // BGM ==================================================================================

    public void PlayBGM(SoundClip clip, float volume = 1f, bool isLoop = true)
    {
        if (bgmSound.clip == clips[(int)clip])
            return;

        bgmSound.clip = clips[(int)clip];
        bgmSound.loop = isLoop;

        bgmSound.Play();
    }

    public void BGMSound(bool isOn, float volume)
    {
        if (isOn)
            bgmSound.volume = volume;
        else
            bgmSound.volume = 0;
    }
}
