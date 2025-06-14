using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Change(Color color)
    {
        ParticleSystem.MainModule particle = GetComponent<ParticleSystem>().main;
        particle.startColor = color;
    }
}
