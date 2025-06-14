using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
	public Material lineMaterial;
	public static List<Wind> wind;

	void Start ()
	{
        wind = new List<Wind>();
	}

    void Update()
    {
        
    }

	void OnPostRender ()
	{
        GL.PushMatrix();
        lineMaterial.SetPass(0);
        for (int m = 0; m < wind.Count; m++)
        {
            if (wind[m].convert.Count >= 5)
            {
                for (int i = 0; i < wind[m].effects.Count; i++)
                {
                    int p = 0;
                    while (wind[m].effects[i].t > wind[m].convert[p + 1] && p < wind[m].convert.Count - 2)
                    {
                        p++;
                    }
                    if (p < wind[m].convert.Count - 2)
                    {
                        wind[m].effects[i].t += Time.deltaTime;
                    }
                    else
                    {
                        if (wind[m].effects[i].s < wind[m].convert[Mathf.Max(p - 8, 0)])
                        {
                            wind[m].effects[i].s = wind[m].convert[Mathf.Max(p - 8, 0)];
                        }
                        wind[m].effects[i].s += Time.deltaTime;
                    }
                    GL.Begin(GL.TRIANGLE_STRIP);
                    int start = Mathf.Max(p - 8, 0);
                    while (wind[m].effects[i].s > wind[m].convert[start + 1] && start < wind[m].convert.Count - 2)
                    {
                        start++;
                    }
                    for (int n = start; n <= p; n++)
                    {
                        GL.TexCoord2((float)(n - start) / (float)(p - start), 0);
                        GL.Vertex(wind[m].line[n + 1] + (Quaternion.Euler(0, 0, 90) * ((wind[m].line[n + 1] - wind[m].line[n]) / Vector3.Distance(wind[m].line[n], wind[m].line[n + 1])) * (wind[m].effects[i].r + 0.1f)) + new Vector3(0, 0, 0.01f));
                        GL.TexCoord2((float)(n - start) / (float)(p - start), 1);
                        GL.Vertex(wind[m].line[n + 1] + (Quaternion.Euler(0, 0, 90) * ((wind[m].line[n + 1] - wind[m].line[n]) / Vector3.Distance(wind[m].line[n], wind[m].line[n + 1])) * (wind[m].effects[i].r - 0.1f)) + new Vector3(0, 0, 0.01f));
                    }
                    GL.End();
                }
                
            }
        }
        GL.PopMatrix();

    }

}
