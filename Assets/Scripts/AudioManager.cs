using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] music;
    public Sound[] dialogs;
    int i = 0;
    private Sound s_sound;
    private Sound s_music;
    private Sound s_dilogs;
    [SerializeField]
    private Slider masterVolumeSlider_music;
    [SerializeField]
    private Slider masterVolumeSlider_sounds;
    [SerializeField]
    private Slider masterVolumeSlider_dilogs;


    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = PlayerPrefs.GetFloat("MasterVolume_Sounds");
            masterVolumeSlider_music.value = s.source.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
           
        }
        foreach(Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = PlayerPrefs.GetFloat("MasterVolume_Music");
            masterVolumeSlider_music.value = s.source.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach(Sound  s in dialogs)
        {
          
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = PlayerPrefs.GetFloat("MasterVolume_Dilogs"); ;
            masterVolumeSlider_dilogs.value = s.source.volume;
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
    public void UpdateVolume()
    {
        PlayerPrefs.SetFloat("MasterVolume_Music", masterVolumeSlider_music.value);
        PlayerPrefs.SetFloat("MasterVolume_Sounds", masterVolumeSlider_sounds.value);
        PlayerPrefs.SetFloat("MasterVolume_Dilogs", masterVolumeSlider_dilogs.value);
        PlayerPrefs.Save();
        foreach (Sound s in sounds)
        {
            s.source.volume = PlayerPrefs.GetFloat("MasterVolume_Sounds");
        }
        foreach (Sound s in music)
        {
            s.source.volume = PlayerPrefs.GetFloat("MasterVolume_Music");
        }
        foreach (Sound s in dialogs)
        {
           
            s.source.volume = PlayerPrefs.GetFloat("MasterVolume_Dilogs"); 
        }
    }
}
