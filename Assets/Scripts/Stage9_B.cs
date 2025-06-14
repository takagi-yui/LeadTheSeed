using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage9_B : MonoBehaviour
{
    public GameObject target;
    Quaternion rotation;
    public Vector3 move;
    Rigidbody2D r;
    Color color;
    float t;
    Vector3 position;

    void Start()
    {
        position = target.transform.position;
        rotation = transform.rotation;
        color = new Color(1, 0.8f, 1, 1);
        r = target.AddComponent<Rigidbody2D>();
        r.gravityScale = 0;
        r.mass = 100;
        if (move.x == 0)
        {
            r.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        if (move.y == 0)
        {
            r.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        r.freezeRotation = true;
    }

    void Update()
    {
        target.transform.position = new Vector2(Mathf.Clamp(target.transform.position.x, position.x, position.x + 40), target.transform.position.y);
        r.linearVelocity = move * Quaternion.Angle(transform.rotation, rotation);
        r.linearVelocity += new Vector2(100 * Time.deltaTime,0);
        rotation = transform.rotation;
        t += Time.deltaTime;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(Mathf.Lerp(1, color.r, Mathf.Abs((t % 2) - 1)), Mathf.Lerp(1, color.g, Mathf.Abs((t % 2) - 1)), Mathf.Lerp(1, color.b, Mathf.Abs((t % 2) - 1)));
        }
    }
}
