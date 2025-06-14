using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public Type type;
    MonoBehaviour script;
    SpriteRenderer spriteRenderer;
    PolygonCollider2D polygonCollider;
    int state;
    Vector3 position;
    Vector3 scale;

    void Start()
    {
        position = transform.position;
        scale = transform.localScale;
        script = (MonoBehaviour)GetComponent(type);
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        script.enabled = false;
        spriteRenderer.enabled = false;
        polygonCollider.enabled = false;
        state = 0;
    }

    void Update()
    {
        if(state == -1 && !IsInside())
        {
            state = 0;
        }
        if(state == 0 && IsInside())
        {
            script.enabled = true;
            spriteRenderer.enabled = true;
            polygonCollider.enabled = true;
            state = 1;
        }
        if(state == 1 && (!IsInside() || Main.seed.GetComponent<Rigidbody2D>().isKinematic))
        {
            Reset(false);
        }
    }

    public void Reset(bool fade)
    {
        script.enabled = false;
        polygonCollider.enabled = false;
        state = -1;
        if (fade)
        {
            StartCoroutine(Fade());
        }
        else
        {
            StartCoroutine(ResetPosition());
        }
    }

    IEnumerator ResetPosition()
    {
        if (Time.timeScale == 0)
        {
            while (Time.timeScale == 0)
            {
                yield return null;
            }
            
            transform.position = position;
            transform.localScale = scale;
        }
        spriteRenderer.enabled = false;
        state = 0;
    }

    bool IsInside()
    {
        if(Mathf.Abs(transform.position.x - Camera.main.transform.position.x) < 16 && Mathf.Abs(transform.position.y - Camera.main.transform.position.y) < 9)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator Fade()
    {
        for(float a = 1; a > 0; a -= Time.deltaTime * 2)
        {
            spriteRenderer.material.SetFloat("_A", a);
            yield return null;
        }
        spriteRenderer.enabled = false;
        spriteRenderer.material.SetFloat("_A", 1);
        transform.position = position;
        transform.localScale = scale;
    }
}
