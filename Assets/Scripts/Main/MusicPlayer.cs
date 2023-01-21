using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    static MusicPlayer instance;
    public AudioClip[] soundtrack;
    public AudioSource source;
    public float songDelay = 1f;
    public int lastIndex;
    public bool shouldPlay;
    // Start is called before the first frame update
    void Awake()
    {
        instance=this;
    }

    // Update is called once per frame
    void Update()
    {
        if (source.isPlaying && !shouldPlay){
            source.Stop();
        }
        if (!source.isPlaying && shouldPlay){
            songDelay-=Time.deltaTime;

            if (songDelay<0){
                source.clip=PickSong();
                source.Play();
                songDelay=Random.Range(1,2);
            }
        }
    }
    AudioClip PickSong(){
        int nextIndex = Random.Range(0,soundtrack.Length);
        int tries = 100;
        while (nextIndex==lastIndex && tries>0){
            tries--;
            nextIndex = Random.Range(0,soundtrack.Length);
        }
        lastIndex=nextIndex;
        return soundtrack[nextIndex];
    }
    public static void Start(){
        instance.shouldPlay=true;
    }
    public static void Stop(){
        instance.shouldPlay=false;
    }
}
