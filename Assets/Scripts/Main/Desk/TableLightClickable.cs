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

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Color targetColor = Color.Lerp(ScrollController.inkColor,Color.white,.5f);
        color=Color.Lerp(color,targetColor,Time.deltaTime*20f);

        candleLight.color=color;
        candleSprite.color=color;
    }
    public override void Activate()
    {
        lit = !lit;
        candleLight.gameObject.SetActive(lit);
    }
}
