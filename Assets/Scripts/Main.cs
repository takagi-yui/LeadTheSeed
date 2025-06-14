using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static int stage;
    public static Main main;
    public static GameObject seed;
    public static int score;
    public static GameObject clear;
    public static int[] bonus =
    {
        0,//0
        141,//1
        138,//2
        152,//3
        155,//4
        154,//5
        174,//6
        149,//7
        192,//8
        144//9
    };
    public static int[,] rank =
    {
    {0,0,0},//0
    {0,860 + 100,860 + 910},//1
    {0,930 + 100,890 + 850},//2
    {0,960 + 100,960 + 910},//3
    {0,1110 + 100,740 + 1020},//4
    {0,1310 + 100,1310 + 880},//5
    {0,1010 + 100,810 + 1050},//6
    {0,860 + 100,590 + 1080},//7
    {0,1400 + 100,1160 + 900},//8
    {0,1080 + 100,720 + 940}//9
    };
    Vector3 random;
    float t;
    public static Rigidbody2D wind;
    GameObject play;
    public static float time;
    public static bool skip;
    GameObject message;

    private void Awake()
    {
        if ((float)Screen.height / (float)Screen.width > 9.0f / 16.0f)
        {
            Screen.SetResolution(Screen.width, Mathf.RoundToInt(Screen.width / 16.0f * 9.0f), Application.platform == RuntimePlatform.WebGLPlayer ? Screen.fullScreen : true);
        }
        else
        {
            Screen.SetResolution(Mathf.RoundToInt(Screen.height / 9.0f * 16.0f), Screen.height, Application.platform == RuntimePlatform.WebGLPlayer ? Screen.fullScreen : true);
        }
    }

    void Start()
    {
        stage = 0;
        if(SceneManager.GetActiveScene().name.Length > 5) int.TryParse(SceneManager.GetActiveScene().name.Substring(5),out stage);
        message = GameObject.Find("Message");
        main = GetComponent<Main>();
        score = 0;
        wind = new GameObject("MainWind").AddComponent<Rigidbody2D>();
        wind.gravityScale = 0;
        wind.linearDamping = 1;
        clear = GameObject.Find("Clear");
        clear.SetActive(false);
        t = 1;
        for(int i = 0; i < 3; i++)
        {
            if ((Data.data.jewel[stage] & 1 << i) == 1 << i) GameObject.Find(i.ToString()).GetComponent<Image>().color = new Color(0.6f,0.6f,0.6f,1);
        }
        play = GameObject.Find("Play");
        time = 0;
        skip = false;
        if (stage != 1)
        {
            message.SetActive(false);
        }
        StartCoroutine(StartFade());
    }

    void Update()
    {
        if (t >= 1)
        {
            random = Random.insideUnitCircle.normalized;
            t -= 1;
        }
        wind.AddForce(random);
        t += Time.deltaTime * 2;
        if (message.activeSelf && seed.transform.position.x > 10)
        {
            message.SetActive(false);
        }
        if (Time.timeScale == 1)
        {
            play.transform.Find("Score").GetComponent<Text>().text = "SCORE " + score.ToString().PadLeft(4, '0');
            play.transform.Find("Time").GetComponent<Text>().text = "TIME " + GetTime();
        }
        time += Time.deltaTime;
    }

    public static IEnumerator Clear()
    {
        if (Time.timeScale == 0) yield break;
        Time.timeScale = 0;
        yield return main.StartCoroutine(Fade(new Color(1,1,1,0), new Color(1,1,1,1), 0.5f));
        clear.SetActive(true);
        int jewel = 0;
        for(int i = 0; i < 3; i++)
        {
            if ((Data.data.jewel[stage] & 1 << i) == 1 << i) GameObject.Find("Jewel" + i.ToString()).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
            GameObject obj = GameObject.Find(i.ToString());
            if (obj.GetComponent<Image>().color == new Color(1, 1, 1, 1))
            {
                jewel = jewel | 1 << i;
                obj.transform.SetParent(clear.transform);
                obj.GetComponent<RectTransform>().position = GameObject.Find("Jewel" + i.ToString()).GetComponent<RectTransform>().position;
                obj.GetComponent<RectTransform>().sizeDelta = new Vector2(220,220);
                obj.GetComponent<Image>().color = new Color(1,1,1,0);
            }
        }
        int timeBonus = Mathf.Max(bonus[stage] - Mathf.FloorToInt(time),0) * 10 + 100;
        int totalScore = score + timeBonus;

        if (stage != 0)
        {
            GameObject.Find("A").GetComponent<RectTransform>().position = Vector3.Lerp(GameObject.Find("B").GetComponent<RectTransform>().position, GameObject.Find("S").GetComponent<RectTransform>().position, (float)Main.rank[stage, 1] / (float)Main.rank[stage, 2]);
            GameObject.Find("Rank").GetComponent<Text>().text = Rank(totalScore, stage);
            Data.data.stage = Mathf.Max(stage,Data.data.stage);
            Data.data.jewel[stage] = Data.data.jewel[stage] | jewel;
            Data.data.highScore[stage] = Mathf.Max(totalScore, Data.data.highScore[stage]);
            Data.Save();
        }

        Camera.main.transform.position = new Vector3(0,0,1000);
        GameObject.Find("Play").SetActive(false);
        yield return main.StartCoroutine(Fade(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.5f));
        for (int i = 0; i < 3; i++)
        {
            if ((jewel & 1 << i) == 1 << i)
            {
                yield return Main.main.StartCoroutine(Animation0(GameObject.Find(i.ToString()),3,0.3f));
            }
        }
        GameObject.Find("Time").GetComponent<Text>().text = GetTime();
        yield return Main.main.StartCoroutine(Animation0(GameObject.Find("Time"), 3, 0.3f));
        yield return Main.main.StartCoroutine(Animation1(GameObject.Find("Score"),score));
        yield return Main.main.StartCoroutine(Animation1(GameObject.Find("TimeBonus"), timeBonus));
        Main.main.StartCoroutine(ScoreGauge());
        yield return Main.main.StartCoroutine(Animation1(GameObject.Find("TotalScore"), totalScore));
        Main.main.StopCoroutine(ScoreGauge());
        yield return Main.main.StartCoroutine(Animation0(GameObject.Find("Rank"), 3, 0.3f));
        time = -0.25f;
        Color color;
        while (!Input.GetMouseButtonUp(0))
        {
            color = GameObject.Find("Click").GetComponent<Text>().color;
            color.a = (Mathf.Sin(time) + 1) * 0.5f;
            GameObject.Find("Click").GetComponent<Text>().color = color;
            color = GameObject.Find("ClickLine1").GetComponent<Image>().color;
            color.a = (Mathf.Sin(time) + 1) * 0.5f;
            GameObject.Find("ClickLine1").GetComponent<Image>().color = color;
            color = GameObject.Find("ClickLine2").GetComponent<Image>().color;
            color.a = (Mathf.Sin(time) + 1) * 0.5f;
            GameObject.Find("ClickLine2").GetComponent<Image>().color = color;
            time += Time.unscaledDeltaTime * 2;
            yield return null;
        }
        main.StartCoroutine(LoadWorld());
    }

    public static IEnumerator LoadWorld()
    {
        main.StartCoroutine(Fade(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 0.5f));
        AudioSource audio = Camera.main.GetComponent<AudioSource>();
        float volume = audio.volume;
        for (float time = 0.5f; time > 0; time -= Time.unscaledDeltaTime)
        {
            audio.volume = Mathf.Lerp(0, volume, time * 2);
            yield return null;
        }
        SceneManager.LoadScene("WorldMap");
    }

    public static string GetTime()
    {
        if(time < 3600)
        {
            return Mathf.FloorToInt(time / 60.0f).ToString().PadLeft(2, '0') + ":" + Mathf.FloorToInt(time % 60.0f).ToString().PadLeft(2, '0');
        }
        else
        {
            return "60:00";
        }
    }

    public static string Rank(int score, int stage)
    {
        string text = "B";
        if (score >= Main.rank[stage, 1])
        {
            text = "A";
        }
        if (score >= Main.rank[stage, 2])
        {
            text = "S";
        }
        return text;
    }

    public static IEnumerator Animation0(GameObject obj, float scale, float time)
    {
        RectTransform transform = obj.GetComponent<RectTransform>();
        Image image = obj.GetComponent<Image>();
        Text text = obj.GetComponent<Text>();
        for (float a = 0; a <= time; a += Time.unscaledDeltaTime)
        {
            transform.localScale = new Vector3(Mathf.Lerp(scale, 1, a / time), Mathf.Lerp(scale, 1, a / time), 1);
            if (image)
            {
                image.color = new Color(1, 1, 1, a);
            }
            else
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, a);
            }
            if (Input.GetMouseButtonUp(0))
            {
                skip = true;
            }
            if (skip) break;
            yield return null;
        }
        transform.localScale = new Vector3(1, 1, 1);
        if (image)
        {
            image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        }
    }

    public static IEnumerator Animation1(GameObject obj, int n)
    {
        Text text = obj.GetComponent<Text>();
        while(int.Parse(text.text) < n)
        {
            text.text = (int.Parse(text.text) + 10).ToString();
            if (Input.GetMouseButtonUp(0))
            {
                skip = true;
            }
            if (skip) break;
            yield return null;
        }
        text.text = n.ToString();
    }

    public static IEnumerator ScoreGauge()
    {
        Slider slider = GameObject.Find("ScoreGauge").GetComponent<Slider>();
        slider.maxValue = rank[stage, 2];
        while (true)
        {
            slider.value = int.Parse(GameObject.Find("TotalScore").GetComponent<Text>().text);
            yield return null;
        }
    }

    public static IEnumerator Fade(Color start, Color end, float time)
    {
        Image fade = GameObject.Find("Fade").GetComponent<Image>();
        fade.enabled = true;
        fade.color = start;
        for (float t = 0; t <= time; t += Time.unscaledDeltaTime)
        {
            fade.color = Color.Lerp(start,end,t / time);
            yield return null;
        }
        fade.color = end;
        if(end.a == 0)
        {
            fade.enabled = false;
        }
    }

    IEnumerator StartFade()
    {
        GameObject.Find("Fade").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Fade(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f));
    }
}
