using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThudController : MonoBehaviour
{
    public static ThudController instance;
    public AudioClip[] thuds;
    AudioSource audioSource;
    void Awake()
    {
        instance=this;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public static void PlayThud(){
        if (!instance.audioSource.isPlaying || instance.audioSource.time>.5f)
            instance.audioSource.PlayRand(instance.thuds);
    }
}
