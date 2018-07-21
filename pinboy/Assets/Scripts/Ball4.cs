using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball4 : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    public Transform leftRotateCenter;
    public Transform rightRotateCenter;
    public Transform rightRotateCenter2;
    public float force;
    public LeftPedalRotate leftPedal;
    public RightPedalRotate rightPedal;
    public RightPedalRotate rightPedal2;
    public GameObject fireball;
    public Boss boss;
    public float insistTime;
    private float deltaTime;
    private int ballState;  //0--normal; 1--fire; 2--ice

    public GameObject[] hearts;
    public GameObject itemIcon;
    public Vector2 initPos;
    public Sprite[] sprites;

    private int life = 5;
    //private int talk = 0;
    private bool flag = false;
    public GameObject content;
    public GameObject storm;
    public bool fuckedByStorm = false;

    public GameObject gameover;

    // Use this for initialization
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        deltaTime = 0;
        fuckedByStorm = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ballState == 2)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime >= insistTime)
            {
                ballState = 0;
                deltaTime = 0;
                //need change sprite
                Debug.Log("lose ice");
                itemIcon.SetActive(false);
                GetComponent<SpriteRenderer>().sprite = sprites[0];
            }
        }
        if (Input.GetKey(KeyCode.Space) && gameover.activeSelf)
        {
            SceneManager.LoadScene(6);
        }

        if (transform.position.y < -10)
        {
            if (transform.position.y < -10)
            {
                BallHurt();
                transform.position = new Vector3(initPos.x, initPos.y, 0);
                rigidbody2D.velocity = Vector3.zero;
            }
        }
        
    }

    void OnCollisionStay2D(Collision2D collision2D)
    {
        if (collision2D.collider.name == "LeftPedal" && leftPedal.isRotating())
            rigidbody2D.AddForce(collision2D.transform.up * force * (transform.position.x - leftRotateCenter.position.x) / (collision2D.transform.position.x - leftRotateCenter.position.x));
        else if (collision2D.collider.name == "RightPedal" && rightPedal.isRotating())
            rigidbody2D.AddForce(collision2D.transform.up * force * (transform.position.x - rightRotateCenter.position.x) / (collision2D.transform.position.x - rightRotateCenter.position.x));
        else if (collision2D.collider.name == "RightPedal2" && rightPedal2.isRotating())
            rigidbody2D.AddForce(collision2D.transform.up * force * (transform.position.x - rightRotateCenter2.position.x) / (collision2D.transform.position.x - rightRotateCenter2.position.x));

    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.collider.name == "House1")
        {
            AudioSource music = GameObject.Find("BananaMusic").GetComponent<AudioSource>();
            if (music != null)
                music.Play();
        }
        else if (collision2D.collider.name == "LeftPedal" || collision2D.collider.name == "RightPedal")
        {
            AudioSource music = GameObject.Find("PedalMusic").GetComponent<AudioSource>();
            if (music != null) music.Play();
        }
        else if (collision2D.collider.name == "Content")
        {
            AudioSource music = GameObject.Find("ContentMusic").GetComponent<AudioSource>();
            if (music != null) music.Play();
        }
        else if (collision2D.collider.tag == "Monster")
        {
            AudioSource music = GameObject.Find("BananaMusic").GetComponent<AudioSource>();
            if (music != null) music.Play();
        }

        if (collision2D.collider.tag == "Monster")
        {
            Debug.Log("hit monster");
            collision2D.gameObject.SetActive(false);
        }
        else if (collision2D.collider.name == "Boss")
        {
            boss.hurt();
        }
    }

    private void BallHurt()
    {
        life--;
        if (life >= 0)
        {
            hearts[life].SetActive(false);
            AudioSource music = GameObject.Find("HpHurtMusic").GetComponent<AudioSource>();
            if (music != null) music.Play();
        }

        if (life <= 0)
        //SceneManager.LoadScene(1);
        {
            gameover.SetActive(true);
            GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
            for(int i = 0; i < musics.Length; i++)
            {
                AudioSource music = musics[i].GetComponent<AudioSource>();
                music.volume = 0;
            }
            
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Ice")
        {
            AudioSource music = GameObject.Find("IceMusic").GetComponent<AudioSource>();
            if (music != null) music.Play();
        }

        if (collider.name == "Ice")
        {
            deltaTime = 0;
            ballState = 2;
            itemIcon.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = sprites[1];
            //Debug.Log("ice");
        }
        else if (collider.name == "Fireball")
        {
            if (ballState != 2)
                BallHurt();
            ballState = 0;
            itemIcon.SetActive(false);
            GetComponent<SpriteRenderer>().sprite = sprites[0];
            Debug.Log("fking fireball!");
            fireball.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Fire")
        {
            AudioSource music = GameObject.Find("FireMusic").GetComponent<AudioSource>();
            if (music != null) music.Play();
        }

        if (collision.name == "Storm" && !fuckedByStorm)
        {
            Debug.Log("bei da le");
            fuckedByStorm = true;
            BallHurt();
        }
    }
}
