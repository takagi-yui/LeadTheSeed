using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotate;
    Rigidbody2D r;

    void Start()
    {
        r = gameObject.AddComponent<Rigidbody2D>();
        r.bodyType = RigidbodyType2D.Kinematic;
    }
    
    void Update()
    {
        r.MoveRotation(r.rotation + (rotate.z * Time.deltaTime));
    }
}
