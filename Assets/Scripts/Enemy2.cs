using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    Rigidbody2D r;

    private void Awake()
    {
        GetComponent<Enemy>().type = System.Type.GetType("Enemy2");
    }

    void Start()
    {
        r = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Main.seed.transform.position.x - transform.position.x >= 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * 1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void FixedUpdate()
    {
        r.AddForce((Main.seed.transform.position - transform.position).normalized * 10);
    }
}
