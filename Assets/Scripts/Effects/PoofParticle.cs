using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoofParticle : MonoBehaviour
{

    float rotationSpeed;

    float lifeTimer=2f;
    float scale;
    public SpriteRenderer spriteRenderer;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        color = Random.Range(.9f,1.1f) * ScrollController.instance.inkController.inkColor;
        scale = Random.Range(1,2);
        rotationSpeed=Random.Range(-180f,180f);
        transform.localPosition=Random.insideUnitCircle*.5f;
        spriteRenderer.color=color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition+= Time.deltaTime*transform.localPosition.normalized*.5f;
        transform.rotation=Quaternion.Euler(0,0,transform.rotation.eulerAngles.z+rotationSpeed * Time.deltaTime);

        lifeTimer-=Time.deltaTime;
        if (lifeTimer<1){
            transform.localScale=Vector3.one*lifeTimer*scale;
        }
        if (lifeTimer<0)
            GameObject.Destroy(gameObject);

    }
}
