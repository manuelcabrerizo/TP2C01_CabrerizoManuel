using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public enum AudioSourceType
{
    SFX,
    UI
}

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    [SerializeField] public SoundData soundData;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource sfxAudioSourcePrefab;
    [SerializeField] private AudioSource uiAudioSourcePrefab;
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private AudioSource musicAudioSource;
    private IObjectPool<AudioSource> sfxPool;
    private IObjectPool<AudioSource> uiPool;

    protected override void OnAwaken()
    {
        musicAudioSource = GetComponent<AudioSource>();

        sfxPool = new ObjectPool<AudioSource>(
            CreateSfxAudioSource, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxSize);

        uiPool = new ObjectPool<AudioSource>(
            CreateUiAudioSource, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxSize);
    }

    protected override void OnDestroyed()
    {
        StopAllCoroutines();
    }

    public void Mute()
    {
        audioMixer.SetFloat("MasterVolume", -80f);
    }

    public void Unmute()
    {
        audioMixer.SetFloat("MasterVolume", 0f);
    }

    public void PlayMusic()
    {
        musicAudioSource.Play();
    }

    public void PauseMusic()
    {
        musicAudioSource.Pause();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }


    public void PlayClip(AudioClip clip, AudioSourceType type)
    {
        switch (type)
        {
            case AudioSourceType.SFX:
                {
                    AudioSource sfxAudioSource = sfxPool.Get();
                    sfxAudioSource.clip = clip;
                    sfxAudioSource.Play();
                    StartCoroutine(ReleaseSfxAudioSourceIfFinish(sfxAudioSource));
                }
                break;
            case AudioSourceType.UI:
                {
                    AudioSource uiAudioSource = uiPool.Get();
                    uiAudioSource.clip = clip;
                    uiAudioSource.Play();
                    StartCoroutine(ReleaseUiAudioSourceIfFinish(uiAudioSource));
                }
                break;
        }
    }

    private IEnumerator ReleaseSfxAudioSourceIfFinish(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        sfxPool.Release(audioSource);
    }

    private IEnumerator ReleaseUiAudioSourceIfFinish(AudioSource audioSource)
    {
        yield return new WaitForSecondsRealtime(audioSource.clip.length);
        uiPool.Release(audioSource);
    }


    private AudioSource CreateSfxAudioSource()
    {
        AudioSource sfxAudioSource = Instantiate(sfxAudioSourcePrefab, transform);
        return sfxAudioSource;
    }

    private AudioSource CreateUiAudioSource()
    {
        AudioSource uiAudioSource = Instantiate(uiAudioSourcePrefab, transform);
        return uiAudioSource;
    }

    private void OnReleaseToPool(AudioSource pooledObject)
    {
        pooledObject.enabled = false;
    }

    private void OnGetFromPool(AudioSource pooledObject)
    {
        pooledObject.enabled = true;
        pooledObject.Stop();
    }

    private void OnDestroyPooledObject(AudioSource pooledObject)
    {
        Destroy(pooledObject);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Utils.LinearToDecibel(volume));
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Utils.LinearToDecibel(volume));
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("SfxVolume", Utils.LinearToDecibel(volume));
    }

    public void SetUiVolume(float volume)
    {
        audioMixer.SetFloat("UiVolume", Utils.LinearToDecibel(volume));
    }
}
