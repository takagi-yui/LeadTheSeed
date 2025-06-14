using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void LoadScene(int stageNumber)
    {
        StartCoroutine(Load(stageNumber));
    }
    IEnumerator Load(int stageNumber)
    {
        StartCoroutine(Main.Fade(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 0.5f));
        AudioSource audio = Camera.main.GetComponent<AudioSource>();
        float volume = audio.volume;
        for(float time = 0.5f; time > 0; time -= Time.unscaledDeltaTime)
        {
            audio.volume = Mathf.Lerp(0,volume,time * 2);
            yield return null;
        }
        SceneManager.LoadScene("Stage" + ((WorldMap.worldNumber - 1) * 3 + stageNumber));
    }
}
