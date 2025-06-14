using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jewel : MonoBehaviour
{
	Vector3 position;

    void Start()
    {
		position = transform.position;
    }
    
    void Update()
    {
		transform.position = new Vector3 (position.x, position.y + ((Mathf.Sin(Time.time * 1.2f)) / 5f), position.z);
    }

    public void Get()
    {
        Main.score += 100;
        GetComponentInChildren<ParticleSystem>().Play();
        GetComponentInChildren<AudioSource>().Play();
        transform.GetChild(0).parent = null;
        Destroy(gameObject);
    }
}
