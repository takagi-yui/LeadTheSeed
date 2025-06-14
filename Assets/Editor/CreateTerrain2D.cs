using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateTerrain2D: MonoBehaviour
{
	/*[MenuItem("GameObject/Create Terrain2D", priority = 21)]
	public static void Terrain2D()
	{
		GameObject prefab = Resources.Load ("Terrain2D") as GameObject;
		GameObject terrain2D = Instantiate (prefab,Vector3.zero,Quaternion.identity);
		int n = 0;
		while (GameObject.Find (prefab.name + (n == 0 ? "": " (" + n.ToString() + ")")) != null) {
			n++;
		}
		terrain2D.name = prefab.name + (n == 0 ? "": " (" + n.ToString() + ")");
		Undo.RegisterCreatedObjectUndo (terrain2D,"Create Terrain2D");
	}*/
}
