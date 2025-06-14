using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class ElementEditor : MonoBehaviour
{
    static GameObject parent;
    static ElementEditor()
    {
        SceneView.duringSceneGui += OnGUI;
        EditorSceneManager.sceneOpened += OnOpened;
        parent = GameObject.Find("Elements");
        if (parent == null)
        {
            parent = new GameObject("Elements");
        }
    }
    static void OnOpened(Scene scene, OpenSceneMode mode)
    {
        parent = GameObject.Find("Elements");
        if (parent == null)
        {
            parent = new GameObject("Elements");
        }
    }
    static bool on;
    static int down = -1;
    static GameObject element;
    static Vector3 mousePosition;
    static void OnGUI(SceneView sceneView)
    {
        Handles.BeginGUI();
        on = GUILayout.Toggle(on, "ElementEditor", "button", GUILayout.Width(100));
        Handles.EndGUI();
        if (on)
        {
            Tools.lockedLayers = (1 << 30) - 1;
            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown && !Event.current.isScrollWheel)
                {
                    down = 1;
                    element = null;
                    mousePosition = GetMousePosition();
                    
                    for (int i = 0; i < parent.transform.childCount; i++)
                    {
                        if (Vector2.Distance(mousePosition, parent.transform.GetChild(i).transform.position) < 0.8f)
                        {
                            if (Event.current.button == 0)
                            {
                                element = parent.transform.GetChild(i).gameObject;
                            }
                            else
                            {
                                DestroyElement(parent.transform.GetChild(i).gameObject);
                            }
                        }
                    }
                    if (element == null && Event.current.button == 0)
                    {
                        CreateElement(mousePosition);
                        down = 0;
                    }
                    Selection.activeGameObject = element;
                    Tools.current = Tool.Rect;
                }
                if(Event.current.type == EventType.MouseMove)
                {
                    down = -1;
                }
                if (down >= 0 && element)
                {
                    if(down == 1 && mousePosition != GetMousePosition())
                    {
                        Undo.RecordObject(element.transform, "Move " + element.name);
                        down = 0;
                    }
                    element.transform.position = GetMousePosition();
                }
            }
        }
        else
        {
            Tools.lockedLayers = 0;
        }
    }
    static Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Event.current.mousePosition;
        mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
        mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        return mousePosition;
    }
    static void CreateElement(Vector3 mousePosition)
    {
        element = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Element.prefab"));
        element.transform.position = mousePosition;
        element.transform.SetParent(parent.transform);
        Undo.RegisterCreatedObjectUndo(element,"CreateElement");
    }
    static void DestroyElement(GameObject obj)
    {
        Undo.DestroyObjectImmediate(obj);
    }
}
