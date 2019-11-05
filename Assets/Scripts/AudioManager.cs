using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public GameObject audioSources;

    public AudioClip[] popSounds;
    public AudioClip[] loopSounds;
    List<AudioSource> sources = new List<AudioSource>();
    List<Fade> fades = new List<Fade>();
    List<float> volumes = new List<float>();
    List<float> maxVolumes = new List<float>();
    float gameVolume = 1.0f;
    Dictionary<SoundsType, AudioClip[]> audioClips = new Dictionary<SoundsType, AudioClip[]>();

    public enum SoundsType
    {
        LOOPING = 0,
        POP_UP = 1,
    }
    public enum PopUpSounds
    {
        ONE = 0,
    }

    public enum Fade
    {
        NO_FADE = 0,
        NOT_YET = 1,
        IN = 2,
        OUT = 3
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        AudioSource[] newSources;
        newSources = audioSources.GetComponents<AudioSource>();
        for (int i = 0; i < loopSounds.Length; ++i)
        {
            if (i >= newSources.Length)
            {
                audioSources.AddComponent<AudioSource>();
                newSources = audioSources.GetComponents<AudioSource>();
            }

            sources.Add(newSources[i]);
            fades.Add(Fade.NOT_YET);
            volumes.Add(0.0f);
            maxVolumes.Add(1.0f);
            sources[i].clip = loopSounds[i];
            if (!sources[i].loop)
            {
                sources[i].loop = true;
            }
        }

        audioClips.Add(SoundsType.POP_UP, popSounds);
        audioClips.Add(SoundsType.LOOPING, loopSounds);
    }

    void Start()
    {
        FadePlay(0);
        SetSoundMaxVolume(0, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        //if(!sources[0].isPlaying)
        //{
        //    Play(SoundsType.LOOPING, 0, 0.1f);
        //}

        for(int i = 0; i < sources.Count; ++i)
        {
            if (!sources[i].isPlaying || fades[i] == Fade.NO_FADE)
            {
                continue;
            }

            if(fades[i] == Fade.IN)
            {
                if (volumes[i] < maxVolumes[i])
                    volumes[i] += Time.deltaTime * 0.1f;
                else if (volumes[i] >= maxVolumes[i])
                    fades[i] = Fade.NOT_YET;

                sources[i].volume = volumes[i];
            }
            else if (fades[i] == Fade.OUT)
            {
                if (volumes[i] > 0.0f)
                    volumes[i] -= Time.deltaTime * 0.1f;
                else if (volumes[i] <= 0.0f)
                    fades[i] = Fade.NOT_YET;

                sources[i].volume = volumes[i] * gameVolume;
            }

            if(sources[i].time >= sources[i].clip.length * 0.9f)
            {
                fades[i] = Fade.OUT;
            }
            else if (sources[i].time <= sources[i].clip.length * 0.1f)
            {
                fades[i] = Fade.IN;
            }
        }

        Debug.Log(volumes[0]);
    }

    public void Play(SoundsType type, int sound)
    {
        if (type != SoundsType.LOOPING)
        {
            AudioSource newSource = GetComponent<AudioSource>();
            newSource.PlayOneShot(GetSound(type, sound), 1.0f * gameVolume);
        }
        else
        {
            //GetComponent<AudioSource>().clip = GetSound(type, sound);
            fades[sound] = Fade.NO_FADE;
            sources[sound].Play();
        }
    }

    public void Play(SoundsType type, int sound, float volume)
    {
        if (type != SoundsType.LOOPING)
        {
            AudioSource newSource = GetComponent<AudioSource>();
            newSource.PlayOneShot(GetSound(type, sound), volume * gameVolume);
        }
        else
        {
            sources[sound].volume = volume * gameVolume;
            fades[sound] = Fade.NO_FADE;
            sources[sound].Play();
        }
    }

    public void FadePlay(int sound)
    {
        fades[sound] = Fade.IN;
        sources[sound].Play();
        volumes[sound] = 0.0f;
    }

    public void FadeStop(int sound)
    {
        fades[sound] = Fade.OUT;
    }

    public void SetSoundMaxVolume(int sound, float volume)
    {
        maxVolumes[sound] = volume;
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
            sources[sound].volume = 1.0f * gameVolume;
            sources[sound].Play();
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
            sources[sound].volume = volume * gameVolume;
            sources[sound].Play();
        }
    }

    void Stop(SoundsType type, int sound)
    {
        sources[sound].Stop();
    }

    void Pause(SoundsType type, int sound)
    {
        sources[sound].Pause();
    }

    void Resume(SoundsType type, int sound)
    {
        sources[sound].UnPause();
    }

    void Volume(SoundsType type, int sound, float _volume)
    {
        sources[sound].volume = _volume * gameVolume;
    }

    float Volume(SoundsType type, int sound)
    {
        return sources[sound].volume;
    }

    ref AudioClip GetSound(SoundsType type, int sound)
    {
        switch (type)
        {
            case SoundsType.POP_UP:
                if (sound >= popSounds.Length)
                    sound = popSounds.Length - 1;

                    return ref popSounds[sound];
        }

        return ref popSounds[0];
    }
}
