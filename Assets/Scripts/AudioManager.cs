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

    private bool sourceAdded=false;


    private void Awake()
    {
        print("янгдюмхе бяеу SOURCE");
        print(masterVolumeSlider_dilogs + " " + masterVolumeSlider_dilogs.value);
        float x = PlayerPrefs.GetFloat("MasterVolume_Dilogs", 0.999f);
        print(x + " dilogs PREFS");
        print(PlayerPrefs.GetFloat("MasterVolume_Sounds", 0.999f) + " sounds prefs");
        print(PlayerPrefs.GetFloat("MasterVolume_Music", 0.999f) + " music prefs");
        masterVolumeSlider_dilogs.value = x;
        masterVolumeSlider_sounds.value = PlayerPrefs.GetFloat("MasterVolume_Sounds", 0.999f);
        masterVolumeSlider_music.value = PlayerPrefs.GetFloat("MasterVolume_Music", 0.999f);

        foreach (Sound s in sounds)
        {
            
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            //s.source.volume = PlayerPrefs.GetFloat("MasterVolume_Sounds", 1);
            //masterVolumeSlider_sounds.value = s.source.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
           
        }
        foreach(Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            //s.source.volume = PlayerPrefs.GetFloat("MasterVolume_Music", 1);
            //masterVolumeSlider_music.value = s.source.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach(Sound  s in dialogs)
        {
          
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            //s.source.volume = PlayerPrefs.GetFloat("MasterVolume_Dilogs", 1); ;
            //masterVolumeSlider_dilogs.value = s.source.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        print("янгдюмхе бяеу SOURCE опнькн сяоеьмн");
        sourceAdded = true;
        UpdateVolume();

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
            s.source.Play();
            i++;
        }
    }
    public void UpdateVolume()
    {
        print("UPDATE");


        if (sourceAdded)
        {
            PlayerPrefs.SetFloat("MasterVolume_Music", masterVolumeSlider_music.value);
            PlayerPrefs.SetFloat("MasterVolume_Sounds", masterVolumeSlider_sounds.value);
            PlayerPrefs.SetFloat("MasterVolume_Dilogs", masterVolumeSlider_dilogs.value);
            PlayerPrefs.Save();

            foreach (Sound s in sounds)
            {
                s.source.volume = masterVolumeSlider_sounds.value;
            }
            foreach (Sound s in music)
            {
                s.source.volume = masterVolumeSlider_music.value;
            }
            foreach (Sound s in dialogs)
            {
                s.source.volume = masterVolumeSlider_dilogs.value;
            }
        }

        
    }
}
