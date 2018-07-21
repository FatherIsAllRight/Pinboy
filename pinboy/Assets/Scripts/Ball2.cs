using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball2 : MonoBehaviour {


    private Rigidbody2D rigidbody2D;
    public Transform leftRotateCenter;
    public Transform rightRotateCenter;
    public float force;
    public LeftPedalRotate leftPedal;
    public RightPedalRotate rightPedal;

    private int bananaState;    //0--normal; 1--gwent
    private int mission;  //0--normal; 1--underwear; 2--fork; 3--gwent; 4--over

    private int talk = 0;
    private bool underwear = false;
    private bool fork = false;
    private bool gwent = false;
    private int blockNum = 8;

    public Vector2 initPos;
    private int life = 5;
    public GameObject[] hearts;
    public Sprite[] sprites;
    public GameObject itemIcon;
    public GameObject bananaObj;
    public GameObject forkObj;

    public GameObject questionMark;
    public GameObject fuckMark;

    public GameObject content;
    public Sprite[] contentSprites;

    private GameObject[] iceBlocks;

    public GameObject gameover;

    //pause parameters
    private Vector3 itemIconFinal;
    public float itemIconMove = 3.0f;
    private bool pause = false;
    private Vector3 pauseSpeed;

    // Use this for initialization
    void Start () {
        rigidbody2D = GetComponent<Rigidbody2D>();
        bananaState = 0;
        iceBlocks = GameObject.FindGameObjectsWithTag("Ice");
        itemIconFinal = itemIcon.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if(blockNum == 0)
        {
            if (life < 5)
            {
                hearts[life].SetActive(true);
                life++;
            }
            
            for (int i = 0; i < iceBlocks.Length; i++)
                iceBlocks[i].SetActive(true);
            blockNum = 8;
        }

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(3);
        }
        if(talk == 6 && content.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                content.GetComponent<SpriteRenderer>().sprite = contentSprites[talk];
                talk++;
                Debug.Log("lets gwent!");
                gwent = true;

                //itemIcon.transform.position -= new Vector3(0f, 0.25f, 0f);
                //itemIcon.GetComponent<SpriteRenderer>().sprite = sprites[2];
                //itemIcon.SetActive(true);
                fuckMark.SetActive(true);
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                talk = 4;
                Debug.Log("fuck off");
                content.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && content.activeSelf)
        {
            if(mission == 4)
                SceneManager.LoadScene(4);
            if (talk < 2 || talk == 5)
            {
                content.GetComponent<SpriteRenderer>().sprite = contentSprites[talk];
                talk++;
                Debug.Log(talk);
            }
            else
            {
                content.GetComponent<SpriteRenderer>().sprite = contentSprites[talk];
                //talk++;
                Debug.Log(talk);
                content.SetActive(false);
                if(talk == 7)
                {

                    pause = true;
                    pauseSpeed = rigidbody2D.velocity;
                    rigidbody2D.bodyType = RigidbodyType2D.Static;
                    itemIcon.transform.position = GameObject.Find("House").transform.position;
                    itemIcon.GetComponent<SpriteRenderer>().sprite = sprites[2];
                    itemIcon.SetActive(true);
                }
            }
        }
        else if (gameover.activeSelf && Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(3);

        /*else if(Input.GetKeyDown(KeyCode.Space) && pause)
        {
            pause = false;
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            rigidbody2D.velocity = pauseSpeed;
            //rigidbody2D.bodyType = RigidbodyType2D.Static;
        }*/

        else if(pause)
        {
            if(itemIcon.transform.position.y > itemIconFinal.y)
            {
                itemIcon.transform.position = Vector3.MoveTowards(itemIcon.transform.position, itemIconFinal, itemIconMove * Time.deltaTime);
            }
            else
            {
                pause = false;
                rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                rigidbody2D.velocity = pauseSpeed;
            }

        }

        if (transform.position.y < -10)
        {

            life--;
            if (life >= 0)
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

    void OnCollisionStay2D(Collision2D collision2D)
    {
        if (collision2D.collider.name == "LeftPedal" && leftPedal.isRotating())
            rigidbody2D.AddForce(collision2D.transform.up * force * (transform.position.x - leftRotateCenter.position.x) / (collision2D.transform.position.x - leftRotateCenter.position.x));
        else if (collision2D.collider.name == "RightPedal" && rightPedal.isRotating())
            rigidbody2D.AddForce(collision2D.transform.up * force * (transform.position.x - rightRotateCenter.position.x) / (collision2D.transform.position.x - rightRotateCenter.position.x));
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.collider.name == "Underwear" || collision2D.collider.name == "House")
        {
            AudioSource music = GameObject.Find("BananaMusic").GetComponent<AudioSource>();
            music.Play();
        }
        else if (collision2D.collider.name == "LeftPedal" || collision2D.collider.name == "RightPedal")
        {
            AudioSource music = GameObject.Find("PedalMusic").GetComponent<AudioSource>();
            music.Play();
        }
        else if (collision2D.collider.name == "Temple")
        {
            AudioSource music = GameObject.Find("TempleMusic").GetComponent<AudioSource>();
            music.Play();
        }
        else if (collision2D.collider.tag == "Ice")
        {
            AudioSource music = GameObject.Find("IceMusic").GetComponent<AudioSource>();
            music.Play();
        }
        else if (collision2D.collider.name == "Content")
        {
            AudioSource music = GameObject.Find("ContentMusic").GetComponent<AudioSource>();
            music.Play();
        }

        if (collision2D.collider.tag == "Ice")
        {
            collision2D.gameObject.SetActive(false);
            blockNum--;
        }
        else if(collision2D.collider.name == "Temple" && !content.activeSelf)
        {
            if(mission == 0)
            {
                Debug.Log("fk hero hit me 0!");
                content.SetActive(true);
                questionMark.SetActive(false);
                mission++;
            }
            else if(mission == 1 && underwear)
            {
                Debug.Log("fk hero hit me 1 and get underwear");
                
                content.GetComponent<SpriteRenderer>().sprite = contentSprites[talk];
                talk++;
                content.SetActive(true);
                mission++;
                underwear = false;
                itemIcon.SetActive(false);
                fuckMark.SetActive(false);
            }
            else if(mission == 2 && fork)
            {
                Debug.Log("fk hero hit me 2 and get fork");
                
                content.GetComponent<SpriteRenderer>().sprite = contentSprites[talk];
                talk++;
                content.SetActive(true);
                mission++;
                fork = false;
                bananaObj.SetActive(true);
                itemIcon.SetActive(false);
                fuckMark.SetActive(false);
            }
            else if (mission == 3 && gwent)
            {
                Debug.Log("fk hero hit me 3 and get gwent");
                content.GetComponent<SpriteRenderer>().sprite = contentSprites[talk];
                content.SetActive(true);
                gwent = false;
                mission++;
                itemIcon.SetActive(false);
                fuckMark.SetActive(false);

            }
        }

        else if (collision2D.collider.name == "House" && !content.activeSelf)
        {
            if(mission == 2 && !fork)
            {
                pause = true;
                pauseSpeed = rigidbody2D.velocity;
                rigidbody2D.bodyType = RigidbodyType2D.Static;

                fork = true;
                forkObj.SetActive(false);
                Debug.Log("u steal the fking fork!");
                //itemIcon.transform.position += new Vector3(0f, 0.25f, 0f);
                itemIcon.transform.position = collision2D.transform.position;
                itemIcon.GetComponent<SpriteRenderer>().sprite = sprites[1];
                itemIcon.SetActive(true);
                fuckMark.SetActive(true);
            }
            else if(mission == 3 && !gwent)
            {
                //gwent = true;
                //Debug.Log("u get the gwent!");
                content.GetComponent<SpriteRenderer>().sprite = contentSprites[talk];
                talk++;
                content.SetActive(true);
            }
        }
        else if(collision2D.collider.name == "Underwear" && mission == 1 && !content.activeSelf && !underwear)
        {
            Debug.Log("u steal the fking underwear!");
            pause = true;
            pauseSpeed = rigidbody2D.velocity;
            rigidbody2D.bodyType = RigidbodyType2D.Static;

            itemIcon.transform.position = collision2D.transform.position;

            underwear = true;
            itemIcon.GetComponent<SpriteRenderer>().sprite = sprites[0];
            itemIcon.SetActive(true);
            fuckMark.SetActive(true);
        }
    }
}
