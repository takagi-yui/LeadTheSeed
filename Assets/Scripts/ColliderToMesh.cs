using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[ExecuteInEditMode]
public class ColliderToMesh : MonoBehaviour
{
	const float grassScale = 2;
	public Material shadowMaterial;
	public Material grassMaterial;
	public float angle;
	Vector2[] points = {};
	List<Vector3> vertices;
	List<Vector2> uv;
	List<int> triangles;

	void Awake()
	{
		vertices = new List<Vector3>();
		uv = new List<Vector2>();
		triangles = new List<int>();
	}

    void Update()
    {
		if (GetComponent<PolygonCollider2D>().points.Length >= 3 && !Application.isPlaying) {
			if(points.Length == 0 || !points.SequenceEqual (GetComponent<PolygonCollider2D>().points)){
				points = new Vector2[GetComponent<PolygonCollider2D> ().points.Length];
				Array.Copy (GetComponent<PolygonCollider2D> ().points, points, points.Length);
				float minX = points [0].x;
				float minY = points [0].y;
				for (int i = 0; i < points.Length; i++) {
					if (points [i].x < minX) {
						minX = points [i].x;
					}
					if (points [i].y < minY) {
						minY = points [i].y;
					}
				}

				Mesh mesh = new Mesh ();
				vertices.Clear ();
				for (int i = 0; i < points.Length; i++) {
					vertices.Add(points [i]);
				}
				mesh.vertices = vertices.ToArray();

				uv.Clear ();
				Vector2 scale = GetComponent<MeshRenderer> ().sharedMaterial.mainTextureScale * 4;
				for (int i = 0; i < points.Length; i++) {
					uv.Add(new Vector2 ((points[i].x - (Mathf.Ceil(minX / 4f) * 4f)) / scale.x, (points[i].y - (Mathf.Ceil(minY / 4f) * 4f)) / scale.y));
				}
				mesh.uv = uv.ToArray();

				triangles.Clear ();
				List<int> leftPoints = new List<int> ();
                for(int i = 0; i < points.Length; i++)
                {
                    leftPoints.Add(i);
                }
				int count1 = 0;
				while (leftPoints.Count > 3 && count1 < 1000)
                {
                    float distance = 0;
                    int p = 0;
                    for (int i = 0; i < leftPoints.Count; i++)
                    {
                        if (Vector2.Distance(points[leftPoints[i]], Vector2.zero) > distance)
                        {
                            distance = Vector2.Distance(points[leftPoints[i]], Vector2.zero);
                            p = i;
                        }
                    }
                    int front = 0;
                    int count = 1;
					int count2 = 0;
					while (count >= 1 && count2 < 1000)
                    {
                        count = 0;
                        Vector2 a = points[leftPoints[Next(leftPoints.Count, p + 1)]] - points[leftPoints[p]];
                        Vector2 b = points[leftPoints[Next(leftPoints.Count, p - 1)]] - points[leftPoints[p]];
                        float c = a.x * b.y - a.y * b.x;
                        if (front == 0)
                        {
                            if (c > 0) front = 1;
                            if (c < 0) front = -1;
                        }
                        else if ((front == 1 && c < 0) || (front == -1 && c > 0))
                        {
                            count++;
                        }
                        if (count == 0)
                        {
                            for (int i = 0; i < leftPoints.Count; i++)
                            {
                                if (Mathf.Abs(i - p) > 1 && !((i == 0 && p == leftPoints.Count - 1) || (p == 0 && i == leftPoints.Count - 1)))
                                {
                                    if (IsInside(points[leftPoints[i]], points[leftPoints[Next(leftPoints.Count, p + 1)]], points[leftPoints[p]], points[leftPoints[Next(leftPoints.Count, p - 1)]]))
                                    {
                                        count++;
                                    }
                                }
                            }
                        }
                        if(count >= 1)
                        {
                            p = Next(leftPoints.Count,p + 1);
                        }
						count2++;
                    }
                    triangles.Add(leftPoints[Next(leftPoints.Count, p + 1)]);
                    triangles.Add(leftPoints[p]);
                    triangles.Add(leftPoints[Next(leftPoints.Count, p - 1)]);
					leftPoints.RemoveAt (p);
					count1++;
                }
                triangles.Add(leftPoints[2]);
                triangles.Add(leftPoints[1]);
                triangles.Add(leftPoints[0]);
                mesh.triangles = triangles.ToArray();
                GetComponent<MeshFilter>().mesh = mesh;

				if (transform.Find ("Shadow") == null) {
					GameObject obj = new GameObject ("Shadow");
					obj.transform.parent = transform;
					obj.AddComponent<MeshFilter> ();
					obj.AddComponent<MeshRenderer> ().material = shadowMaterial;
					obj.transform.localPosition = Vector3.zero;
				}
				Mesh shadowMesh = new Mesh ();
				vertices.Clear();
				uv.Clear();
				triangles.Clear();
				for (int i = 0; i <= points.Length; i++) {
					uv.Add(new Vector2((float)Next(points.Length,i) / (float)points.Length,0));
					vertices.Add(points[Next(points.Length,i)]);
					uv.Add(new Vector2((float)Next(points.Length,i) / (float)points.Length,1));
					Vector2 vector = GetVector (points [Next (points.Length, i)],points [Next (points.Length, i + 1)],points [Next (points.Length, i - 1)]);
					vertices.Add((Vector3)points[Next(points.Length,i)] + (Vector3)(vector * 0.3f));
					if (i != 0) {
						AddTriangles ();
					}
				}
				shadowMesh.vertices = vertices.ToArray ();
				shadowMesh.uv = uv.ToArray();
				shadowMesh.triangles = triangles.ToArray ();
				transform.Find ("Shadow").GetComponent<MeshFilter> ().mesh = shadowMesh;
				Grass ();
            }
		}
		if (gameObject.transform.position.z == 0) {
			GetComponent<PolygonCollider2D> ().enabled = true;
		} else {
			GetComponent<PolygonCollider2D> ().enabled = false;
		}
    }

