using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private Sound[] sounds;
    [SerializeField]
    private AudioMixerGroup musicMixer;
    [SerializeField]
    private AudioMixerGroup sfxMixer;

    private void Awake()
    {
        instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.isLoop;
            s.source.volume = s.volume;

            switch (s.type)
            {
                case Sound.AudioTypes.sfx:
                    s.source.outputAudioMixerGroup = sfxMixer;
                    break;

                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musicMixer;
                    break;
            }

            if (s.playOnAwake)
            {
                s.source.Play();
            }
        }
    }

    public void PlayByName(string name)
    {
        Sound soundPlay = Array.Find(sounds, s => s.name == name);

        if (soundPlay != null)
        {
            soundPlay.source.Play();
        }
        else
        {
            Debug.LogError("Sound not found");
            return;
        }
    }

    public void Stop(string name)
    {

        Sound soundPlay = Array.Find(sounds, s => s.name == name);

        if (soundPlay == null)
        {
            Debug.LogError("Sound not found");
            return;
        }
        soundPlay.source.Stop();
    }

    public void UpdateMixerVolume()
    {
        musicMixer.audioMixer.SetFloat("MusicVol", Mathf.Log10(AudioOptions.MusicVol) * 20);
        sfxMixer.audioMixer.SetFloat("sfxVol", Mathf.Log10(AudioOptions.sfxVol) * 20);

    }
}
