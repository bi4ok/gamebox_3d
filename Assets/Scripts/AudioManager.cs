using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] music;
    public Sound[] dialogs;
    int i = 0;
    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach(Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach(Sound  s in dialogs)
        {
            print(s.name);
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

    }
    

    public void PlaySounds(string name)
    {
      Sound s =   Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Play();
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(music, sound => sound.name == name);
        print(s);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Play();
    }
    public void StopMusic(string name)
    {
        Sound s = Array.Find(music, sound => sound.name == name);
        print(s);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Stop();
    }
    public void PlayDilogs()
    {
        if(dialogs.Length > i)
        {
            print(dialogs.Length + " " + i + " " + dialogs[i]);
            Sound s = dialogs[i];
            print(s.source);
            s.source.Play();
            i++;
        }
    }
}
