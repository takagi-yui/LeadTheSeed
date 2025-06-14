using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColliderToMesh))]
public class ColliderToMeshEditor : Editor
{
	public override void OnInspectorGUI(){
		ColliderToMesh obj = target as ColliderToMesh;
		obj.grassMaterial = (Material)EditorGUILayout.ObjectField ("Grass",obj.grassMaterial,typeof(Material),false);
		obj.shadowMaterial = (Material)EditorGUILayout.ObjectField ("Shadow",obj.shadowMaterial,typeof(Material),false);
		obj.angle = EditorGUILayout.Slider ("Angle",obj.angle,-1.0f,180.0f);
		EditorUtility.SetDirty (obj);
	}
}
