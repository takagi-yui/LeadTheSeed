using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline{
	private float[] a,b,c,d;
	private int num;

	public Spline(float[] spline){
		float tmp;
		float[] w = new float[spline.Length + 1];
		a = new float[spline.Length + 1];
		b = new float[spline.Length + 1];
		c = new float[spline.Length + 1];
		d = new float[spline.Length + 1];
		int i;
		num = spline.Length - 1;
		for (i = 0; i <= num; i++) {
			a[i] = spline[i];
		}
		c [0] = 0.0f;
		c [num] = 0.0f;
		for (i = 1; i < num; i++) {
			c[i] = 3.0f * (a[i - 1] - 2.0f * a[i] + a[i + 1]);
		}
		w[0] = 0.0f;
		for(i = 1; i < num; i++) {
			tmp = 4.0f - w[i - 1];
			c[i] = (c[i] - c[i - 1]) / tmp;
			w[i] = 1.0f / tmp;
		}
		for(i = num - 1; i > 0; i--) {
			c[i] = c[i] - c[i + 1] * w[i];
		}
		b [num] = 0.0f;
		d[num] = 0.0f;
		for(i = 0; i < num; i++) {
			d[i] = (c [i + 1] - c [i]) / 3.0f;
			b[i] = (a[i + 1] - a[i] - c[i] - d[i]);
		}
	}

	public float Get(float t){
		int j;
		float dt;
		j = Mathf.FloorToInt(t);
		if(j < 0) j = 0; else if (j >= num) j = num - 1;
		dt = t - (float)j;
		return a[j] + (b[j] + (c[j] + d[j] * dt) * dt) * dt;
	}
}
