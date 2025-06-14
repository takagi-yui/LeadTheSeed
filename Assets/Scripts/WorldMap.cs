using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldMap : MonoBehaviour
{
    public static bool title = true;
    public static int worldNumber = 1;
    public GameObject windPrefab;

    class TitleWind
    {
        Wind wind;
        Vector3 position;
        float rotation;
        float r;
        float t;
        float count;

        public TitleWind(GameObject prefab)
        {
            position = new Vector3(Random.Range(-10.0f,10.0f), Random.Range(-6.0f, 6.0f));
            rotation = Random.Range(0, 360);
            wind = Instantiate(prefab, Vector3.zero, Quaternion.identity,GameObject.Find("Title").transform).GetComponent<Wind>();
            wind.x.Add(position.x);
            wind.y.Add(position.y);
            wind.time.Add(Time.time + 2);
            wind.x.Add(position.x);
            wind.y.Add(position.y);
            wind.time.Add(Time.time + 2);
            wind.x.Add(position.x);
            wind.y.Add(position.y);
            wind.time.Add(Time.time + 2);
            wind.x.Add(position.x);
            wind.y.Add(position.y);
            wind.time.Add(Time.time + 2);
            r = 0;
            t = Random.Range(3, 5);
            count = 0.3f;
        }

        public void Move()
        {
            if(t > 0)
            {
                if(count == 0)
                {
                    r += Random.Range(-20, 20);
                    rotation += r;
                    position += new Vector3(Mathf.Sin(rotation * Mathf.Deg2Rad),Mathf.Cos(rotation * Mathf.Deg2Rad),0) * 3;
                    wind.x.Add(position.x);
                    wind.y.Add(position.y);
                    wind.time.Add(Time.time + 2);
                    count = 0.2f;
                }
                count = Mathf.Max(count - Time.deltaTime, 0);
                t = Mathf.Max(t - Time.deltaTime,0);
            }
        }
    }

    List<TitleWind> titleWind;
    Vector3 mousePosition;
    GameObject[] stageButton;
    GameObject worldCanvas;
    GameObject titleCanvas;
    GameObject background;
    public GameObject reset;
    float time;
    bool move;

    private void Awake()
    {
        if ((float)Screen.height / (float)Screen.width > 9.0f / 16.0f)
        {
            Screen.SetResolution(Screen.width, Mathf.RoundToInt(Screen.width / 16.0f * 9.0f), Application.platform == RuntimePlatform.WebGLPlayer? Screen.fullScreen : true);
        }
        else
        {
            Screen.SetResolution(Mathf.RoundToInt(Screen.height / 9.0f * 16.0f), Screen.height, Application.platform == RuntimePlatform.WebGLPlayer ? Screen.fullScreen : true);
        }
    }

    void Start()
    {
        Time.timeScale = 1;
        transform.position = new Vector3(10 * (worldNumber - 1), transform.position.y, transform.position.z);
        mousePosition = new Vector3(0, 0, -1);
        stageButton = new GameObject[3];
        for(int i = 0; i < 3; i++)
        {
            stageButton[i] = GameObject.Find("StageButton" + (i + 1));
        }
        worldCanvas = GameObject.Find("WorldMap");
        worldCanvas.SetActive(false);
        titleCanvas = GameObject.Find("Title");
        background = GameObject.Find("Background1");
        if (!title)
        {
            titleCanvas.SetActive(false);
            background.SetActive(false);
            worldCanvas.SetActive(true);
        }
        titleWind = new List<TitleWind>();
        time = 0;
        StartCoroutine(StartFade());
        
    }

    void Update()
    {
        if (!title)
        {
            for (int i = 0; i < 3; i++)
            {
                stageButton[i].transform.Find("Text").GetComponent<Text>().text = worldNumber + " - " + (i + 1);
                for (int n = 0; n < 3; n++)
                {
                    if ((Data.data.jewel[(worldNumber - 1) * 3 + i + 1] & 1 << n) == 1 << n)
                    {
                        stageButton[i].transform.Find(n.ToString()).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    }
                    else
                    {
                        stageButton[i].transform.Find(n.ToString()).GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
                    }
                }
                stageButton[i].transform.Find("Score").GetComponent<Text>().text = Data.data.highScore[(worldNumber - 1) * 3 + i + 1].ToString().PadLeft(4, '0');
                stageButton[i].transform.Find("Rank").GetComponent<Text>().text = (worldNumber - 1) * 3 + i + 1 <= Data.data.stage ? Main.Rank(Data.data.highScore[(worldNumber - 1) * 3 + i + 1], (worldNumber - 1) * 3 + i + 1) : "-";
                stageButton[i].GetComponent<Button>().interactable = (worldNumber - 1) * 3 + i + 1 <= Data.data.stage + 1 ? true : false;
                GameObject obj = GameObject.Find("World").transform.Find((i + 1).ToString()).gameObject;
                obj.transform.Find("WorldClear").GetComponent<SpriteRenderer>().enabled = Mathf.FloorToInt(Data.data.stage / 3.0f) >= i + 1 ? true : false;
            }
            GameObject.Find("Right").GetComponent<Button>().interactable = worldNumber == Mathf.Min(Mathf.FloorToInt(Data.data.stage / 3.0f),2) + 1 ? false : true;
            GameObject.Find("Left").GetComponent<Button>().interactable = worldNumber == 1 ? false : true;
            if (Input.GetMouseButtonDown(0))
            {
                mousePosition = Input.mousePosition;
                if (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)).y > -0.1f && Mathf.Abs(transform.position.x - GameObject.Find("World").transform.Find(worldNumber.ToString()).transform.position.x) < 1)
                {
                    move = true;
                }
                else
                {
                    move = false;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                mousePosition = new Vector3(0, 0, -1);
                if (move)
                {
                    if (worldNumber == Mathf.RoundToInt(transform.position.x / 10) + 1)
                    {
                        if (transform.position.x - (10 * (worldNumber - 1)) > 0.5f)
                        {
                            worldNumber++;
                        }
                        else
                        {
                            if (transform.position.x - (10 * (worldNumber - 1)) < -0.5f)
                            {
                                worldNumber--;
                            }
                        }
                    }
                    else
                    {
                        worldNumber = Mathf.RoundToInt(transform.position.x / 10) + 1;
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (move)
                {
                    transform.position += new Vector3((mousePosition.x - Input.mousePosition.x) * 0.01f, 0, 0);
                }
                mousePosition = Input.mousePosition;
            }
            else
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, 10 * (worldNumber - 1), 0.05f), transform.position.y, transform.position.z);
            }
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, 10 * Mathf.Min(Mathf.FloorToInt(Data.data.stage / 3.0f),2)), transform.position.y, transform.position.z);
        }
        else
        {
            foreach(TitleWind wind in titleWind)
            {
                wind.Move();
            }
            if(time == 0) {
                titleWind.Add(new TitleWind(windPrefab));
                time = 1;
            }
            time = Mathf.Max(time - Time.deltaTime, 0);
        }
    }

    public void StartButton()
    {
        StartCoroutine(Button());
    }

    public void End()
    {
        Application.Quit();
    }

    IEnumerator Button()
    {
        yield return StartCoroutine(Main.Fade(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 0.5f));
        foreach(Wind wind in WindEffect.wind)
        {
            Destroy(wind.gameObject);
        }
        WindEffect.wind.Clear();
        titleCanvas.SetActive(false);
        background.SetActive(false);
        worldCanvas.SetActive(true);
        title = false;
        yield return StartCoroutine(Main.Fade(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f));
    }

    IEnumerator StartFade()
    {
        GameObject.Find("Fade").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Main.Fade(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f));
    }

    public void Right()
    {
        worldNumber++;
        move = false;
    }

    public void Left()
    {
        worldNumber--;
        move = false;
    }

    public void BackButton()
    {
        StartCoroutine(Back());
    }

    IEnumerator Back()
    {
        yield return StartCoroutine(Main.Fade(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 0.5f));
        transform.position = new Vector3(0, 0.5f, -10);
        titleCanvas.SetActive(true);
        background.SetActive(true);
        title = true;
        worldNumber = 1;
        Start();
    }

    public void ResetButton()
    {
        Time.timeScale = 0;
        reset.SetActive(true);
    }

    public void Yes()
    {
        Data.Delete();
        StartCoroutine(LoadWorld());
    }

    IEnumerator LoadWorld()
    {
        StartCoroutine(Main.Fade(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 0.5f));
        AudioSource audio = Camera.main.GetComponent<AudioSource>();
        float volume = audio.volume;
        for (float time = 0.5f; time > 0; time -= Time.unscaledDeltaTime)
        {
            audio.volume = Mathf.Lerp(0, volume, time * 2);
            yield return null;
        }
        worldNumber = 1;
        SceneManager.LoadScene("WorldMap");
    }

    public void No()
    {
        reset.SetActive(false);
        Time.timeScale = 1;
    }
}
