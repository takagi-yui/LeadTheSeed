using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public Color colorOff;
    public Color colorOn;
    [HideInInspector]
    public bool on;
    ParticleSystem.MainModule particle;

    void Start()
    {
        
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,0,0);
        if (on)
        {
            particle = GetComponent<ParticleSystem>().main;
            particle.startColor = colorOn;
            GetComponentInChildren<ChangeColor>().Change(colorOn);
        }
        else
        {
            particle = GetComponent<ParticleSystem>().main;
            particle.startColor = colorOff;
            GetComponentInChildren<ChangeColor>().Change(colorOff);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            on = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            on = false;
        }
    }
}