	void Grass(){
		if (transform.Find ("Grass") == null) {
			GameObject obj = new GameObject ("Grass");
			obj.transform.parent = transform;
			obj.AddComponent<MeshFilter> ();
			obj.AddComponent<MeshRenderer> ().material = grassMaterial;
			obj.transform.localPosition = new Vector3(0,0,-0.01f);
		}
		Mesh grassMesh = new Mesh ();
		vertices.Clear();
		uv.Clear();
		triangles.Clear();
		List<Vector2> grass = new List<Vector2> ();
		for (int i = 0; i < points.Length; i++) {
			if (Vector2.Angle (Vector2.left, points [Next (points.Length, i + 1)] - points [i]) <= angle) {
				grass.Add (points[i]);
			}
		}
		while (grass.Count > 0) {
			int p = 0;
			int count = 0;
			while (grass [Next(grass.Count, p + 1)] == Next(points,grass[p],1) && count < grass.Count) {
				p = Next(grass.Count, p + 1);
				count++;
			}
			if (count < grass.Count) {
				float length = 0;
				p++;
				do {
					p = Next (grass.Count, p - 1);
					length += Vector2.Distance (grass [p], Next (points, grass [p], 1));
				} while (grass [Next (grass.Count, p - 1)] == Next (points, grass [p], -1));
				uv.Add(new Vector2(0,0.5f));
				vertices.Add((Vector3)grass[p] + (Quaternion.Euler (0, 0, 90) * (Next (points, grass [p], 1) - grass[p]).normalized * -1));
				uv.Add(new Vector2(0,0));
				vertices.Add((Vector3)grass[p] + (Quaternion.Euler (0, 0, 90) * (Next (points, grass [p], 1) - grass[p]).normalized * 1));

				float u = 0;
				while (grass [Next(grass.Count, p + 1)] == Next(points,grass[p],1)) {
					grass.RemoveAt(p);
					if (p >= grass.Count) {
						p = 0;
					}
					u += Vector2.Distance (grass [p], Next (points, grass [p], -1)) / length * Mathf.RoundToInt (length / grassScale);
					Vector2 vector = GetVector (grass [p], Next (points, grass [p], 1), Next (points, grass [p], -1));
					uv.Add(new Vector2(u,0.5f));
					vertices.Add ((Vector3)grass[p] + (Vector3)(vector * -1));
					uv.Add(new Vector2(u,0));
					vertices.Add ((Vector3)grass[p] + (Vector3)(vector * 1));

					AddTriangles ();
				}
				uv.Add(new Vector2(Mathf.RoundToInt (length / grassScale),0.5f));
				vertices.Add((Vector3)Next (points, grass [p], 1) + (Quaternion.Euler (0, 0, 90) * (Next (points, grass [p], 1) - grass[p]).normalized * -1));
				uv.Add(new Vector2(Mathf.RoundToInt (length / grassScale),0));
				vertices.Add((Vector3)Next (points, grass [p], 1) + (Quaternion.Euler (0, 0, 90) * (Next (points, grass [p], 1) - grass[p]).normalized * 1));

				AddTriangles ();
				grass.RemoveAt(p);
			} else {
				grass.Clear ();
			}
		}
		grassMesh.vertices = vertices.ToArray ();
		grassMesh.uv = uv.ToArray();
		grassMesh.triangles = triangles.ToArray ();
		transform.Find ("Grass").GetComponent<MeshFilter> ().mesh = grassMesh;
	}
	
	bool IsInside(Vector2 p,Vector2 a,Vector2 b, Vector2 c){
		Vector2 ab = b - a;
		Vector2 bp = p - b;
		Vector2 bc = c - b;
		Vector2 cp = p - c;
		Vector2 ca = a - c;
		Vector2 ap = p - a;
		float c1 = ab.x * bp.y - ab.y * bp.x;
		float c2 = bc.x * cp.y - bc.y * cp.x;
		float c3 = ca.x * ap.y - ca.y * ap.x;
		if ((c1 > 0 && c2 > 0 && c3 > 0) || (c1 < 0 && c2 < 0 && c3 < 0)) {
			return true;
		} else {
			return false;
		}
	}
	int Next(int count, int p){
		int n = p % count;
		return n >= 0 ? n : count + n;
	}
	Vector2 Next(Vector2[] array, Vector2 vector2, int move){
		return array[Next (array.Length, Array.IndexOf (array, vector2) + move)];
	}
	Vector2 GetVector(Vector2 p, Vector2 a, Vector2 b){
		a = (a - p).normalized;
		b = (b - p).normalized;
		float c = Mathf.Sign(a.x * b.y - a.y * b.x);
		return ((a + b) * c).normalized;
	}
	void AddTriangles(){
		triangles.Add (vertices.Count - 4);
		triangles.Add (vertices.Count - 3);
		triangles.Add (vertices.Count - 2);
		triangles.Add (vertices.Count - 3);
		triangles.Add (vertices.Count - 1);
		triangles.Add (vertices.Count - 2);
	}
}
