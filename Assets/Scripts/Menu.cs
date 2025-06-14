using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    GameObject menu;

    void Start()
    {
        menu = GameObject.Find("Menu");
        menu.SetActive(false);
    }

    void Update()
    {
        
    }

    public void Open()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }

    public void Close()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
    }

    public void WorldMap()
    {
        StartCoroutine(Main.LoadWorld());
    }
}
