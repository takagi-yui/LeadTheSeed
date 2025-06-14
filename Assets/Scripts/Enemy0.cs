using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0 : MonoBehaviour
{
    Rigidbody2D r;

    private void Awake()
    {
        GetComponent<Enemy>().type = System.Type.GetType("Enemy0");
    }

    void Start()
    {
        r = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        r.AddForce(new Vector2(-10 * Mathf.Sign(transform.localScale.x), 0));
    }

    public void Turn()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
