using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public bool move;
    public GameObject eye;
    Rigidbody2D r;
    Animator animator;
    bool attack;
    float time;
    Vector3 position;
    Quaternion rotation;
    Vector3 scale;
    bool reset;
    bool turn;
    bool start;
    Transform parent;

    private void Awake()
    {
        
    }

    void Start()
    {
        r = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attack = false;
        time = 0.2f;
        position = transform.localPosition;
        rotation = transform.rotation;
        scale = transform.localScale;
        reset = false;
        turn = false;
        start = false;
        parent = transform.parent;
        if (!move)
        {
            animator.CrossFade("Enemy3_Idle", 0);
            if(parent != null)
            {
                r.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    void Update()
    {
        if(start)time = Mathf.Max(time - Time.deltaTime, 0);
        if (attack)
        {
            if (time == 0 && transform.Find("Ground").GetComponent<Enemy3_IsGrounded>().isGrounded)
            {
                if (move)
                {
                    animator.CrossFade("Enemy3_Run", 0.1f);
                }
                else
                {
                    animator.CrossFade("Enemy3_Idle", 0.1f);
                }
                attack = false;
                time = 0.1f;
            }
        }
        RaycastHit2D hit = Physics2D.Raycast(eye.transform.position, Main.seed.transform.position - eye.transform.position, 8, LayerMask.GetMask(new string[] { "ObjectA","ObjectB", "ObjectC"}));
        if (hit)
        {
            if (hit.collider.gameObject == Main.seed && !attack && transform.Find("Ground").GetComponent<Enemy3_IsGrounded>().isGrounded)
            {
                transform.SetParent(null);
                r.bodyType = RigidbodyType2D.Dynamic;
                animator.CrossFade("Enemy3_Attack", 0.1f);
                r.AddForce((Main.seed.transform.position - transform.position + new Vector3(0, 2, 0)) * 10000);
                attack = true;
                time = 0.1f;
            }
        }
        if(Main.seed.GetComponent<Rigidbody2D>().isKinematic)
        {
            StartCoroutine(Reset());
        }
    }

    private void FixedUpdate()
    {
        if (!attack && move && start)
        {
            r.AddForce(new Vector2(-transform.localScale.x * 500, 0));
        }
    }

    IEnumerator Reset()
    {
        if (reset) yield break;
        reset = true;
        while(Time.timeScale == 0)
        {
            yield return null;
        }
        if (parent != null)
        {
            transform.SetParent(parent);
            r.bodyType = RigidbodyType2D.Kinematic;
        }
        transform.localPosition = position;
        transform.rotation = rotation;
        transform.localScale = scale;
        r.linearVelocity = Vector2.zero;
        start = false;
        reset = false;
        turn = false;
    }

    private void OnWillRenderObject()
    {
        if(Camera.current.name != "SceneCamera")
        {
            start = true;
        }
    }

    public IEnumerator Turn()
    {
        if (turn || !move || attack) yield break;
        turn = true;
        animator.CrossFade("Enemy3_Idle", 0.1f);
        yield return new WaitForSeconds(1);
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        r.linearVelocity = Vector2.zero;
        transform.position -= new Vector3(transform.localScale.x,0,0);
        time = 0.1f;
        animator.CrossFade("Enemy3_Run", 0.1f);
        turn = false;
    }
}
