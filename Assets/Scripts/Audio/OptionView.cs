using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionView : MonoBehaviour
{
    public const string key_music = "key_music";
    public const string key_sound = "key_sound";
    public Slider music;
    public Slider sound;
    private void Awake() {
        //初始化滑动条
        music.value = PlayerPrefs.GetFloat(key_music,0.5f);
        sound.value = PlayerPrefs.GetFloat(key_sound,0.5f);
    }
    public void OnSliderMusicChange(float f)
    {
        PlayerPrefs.SetFloat(key_music,f);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().UpdateVolume();
    }
    public void OnSliderSoundChange(float f)
    {
        PlayerPrefs.SetFloat(key_sound,f);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().UpdateVolume();
    }

}
