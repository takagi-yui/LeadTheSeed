using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if(Vector3.Distance(transform.position,Main.seed.transform.position) < 1.4f)
        {
            Main.score += 10;
            GetComponentInChildren<ParticleSystem>().Play();
            GetComponentInChildren<AudioSource>().Play();
            transform.GetChild(0).parent = null;
            Destroy(gameObject);
        }
    }
}
