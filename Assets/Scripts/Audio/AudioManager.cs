using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public const string key_music = "key_music";
    public const string key_sound = "key_sound";
    private AudioSource music;
    private AudioSource sound;
    public AudioMixer audioMixer;
    private void Awake() {
        
        music = transform.Find("music").GetComponent<AudioSource>();
        sound = transform.Find("sound").GetComponent<AudioSource>();

        UpdateVolume();
    }

    //播放背景音乐(不能重叠)
    public void PlayMusic(AudioClip clip)
    {
        music.clip = clip;
        music.Play();
    }
    //播放音效
    public void PlaySound(AudioClip clip)
    {
        sound.PlayOneShot(clip);
    }
    //更新音量大小
    public void UpdateVolume(){
        //music.volume = PlayerPrefs.GetFloat(key_music,0.5f);
        //sound.volume = PlayerPrefs.GetFloat(key_sound,0.5f);
        audioMixer.SetFloat("MusicVolume",PlayerPrefs.GetFloat(key_music,0.5f));
        audioMixer.SetFloat("SoundVolume",PlayerPrefs.GetFloat(key_sound,0.5f));
    }

    private void OnDestroy() {
        
    }
}
