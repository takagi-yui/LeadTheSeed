using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_A : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator.GetBool("Animation"))
        {
            GetComponentInChildren<Canvas>().enabled = false;
            enabled = false;
        }
    }
}
