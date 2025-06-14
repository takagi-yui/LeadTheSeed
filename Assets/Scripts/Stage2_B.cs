using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_B : MonoBehaviour
{
    public GameObject target;
    AreaEffector2D effector;
    ParticleSystem.EmissionModule particle;
    float rate;

    void Start()
    {
        effector = GetComponent<AreaEffector2D>();
        particle = GetComponent<ParticleSystem>().emission;
        rate = particle.rateOverTime.constant;
    }

    void Update()
    {
        if (target.GetComponent<Switch>().on)
        {
            effector.enabled = false;
            particle.rateOverTime = 0;
        }
        else
        {
            effector.enabled = true;
            particle.rateOverTime = rate;
        }
    }
}
