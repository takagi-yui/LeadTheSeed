using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public GameObject windCollider;
    public List<float> x;
    public List<float> y;
    public List<float> time;
    public List<Vector3> line;
    public List<float> convert;
	public List<GameObject> collider;
    float count;

    public class Effect
    {
        public float r;
        public float t;
        public float s;
    }
    public List<Effect> effects;

    void Start()
    {
		collider = new List<GameObject>();
        effects = new List<Effect>();
        count = Time.time;
        WindEffect.wind.Add(this);
    }

    void Update()
    {
		if (Time.time > time [3]) {
            if (time.Count < 5)
            {
                WindEffect.wind.Remove(this);
                Destroy(gameObject);
            }else if (Time.time > time [4]) {
				x.RemoveAt (0);
				y.RemoveAt (0);
				time.RemoveAt (0);
			}
		}
		if (time.Count >= 5)
        {
			Draw ();
		}
        if (Time.time >= count && convert.Count > 0)
        {
            effects.Add(new Effect());
            effects[effects.Count - 1].r = UnityEngine.Random.Range(-0.5f, 0.5f);
            effects[effects.Count - 1].t = convert[0];
            effects[effects.Count - 1].s = convert[0];
            count += 0.25f;
        }
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].s > effects[i].t)
            {
                effects.RemoveAt(i);
            }
        }
    }

    public void Draw()
    {
        line.Clear();
        convert.Clear();
        Spline splineX = new Spline(x.ToArray());
        Spline splineY = new Spline(y.ToArray());
		for (float t = Mathf.Max((Time.time - time[3]) / (time[4] - time[3]) + 3.0f,3.0f); t <= time.Count - 1; t += 0.25f)
        {
            line.Add(new Vector3(splineX.Get(t), splineY.Get(t),0));
            convert.Add(time[Mathf.FloorToInt(t)] + ((time[Mathf.CeilToInt(t)] - time[Mathf.FloorToInt(t)]) / 4.0f * (Mathf.FloorToInt(t / 0.25f) % 4)));
			t -= t % 0.25f;
        }
		if (line.Count >= 2 && Vector3.Distance(line[0],line[line.Count - 1]) >= 1)
		{
			while (collider.Count < line.Count - 1) {
				collider.Add ((GameObject)Instantiate (windCollider, gameObject.transform));
				collider[collider.Count - 1].name = (collider.Count == 1 ? "0" : (int.Parse(collider[collider.Count - 2].name) + 1).ToString());
			}
			while (collider.Count > line.Count - 1) {
				Destroy(collider[0]);
				collider.RemoveAt (0);
			}
			for (int i = 0; i < line.Count - 1; i++) {
                collider[i].GetComponent<AreaEffector2D>().forceMagnitude = Mathf.Min(Vector3.Distance(line[i], line[i + 1]) / (convert[i + 1] - convert[i]) * 0.8f, 10);
                if (i == 0)
                {
                    collider[i].transform.position = line[i] + (line[i] - line[i + 1]);
                    collider[i].transform.localScale = new Vector3(1, Vector3.Distance(line[i], line[i + 1]) * 4, 1);
                }
                else
                {
                    collider[i].transform.position = Vector3.Lerp(line[i], line[i + 1], 0.5f);
                    collider[i].transform.localScale = new Vector3(1, Vector3.Distance(line[i], line[i + 1]), 1);
                }
				
                collider [i].transform.rotation = Quaternion.FromToRotation (Vector3.up,line[i + 1] - line[i]);
			}
		}
    }

    public void AddPosition()
    {
		Vector3 position = Input.mousePosition;
		position.z = 10;
		position = Camera.main.ScreenToWorldPoint (position);
		if (time.Count == 0) {
			x.Add(position.x);
			y.Add(position.y);
			time.Add (Time.time + 2);
			x.Add(position.x);
			y.Add(position.y);
			time.Add (Time.time + 2);
			x.Add(position.x);
			y.Add(position.y);
			time.Add (Time.time + 2);
		}
        x.Add(position.x);
        y.Add(position.y);
		time.Add (Time.time + 2);
    }
}
