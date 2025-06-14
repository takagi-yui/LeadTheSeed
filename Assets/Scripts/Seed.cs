using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class Seed : MonoBehaviour
{
    Vector3 startPosition;
    GameObject[] checkpoint;
    GameObject[] jewel;
    [HideInInspector]
    public GameObject moveObject;
    public int startPoint;
    public bool sortX;
    bool thunder;

    private void Awake()
    {
        Main.seed = gameObject;
    }

    void Start()
    {
        startPosition = transform.position;
        checkpoint = GameObject.FindGameObjectsWithTag("Checkpoint");
        checkpoint = checkpoint.OrderBy(obj => sortX ? obj.transform.position.x : obj.transform.position.y).ToArray();
        jewel = GameObject.FindGameObjectsWithTag("Jewel");
        jewel = jewel.OrderBy(obj => sortX ? obj.transform.position.x : obj.transform.position.y).ToArray();
        thunder = false;

        if (startPoint >= 0)
        {
            startPosition = checkpoint[startPoint].transform.position;
            transform.position = startPosition;
            Camera.main.gameObject.GetComponent<Scroll>().ResetPosition(startPosition);
        }
    }

    void Update()
    {
        if(gameObject.transform.position.y < GameObject.Find("StageSize").transform.position.y - (GameObject.Find("StageSize").transform.localScale.y * 0.5f) - 1 && !transform.GetComponent<Rigidbody2D>().isKinematic)
        {
            StartCoroutine(ReStart());
        }
        if (gameObject.transform.position.x > GameObject.Find("StageSize").transform.position.x + (GameObject.Find("StageSize").transform.localScale.x * 0.5f) + 1)
        {
            StartCoroutine(Main.Clear());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint" && Array.IndexOf(checkpoint, collision.gameObject) != -1)
        {
            for(int i = 0; i <= Array.IndexOf(checkpoint, collision.gameObject); i++)
            {
                if (checkpoint[i] != null)
                {
                    startPosition = checkpoint[i].transform.position + new Vector3(0,1,0);
                    StartCoroutine(checkpoint[i].GetComponent<Checkpoint>().Animation());
                    checkpoint[i] = null;
                }
            }
        }
        if(collision.tag == "Jewel")
        {
            GameObject.Find(Array.IndexOf(jewel, collision.gameObject).ToString()).GetComponent<Image>().color = new Color(1,1,1,1);
            collision.GetComponent<Jewel>().Get();
        }
        if(collision.tag == "Enemy")
        {
            StartCoroutine(ReStart());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(ReStart());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            Vector2 normal = Vector2.zero;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                normal = contact.normal;
            }

            Move move;
            if (moveObject != null && collision.gameObject != moveObject)
            {
                move = moveObject.GetComponent<Move>();
                if (Mathf.Abs(Vector2.Angle(move.position1 - move.position2, normal)) < 10)
                {
                    StartCoroutine(ReStart());
                }
            }
            move = collision.gameObject.GetComponent<Move>();
            if (move != null)
            {
                if (Mathf.Abs(Vector2.Angle(move.position2 - move.position1, normal)) < 10)
                {
                    moveObject = move.gameObject;
                }
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Enemy")
        {
            StartCoroutine(Thunder());
        }
    }

    IEnumerator Thunder()
    {
        if (thunder) yield break;
        thunder = true;
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(ReStart());
        thunder = false;
    }

    IEnumerator ReStart()
    {
        if (Time.timeScale == 0) yield break;
        transform.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
        transform.GetComponent<Rigidbody2D>().isKinematic = true;
        Time.timeScale = 0;
        yield return StartCoroutine(Main.Fade(new Color(0,0,0,0),new Color(0,0,0,1),0.5f));
        transform.rotation = Quaternion.identity;
        transform.position = startPosition;
        Camera.main.gameObject.GetComponent<Scroll>().ResetPosition(startPosition);
        GetComponent<ObjectA>().Clear();
        transform.GetComponent<Rigidbody2D>().isKinematic = false;
        Time.timeScale = 1;
        yield return StartCoroutine(Main.Fade(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.5f));
    }
}
