using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassObject : MonoBehaviour
{
    float tilt;
    float force;
    Vector3 position;
    float time;
    bool on;

    void Start()
    {
        tilt = 0;
        force = 0;
        position = transform.localPosition;
        time = 0.3f;
        on = true;
    }

    void Update()
    {
        if (Time.timeScale == 1)
        {
            force += (Quaternion.LookRotation(Vector3.forward, transform.up) * GetComponent<Rigidbody2D>().linearVelocity).x * 0.05f;
            if (on || Mathf.Abs(tilt) < 0.2f) force += (Quaternion.LookRotation(Vector3.forward, transform.up) * Main.wind.linearVelocity).x * 0.05f;
            time -= Time.deltaTime;
            if (time <= 0)
            {
                on = !on;
                time += Random.Range(0.2f, 0.4f);
            }
            force -= ((Mathf.Sign(tilt)) * (tilt * tilt) * 0.1f + (force * 5)) / 6.0f;
            tilt += force;
            GetComponent<Renderer>().material.SetFloat("_Tilt", tilt);
            transform.localPosition = position;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }
}
