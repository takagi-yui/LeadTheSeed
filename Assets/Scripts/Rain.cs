using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    ParticleSystem.EmissionModule particle;
    public static bool rain;
    SpriteRenderer background;
    IEnumerator ienumerator;

    void Start()
    {
        particle = GameObject.Find("Rain").GetComponent<ParticleSystem>().emission;
        rain = false;
        background = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        ienumerator = Background(0, 0, 0);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "RainZone")
        {
            StartCoroutine(RainStart());
        }
    }
    IEnumerator RainStart()
    {
        particle.rateOverTime = 200;
        StopCoroutine(ienumerator);
        ienumerator = Background(background.color.r * 255, 200, 1.5f);
        StartCoroutine(ienumerator);
        yield return new WaitForSeconds(1.5f);
        rain = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "RainZone")
        {
            StartCoroutine(RainEnd());
        }
    }
    IEnumerator RainEnd()
    {
        particle.rateOverTime = 0;
        StopCoroutine(ienumerator);
        ienumerator = Background(background.color.r * 255, 230, 1.5f);
        StartCoroutine(ienumerator);
        yield return new WaitForSeconds(1f);
        rain = false;
    }

    IEnumerator Background(float a, float b, float time)
    {
        for(float i = 0; i < time; i+= Time.deltaTime)
        {
            float c = Mathf.Lerp(a,b,i / time) / 255;
            background.color = new Color(c, c, c, 1);
            yield return null;
        }
    }
}
