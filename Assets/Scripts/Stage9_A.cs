using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage9_A : MonoBehaviour
{
    Switch s;
    SpriteRenderer[] spriteRenderer;
    public GameObject obj;
    public float time;

    void Start()
    {
        s = GetComponent<Switch>();
        spriteRenderer = new SpriteRenderer[obj.transform.childCount];
        for(int i = 0; i < obj.transform.childCount; i++)
        {
            spriteRenderer[i] = obj.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (s.on)
        {
            foreach(SpriteRenderer sprite in spriteRenderer)
            {
                Color color = sprite.color;
                color.a = Mathf.Min(color.a + (5 * Time.deltaTime),1);
                sprite.color = color;
            }
        }
        else
        {
            foreach (SpriteRenderer sprite in spriteRenderer)
            {
                Color color = sprite.color;
                color.a = Mathf.Max(color.a - (Time.deltaTime / time),0);
                sprite.color = color;
            }
        }
    }
}
