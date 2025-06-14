using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DrawLine : MonoBehaviour
{
    public GameObject windPrefab;
    Wind wind;
    GameObject mousePosition;

	void Start ()
	{
		wind = null;
        mousePosition = GameObject.Find("MousePosition");
    }

	void Update ()
	{
		Vector3 position = Input.mousePosition;
		position.z = 10;
		position = Camera.main.ScreenToWorldPoint (position);
		if (wind == null) {
			if (Input.GetMouseButtonDown (0)) {
				wind = Instantiate (windPrefab, Vector3.zero, Quaternion.identity).GetComponent<Wind> ();
				wind.AddPosition ();
				wind.AddPosition ();
                mousePosition.GetComponent<CircleCollider2D>().enabled = true;
			}
		} else {
			if (Input.GetMouseButton (0)) {
				if (Vector2.Distance (new Vector3 (wind.x [wind.x.Count - 2], wind.y [wind.y.Count - 2], 0), new Vector3 (wind.x [wind.x.Count - 1], wind.y [wind.y.Count - 1], 0)) < 2) {
					wind.x.RemoveAt (wind.x.Count - 1);
					wind.y.RemoveAt (wind.y.Count - 1);
					wind.time.RemoveAt (wind.time.Count - 1);
				}
				wind.AddPosition ();
			}
			if (Input.GetMouseButtonUp (0) || Time.timeScale == 0) {
				if (Vector2.Distance (new Vector3 (wind.x [wind.x.Count - 2], wind.y [wind.y.Count - 2], 0), new Vector3 (wind.x [wind.x.Count - 1], wind.y [wind.y.Count - 1], 0)) < 1) {
					wind.x.RemoveAt (wind.x.Count - 1);
					wind.y.RemoveAt (wind.y.Count - 1);
					wind.time.RemoveAt (wind.time.Count - 1);
				}
				wind = null;
                mousePosition.GetComponent<CircleCollider2D>().enabled = false;
            }
		}
        mousePosition.transform.position = position;
	}
}
