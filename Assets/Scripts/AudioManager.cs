using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public GameObject audioSources;
    List<LoopSound> loopSounds = new List<LoopSound>();
    public AudioClip[] taskSounds;
    public AudioClip[] miscSounds;
    public AudioClip[] loopedSounds;
    float gameVolume = 1.0f;
    Dictionary<SoundsType, AudioClip[]> audioClips = new Dictionary<SoundsType, AudioClip[]>();

    class LoopSound
    {
        public AudioSource source;
        public bool fade;
        public float maxVolume;
    }

    public enum SoundsType
    {
        LOOPING = 0,
        TASK = 1,
        MISC = 2
    }
    public enum TaskSounds
    {
        CREATED = 0,
        ACCEPTED = 1,
        REJECT = 2,
        COMPLETED = 3
    }

    public enum MiscSounds
    {
        CELEBRATION = 0,
    }

    public enum LoopSounds
    {
        MUSIC = 0,
        CHATTER = 1,
        FOOTSTEPS = 3,
        TYPING = 2
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        AudioSource[] newSources;
        newSources = audioSources.GetComponents<AudioSource>();

        audioClips.Add(SoundsType.LOOPING, loopedSounds);
        audioClips.Add(SoundsType.TASK, taskSounds);
        audioClips.Add(SoundsType.MISC, miscSounds);

        for (int i = 0; i < loopedSounds.Length; ++i)
        {
            if (i >= newSources.Length)
            {
                audioSources.AddComponent<AudioSource>();
                newSources = audioSources.GetComponents<AudioSource>();
            }

            LoopSound sound = new LoopSound();
            sound.source = newSources[i];
            sound.source.clip = loopedSounds[i];
            sound.source.loop = true;
            sound.source.playOnAwake = false;
            sound.fade = false;
            sound.maxVolume = 1.0f;
            loopSounds.Add(sound);
        }
    }

    void Start()
    {
        FadeIn((int)LoopSounds.MUSIC, 0.1f, 0.2f);
        Play(SoundsType.LOOPING, (int)LoopSounds.MUSIC, 0.0f);
        //Play(SoundsType.LOOPING, (int)LoopSounds.FOOTSTEPS, 0.2f);
        //SetFade((int)LoopSounds.FOOTSTEPS, false);
        //Pause((int)LoopSounds.FOOTSTEPS);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < loopSounds.Count; ++i)
        {
            if (!loopSounds[i].source.isPlaying)
            {
                continue;
            }
            else if(!loopSounds[i].fade && loopSounds[i].source.time >= loopSounds[i].source.clip.length)
            {
                Stop(i);
                continue;
            }

            //if (loopSounds[i].source.time >= loopSounds[i].source.clip.length * 0.9f)
            //{
            //    StartCoroutine(fadeOut(i, 0.5f));
            //}
            //else if (loopSounds[i].source.time <= loopSounds[i].source.clip.length * 0.1f)
            //{
            //    StartCoroutine(fadeIn(i, 0.1f));
            //}
        }
    }

    //public void Play(SoundsType type, int sound)
    //{
    //    if (type != SoundsType.LOOPING)
    //    {
    //        AudioSource newSource = GetComponent<AudioSource>();
    //        newSource.PlayOneShot(GetSound(type, sound), 1.0f * gameVolume);
    //    }
    //    else
    //    {
    //        Debug.Log(type + " :: " + sound);
    //        loopSounds[sound].source.volume = 1.0f * gameVolume;
    //        loopSounds[sound].fade = false;
    //        loopSounds[sound].source.Play();
    //    }
    //}

    public void Play(SoundsType type, int sound, float volume)
    {
        if (type != SoundsType.LOOPING)
        {
            AudioSource newSource = GetComponent<AudioSource>();
            newSource.PlayOneShot(GetSound(type, sound), volume * gameVolume);
        }
        else
        {
            Debug.Log(type + " :: " + sound);
            loopSounds[sound].source.volume = volume * gameVolume;
            loopSounds[sound].fade = false;
            loopSounds[sound].source.Play();
        }
    }

    public void FadeIn(int sound, float fadeSpeed, float maxVolume)
    {
        loopSounds[sound].source.volume = 0.0f;
        loopSounds[sound].maxVolume = maxVolume;
        loopSounds[sound].fade = true;
        StartCoroutine(fadeIn(sound, fadeSpeed));
    }

    public void FadeOut(int sound, float fadeSpeed)
    {
        StartCoroutine(fadeOut(sound, fadeSpeed));
    }

    public void SetFade(int sound, bool fade)
    {
        loopSounds[sound].fade = fade;
    }

    public void SetSoundMaxVolume(int sound, float volume)
    {
        loopSounds[sound].maxVolume = volume;
    }

    public void PlayRandom(SoundsType type)
    {
        if (type != SoundsType.LOOPING)
        {
            AudioSource newSource = GetComponent<AudioSource>();
            int sound = Random.Range(0, audioClips[type].Length);
            newSource.PlayOneShot(GetSound(type, sound), 1.0f * gameVolume);
        }
        else
        {
            int sound = Random.Range(0, audioClips[type].Length);
            loopSounds[sound].source.volume = 1.0f * gameVolume;
            loopSounds[sound].fade = false;
            loopSounds[sound].source.Play();
        }
    }

    public void PlayRandom(SoundsType type, float volume)
    {
        if (type != SoundsType.LOOPING)
        {
            AudioSource newSource = GetComponent<AudioSource>();
            int sound = Random.Range(0, audioClips[type].Length);
            newSource.PlayOneShot(GetSound(type, sound), volume * gameVolume);
        }
        else
        {
            int sound = Random.Range(0, audioClips[type].Length);
            loopSounds[sound].source.volume = volume * gameVolume;
            loopSounds[sound].fade = false;
            loopSounds[sound].source.Play();
        }
    }

    public void Stop(int sound)
    {
        loopSounds[sound].source.Stop();
    }

    public void Pause(int sound)
    {
        loopSounds[sound].source.Pause();
    }

    public void Resume(int sound)
    {
        loopSounds[sound].source.UnPause();
    }

    public void Volume(int sound, float _volume)
    {
        loopSounds[sound].source.volume = _volume * gameVolume;
    }

    public float Volume(int sound)
    {
        return loopSounds[sound].source.volume;
    }

    ref AudioClip GetSound(SoundsType type, int sound)
    {
        switch (type)
        {
            case SoundsType.TASK:
                    return ref taskSounds[sound];
            case SoundsType.MISC:
                return ref miscSounds[sound];
        }

        return ref taskSounds[0];
    }

    public bool IsSoundPlaying(int sound)
    {
        return loopSounds[sound].source.isPlaying;
    }

    IEnumerator fadeIn(int sound, float fadeSpeed)
    {
        while(loopSounds[sound].source.volume < loopSounds[sound].maxVolume)
        {
            loopSounds[sound].source.volume += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        loopSounds[sound].source.volume = loopSounds[sound].maxVolume;
        yield return 0;
    }

    IEnumerator fadeOut(int sound, float fadeSpeed)
    {
        while (loopSounds[sound].source.volume > 0.0f)
        {
            loopSounds[sound].source.volume -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        loopSounds[sound].source.volume = 0.0f;
        loopSounds[sound].source.Stop();
        yield return 0;
    }
}
