using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0_Collider : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 11)
        {
            transform.parent.GetComponent<Enemy0>().Turn();
        }
    }
}
