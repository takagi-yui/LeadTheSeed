using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public GameObject attack;
    public float time;

    private void Awake()
    {
        GetComponent<Enemy>().type = System.Type.GetType("Enemy1");
    }

    void Start()
    {
        StartCoroutine(Count());
    }

    void Update()
    {
        if(Main.seed.transform.position.x - transform.position.x >= 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1,transform.localScale.y,transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * 1, transform.localScale.y, transform.localScale.z);
        }
    }

    IEnumerator Count()
    {
        yield return new WaitForSeconds(time);
        while (true)
        {
            GetComponent<Animator>().CrossFade("Enemy1_Attack", 0.1f, 1);
            yield return new WaitForSeconds(0.5f);
            if(GetComponent<SpriteRenderer>().enabled == true)
            Instantiate(attack,transform.position + new Vector3(-1 * Mathf.Sign(transform.localScale.x),0,0),Quaternion.identity);
            yield return new WaitForSeconds(2.5f);
        }
    }
}
