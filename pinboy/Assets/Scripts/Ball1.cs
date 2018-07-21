using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball1 : MonoBehaviour {

    private Rigidbody2D rigidbody2D;
    public Transform leftRotateCenter;
    public Transform rightRotateCenter;
    public float force;
    public LeftPedalRotate leftPedal;
    public RightPedalRotate rightPedal;
    public IceBlock iceBlock;
    public float insistTime;
    private float deltaTime;
    private int ballState;  //0--normal; 1--fire; 2--ice
    
    private int talk = 0;
    private bool flag = false;
    public GameObject content;
    public GameObject fire;
    private int life = 5;
    public GameObject[] hearts;
    public Vector2 initPos;
    public Sprite[] sprites;
    public GameObject fireIcon;
    public Sprite[] contentSprites;

    public GameObject questionMark;

    public GameObject gameover;

    // Use this for initialization
    void Start () {
        rigidbody2D = GetComponent<Rigidbody2D>();
        deltaTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
	    if(ballState == 1)
        {
            deltaTime += Time.deltaTime;
            if(deltaTime >= insistTime)
            {
                ballState = 0;
                deltaTime = 0;
                //need change sprite
                Debug.Log("lose fire");
                GetComponent<SpriteRenderer>().sprite = sprites[0];
                fireIcon.SetActive(false);
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if(gameover.activeSelf)
                SceneManager.LoadScene(1);

            if (content.activeSelf && !iceBlock.gameObject.activeSelf)
            {
                //need shift content sprites
                content.SetActive(false);
                Debug.Log("daasd");
                SceneManager.LoadScene(2);
            }
            else if (flag)
            {
                flag = false;
                content.SetActive(false);
                fire.SetActive(true);
                fireIcon.SetActive(true);
            }
        }

        /*else if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }*/

        if (transform.position.y < -10)
        {
            
            life--;
            if(life >= 0)
            {
                hearts[life].SetActive(false);
                AudioSource music = GameObject.Find("HpHurtMusic").GetComponent<AudioSource>();
                music.Play();
            }
               
            if (life <= 0)
            //SceneManager.LoadScene(1);
            {
                gameover.SetActive(true);
                //GameObject.Find("Music").SetActive(false);
            }
            else
            {
                transform.position = new Vector3(initPos.x, initPos.y, 0);
                rigidbody2D.velocity = Vector3.zero;
            }
        }
    }

    void OnCollisionStay2D (Collision2D collision2D)
    {
        if (collision2D.collider.name == "LeftPedal" && leftPedal.isRotating())
            rigidbody2D.AddForce(collision2D.transform.up * force * (transform.position.x - leftRotateCenter.position.x) / (collision2D.transform.position.x - leftRotateCenter.position.x));
        else if (collision2D.collider.name == "RightPedal" && rightPedal.isRotating())
            rigidbody2D.AddForce(collision2D.transform.up * force * (transform.position.x - rightRotateCenter.position.x) / (collision2D.transform.position.x - rightRotateCenter.position.x));
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.collider.name == "Banana" || collision2D.collider.name == "Field")
        {
            AudioSource music = GameObject.Find("BananaMusic").GetComponent<AudioSource>();
            music.Play();
        }
        else if (collision2D.collider.name == "LeftPedal" || collision2D.collider.name == "RightPedal")
        {
            AudioSource music = GameObject.Find("PedalMusic").GetComponent<AudioSource>();
            music.Play();
        }
        else if (collision2D.collider.name == "IceBlock")
        {
            AudioSource music = GameObject.Find("IceBlockMusic").GetComponent<AudioSource>();
            music.Play();
        }
        else if (collision2D.collider.name == "Content")
        {
            AudioSource music = GameObject.Find("ContentMusic").GetComponent<AudioSource>();
            music.Play();
        }

        if (collision2D.collider.name == "IceBlock" && ballState == 1)
        {
            ballState = 0;
            GetComponent<SpriteRenderer>().sprite = sprites[0];
            fireIcon.SetActive(false);
            Debug.Log("fking ice block!");
            if (iceBlock.hurt())
            {
                iceBlock.gameObject.SetActive(false);
                content.GetComponent<SpriteRenderer>().sprite = contentSprites[0];
                content.SetActive(true);
            }
        }

        if (collision2D.collider.name == "Banana" && !flag)
        {
            if (talk == 0)
            {
                talk++;
                flag = true;
                content.SetActive(true);
                questionMark.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Fire")
        {
            AudioSource music = GameObject.Find("FireMusic").GetComponent<AudioSource>();
            music.Play();
        }

        if (collider.name == "Fire")
        {
            deltaTime = 0;
            ballState = 1;
            Debug.Log("fire");
            GetComponent<SpriteRenderer>().sprite = sprites[1];
            fireIcon.SetActive(true);
        }
    }
}
