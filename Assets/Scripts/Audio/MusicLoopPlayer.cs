using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoopPlayer : MonoBehaviour
{
    public AudioClip[] audios;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Audio());
    }
    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator Audio()
    {  
            for (int i = 0; i < audios.Length; i++)
            {
                this.GetComponent<AudioSource>().clip = audios[i];
                this.GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(audios[i].length);
                if(i==audios.Length-1)
                {
                    i=-1;
                }
            }
    }
}