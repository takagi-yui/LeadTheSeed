using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3_Collider : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 11)
        {
            StartCoroutine(transform.parent.GetComponent<Enemy3>().Turn());
        }
    }
}
