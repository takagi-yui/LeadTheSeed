using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Rigidbody2D r;
    bool exit;

    void Start()
    {
        exit = false;
        r = GetComponent<Rigidbody2D>();
        r.AddForce((Main.seed.transform.position - transform.position).normalized * 200);
    }

    void Update()
    {
        transform.GetChild(0).Rotate(0,0,1000 * Time.deltaTime * -Mathf.Sign(r.linearVelocity.x));
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            exit = true;
            gameObject.layer = 10;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && exit)
        {
            collision.gameObject.GetComponent<Enemy>().Reset(true);
        }
        if ((collision.gameObject.layer == 10 || collision.gameObject.layer == 11) && exit)
        {
            Destroy(gameObject);
        }
    }
}
