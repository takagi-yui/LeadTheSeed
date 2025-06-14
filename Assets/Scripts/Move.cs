using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector2 position1;
    public Vector2 position2;
    public float speed;
    Rigidbody2D rigidbody2d;
    bool on;
    Vector3 position;

    void Start()
    {
        position1 += (Vector2)transform.position;
        position2 += (Vector2)transform.position;
        rigidbody2d = gameObject.AddComponent<Rigidbody2D>();
        rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (on) Main.seed.transform.position += transform.position - position;
        position = transform.position;
        if (Vector2.Distance(transform.position,position2) < 0.1f)
        {
            Vector2 position3 = position1;
            position1 = position2;
            position2 = position3;
        }
    }

    private void FixedUpdate()
    {
        rigidbody2d.MovePosition(Vector2.MoveTowards(transform.position, position2, speed * Time.deltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Seed")
        {
            on = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Seed")
        {
            on = false;
            collision.gameObject.GetComponent<Seed>().moveObject = null;
        }
    }
}
