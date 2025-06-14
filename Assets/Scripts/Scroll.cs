using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public static Vector3 screenSize;
	Vector2 center;
	Rigidbody2D r;
	GameObject stageSize;
	Vector3 min;
	Vector3 max;
    void Start()
    {
		center = Main.seed.transform.position - transform.position;
		r = GetComponent<Rigidbody2D> ();
		stageSize = GameObject.Find ("StageSize");
        Vector3 position = transform.position;
        transform.position = new Vector3(0, 0, -10);
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -transform.position.z));
        transform.position = position;
		min = stageSize.transform.position - (stageSize.transform.localScale / 2f) + (screenSize);
		max = stageSize.transform.position + (stageSize.transform.localScale / 2f) - (screenSize);
    }
    void Update()
    {
		Vector2 vector = ((Vector2)Main.seed.transform.position - ((Vector2)transform.position + center));
		r.linearVelocity = (r.linearVelocity * 4 + (new Vector2(Mathf.Pow(vector.x, 2) * Mathf.Sign(vector.x), Mathf.Pow(vector.y, 2) * Mathf.Sign(vector.y)))) / 5f;
		transform.position = new Vector3 (Mathf.Clamp(transform.position.x,min.x,max.x),Mathf.Clamp(transform.position.y,min.y,max.y),transform.position.z);
    }

    public void ResetPosition(Vector3 startPosition)
    {
        transform.position = startPosition - ((Vector3)center - new Vector3(0, 0, transform.position.z));
        r.linearVelocity = Vector2.zero;
    }
}
