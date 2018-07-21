using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPedalRotate : MonoBehaviour {
    public Transform rightRotateCenter;
    public float rotateSpeed;
    public float loveTime;
    private float deltaTime;
    public bool inLove;

    private KeyCode controlLeft = KeyCode.LeftArrow;
    private KeyCode controlRight = KeyCode.RightArrow;
    public Sprite[] pedelSprites;

    // Use this for initialization
    void Start()
    {
        inLove = false;
        deltaTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (inLove)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime >= loveTime)
            {
                deltaTime = 0;
                inLove = false;
                GetComponent<SpriteRenderer>().sprite = pedelSprites[0];
            }
        }

        float temp = transform.eulerAngles.z;
        while (temp > 180)
            temp -= 360;
        while (temp < -180)
            temp += 360;
        if (Input.GetKey(controlRight) && !inLove || Input.GetKey(controlLeft) && inLove)
        {
            if (temp > -30)
                transform.RotateAround(rightRotateCenter.position, new Vector3(0, 0, 1), rotateSpeed * Time.deltaTime);
            else
            {
                transform.RotateAround(rightRotateCenter.position, new Vector3(0, 0, 1), -30 - temp);
            }
        }
        else
        {
            if (temp < 30)
                transform.RotateAround(rightRotateCenter.position, new Vector3(0, 0, 1), -rotateSpeed * Time.deltaTime);
            else
            {
                transform.RotateAround(rightRotateCenter.position, new Vector3(0, 0, 1), 30.00001f - temp);
            }
        }
    }

    public bool isRotating()
    {
        float temp = transform.eulerAngles.z;
        while (temp > 180)
            temp -= 360;
        while (temp < -180)
            temp += 360;
        return (Input.GetKey(controlRight) && !inLove || Input.GetKey(controlLeft) && inLove) && temp > -30;
    }
}
