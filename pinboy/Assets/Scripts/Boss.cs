using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour {
    public float warningTime;
    public float attackTime;
    private float deltaTime;
    private float moveTime;
    public float moveRound;
    public float moveSpeedX;
    public float moveSpeedY;
    private int attackType; // 0 none; 1 sex trade; 2 fire ball; 3 blizzard; 4 call 50 friends
    public LeftPedalRotate leftPedal;
    public RightPedalRotate rightPedal;
    public RightPedalRotate rightPedal2;
    private int state;
    public GameObject fireball;
    public GameObject[] monster1;
    public GameObject[] monster2;
    public GameObject storm;
    private float stormTime;
    public float stormInsistTime;

    public GameObject lure;
    public int hp;
    private Animator bossAni;


    //hp parameter
    public Transform redHp;
    public Transform yellowHp;
    private float moveHp = 3.64f;
    private bool boolHp = false;
    public float speedHp = 0.5f;
    private int maxHp;

    // Use this for initialization
    void Start () {
        deltaTime = 0;
        attackType = 0;
        state = 0;
        moveTime = 0;
        bossAni = GetComponent<Animator>();
        stormTime = 0;
        maxHp = hp;
    }
	
	// Update is called once per frame
	void Update () {

        if(boolHp)
        {
            if (yellowHp.position.x > redHp.position.x)
                yellowHp.position = Vector3.MoveTowards(yellowHp.position, redHp.position, speedHp * Time.deltaTime);
            else
            {
                yellowHp.position = redHp.position;
                boolHp = false;
            }

        }

        deltaTime += Time.deltaTime;
        moveTime += Time.deltaTime;
        moveTime = moveTime >= moveRound ? moveTime - moveRound : moveTime;
        if(moveTime >= 0 && moveTime < moveRound/4)
        {
            transform.position = new Vector3(transform.position.x + moveSpeedX * Time.deltaTime, transform.position.y + moveSpeedY * Time.deltaTime, transform.position.z);
        }
        else if (moveTime >= moveRound / 4 && moveTime < 2 * moveRound / 4)
        {
            transform.position = new Vector3(transform.position.x - moveSpeedX * Time.deltaTime, transform.position.y - moveSpeedY * Time.deltaTime, transform.position.z);
        }
        else if(moveTime >= 2 * moveRound/4  && moveTime < 3 * moveRound / 4)
        {
            transform.position = new Vector3(transform.position.x - moveSpeedX * Time.deltaTime, transform.position.y + moveSpeedY * Time.deltaTime, transform.position.z);
        }
        else if(moveTime >= 3 * moveRound / 4 && moveTime < moveRound)
        {
            transform.position = new Vector3(transform.position.x + moveSpeedX * Time.deltaTime, transform.position.y - moveSpeedY * Time.deltaTime, transform.position.z);
        }

        if(storm.activeSelf)
        {
            stormTime += Time.deltaTime;
            if(stormTime >= stormInsistTime)
            {
                stormTime = 0;
                storm.SetActive(false);
            }
        }

        if (deltaTime >= attackTime && state == 1)
        {
            deltaTime = 0;
            state = 0;
            //bossAni.SetInteger("SkillType", 0);
            if (attackType == 1)
            {
                AudioSource music = GameObject.Find("Attack1Music").GetComponent<AudioSource>();
                music.Play();

                bossAni.SetInteger("SkillType", 11);

                lure.SetActive(true);
                leftPedal.inLove = true;
                leftPedal.GetComponent<SpriteRenderer>().sprite = leftPedal.pedelSprites[1];
                rightPedal.inLove = true;
                rightPedal.GetComponent<SpriteRenderer>().sprite = rightPedal.pedelSprites[1];
                rightPedal2.inLove = true;
                rightPedal2.GetComponent<SpriteRenderer>().sprite = rightPedal2.pedelSprites[1];
                Debug.Log("Fuck you!!!!");
            }
            else if (attackType == 2)
            {
                AudioSource music = GameObject.Find("Attack2Music").GetComponent<AudioSource>();
                music.Play();

                bossAni.SetInteger("SkillType", 21);

                fireball.gameObject.SetActive(true);
                fireball.transform.position = transform.position;
                fireball.transform.rotation = Quaternion.Euler(0, 0, 0);
                fireball.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                Debug.Log("Fffffffffffffffffire!!!!");
            }
            else if (attackType == 3)
            {
                AudioSource music = GameObject.Find("Attack3Music").GetComponent<AudioSource>();
                music.Play();

                bossAni.SetInteger("SkillType", 31);

                storm.SetActive(true);
                GameObject.Find("Ball").GetComponent<Ball4>().fuckedByStorm = false;
                Debug.Log("Blizzard!!!!!!!");

            }
            else if (attackType == 4)
            {
                AudioSource music = GameObject.Find("Attack4Music").GetComponent<AudioSource>();
                music.Play();

                bossAni.SetInteger("SkillType", 41);

                if (Random.Range(0, 2) == 0)
                {
                    //monster1.SetActive(true); Transform[] monsterList = monster1.GetComponentsInChildren<Transform>();
                    for (int i = 0; i < monster1.Length; i++)
                    {
                        
                        monster1[i].gameObject.SetActive(true);
                    }
                }
                else
                {
                    for (int i = 0; i < monster2.Length; i++)
                    {

                        monster2[i].gameObject.SetActive(true);
                    }
                }
                Debug.Log("Brother fuck!!!!!!!");
            }
            attackType = 0;
        }
        else if (deltaTime >= warningTime && state == 0)
        {
            AudioSource music = GameObject.Find("WarningMusic").GetComponent<AudioSource>();
            music.Play();

            //bossAni.SetInteger("SkillType", 0);
            attackType = Random.Range(1, 5); //Random.Range(1, 5);
            //attackType = 1;
            state = 1;
            if (attackType == 1)
            {
                Debug.Log("I fuck you");
                bossAni.SetInteger("SkillType", 1);
            }
            else if (attackType == 2)
            {
                Debug.Log("Fire Hell^^^");
                bossAni.SetInteger("SkillType", 2);
            }
            else if (attackType == 3)
            {
                Debug.Log("Ice Age****");
                bossAni.SetInteger("SkillType", 3);
            }
            else if (attackType == 4)
            {
                Debug.Log("My brother");
                bossAni.SetInteger("SkillType", 4);
            }
        }
    }

    public void hurt()
    {
        hp--;
        redHp.position = new Vector3(redHp.position.x - moveHp / maxHp, redHp.position.y, redHp.position.z);
        boolHp = true;
        Debug.Log(hp);
        if(hp <= 0)
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene(7);
        }
    }
}
