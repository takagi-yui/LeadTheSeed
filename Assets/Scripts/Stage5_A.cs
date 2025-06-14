using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5_A : MonoBehaviour
{
    Switch s;
    bool on;
    GameObject[] windObject;

    void Start()
    {
        s = GetComponent<Switch>();
        windObject = GameObject.FindGameObjectsWithTag("WindObject");
    }

    void Update()
    {
        if(s.on != on)
        {
            foreach(GameObject obj in windObject)
            {
                obj.transform.Rotate(0, 0, 180);
            }
        }
        on = s.on;
    }
}
