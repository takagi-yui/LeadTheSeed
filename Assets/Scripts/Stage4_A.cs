using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4_A : MonoBehaviour
{
    Switch s;
    Rigidbody2D r;
    public GameObject target;
    public Vector3 move;
    public bool isKinematic;

    void Start()
    {
        s = target.GetComponent<Switch>();
        r = gameObject.AddComponent<Rigidbody2D>();
        r.gravityScale = 0;
        r.mass = 10000;
        if (isKinematic) r.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        if (s.on)
        {
            r.MovePosition(transform.position + (move * 2 * Time.deltaTime));
        }
    }
}
