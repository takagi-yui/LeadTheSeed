using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioClip bgm;
    public float time;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgm;
        StartCoroutine(Play());
    }

    void Update()
    {
        
    }

    IEnumerator Play()
    {
        while (true)
        {
            audioSource.Play();
            while(audioSource.time < time)
            {
                yield return null;
            }
        }
    }
}
