using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public IEnumerator Animation()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetBool("Animation",true);
        yield return null;
    }
}
