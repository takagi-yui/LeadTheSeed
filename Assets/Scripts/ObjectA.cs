using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectA : MonoBehaviour
{
	Wind wind;
	Rigidbody2D r;
    int n;
    float time;

	void Start()
    {
		r = GetComponent<Rigidbody2D>();
		n = -1;
		wind = null;
        time = 0;
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
		if(collider.gameObject.layer == 8){
            if (wind == null || (wind != null && int.Parse(collider.gameObject.name) > n && int.Parse(collider.gameObject.name) <= n + 4))
            {
                n = int.Parse(collider.gameObject.name);
                wind = collider.transform.parent.GetComponent<Wind>();
            }
            if (collider.transform.parent.gameObject != wind.gameObject && time == 0)
            {
                n = int.Parse(collider.gameObject.name);
                wind = collider.transform.parent.GetComponent<Wind>();
                time = 0.2f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WindObject")
        {
            Clear();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 8 && wind != null)
        {
            if(collider.gameObject == wind.collider[wind.collider.Count - 1])
            {
                Clear();
            }
        }
    }

    void Update()
    {
        time = Mathf.Max(time - Time.deltaTime, 0);
        if (wind != null) {
			if (n < int.Parse (wind.collider [0].name)) {
                Clear();
			}
		}
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 100, LayerMask.GetMask(new string[] { "ObjectB", "ObjectC", "Rain" }));
        if (hit == true && Rain.rain)
        {
            if (hit.collider.gameObject.layer == 16)
            {
                r.AddForce(Vector3.down * 30);
                Clear();
            }
        }
        if (wind != null)
        {
            int m = n - int.Parse (wind.collider [0].name);
			float a = (wind.line [m + 1].y - wind.line [m].y) / (wind.line [m + 1].x - wind.line [m].x);
			float b = wind.line [m].y - (a * wind.line [m].x);
			float c = 1 / a * -1;
			float d = transform.position.y - (c * transform.position.x);
			float x = (d - b) / (a - c);
			float y = a * x + b;
            if (!float.IsNaN(x) && !float.IsNaN(y))
            {
                transform.position = (Vector3.Lerp(transform.position, new Vector3(x, y, 0), 3 * Time.deltaTime));
                r.AddForce(new Vector2(x - transform.position.x, y - transform.position.y) * wind.collider[m].GetComponent<AreaEffector2D>().forceMagnitude * 15);
            }
        }
    }

    public void Clear()
    {
        n = -1;
        wind = null;
        time = 0.1f;
    }
}
