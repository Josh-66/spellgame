using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class TableLightClickable : DeskObject
{
    // Start is called before the first frame update
    public Light2D candleLight;
    public SpriteRenderer candleSprite;
    public Color color;
    public bool lit = true;
    public AudioClip lightSound,extinguishSound;
    AudioSource audioSource;

    public override void Awake()
    {
        base.Awake();
        audioSource=GetComponent<AudioSource>();
    }

    public override void Update()
    {
        base.Update();
        Color targetColor = Color.Lerp(ScrollController.inkColor,Color.white,.5f);
        color=Color.Lerp(color,targetColor,Time.deltaTime*10f);

        candleLight.color=color;
        candleSprite.color=color;
    }
    public override void Activate()
    {
        audioSource.clip= lit ? extinguishSound:lightSound;
        audioSource.Play();
        lit = !lit;
        candleLight.gameObject.SetActive(lit);
    }
}
