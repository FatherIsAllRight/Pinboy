using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana1 : MonoBehaviour {
    private int talk = 0;
    private bool flag = false;
    public GameObject content;
    public GameObject fire;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(flag && Input.GetKey(KeyCode.Space))
        {
            flag = false;
            content.SetActive(false);
            fire.SetActive(true);
        }
	}

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        /*
        if (collision2D.collider.name == "Ball" && !flag)
        {
            if (talk == 0)
            {
                talk++;
                flag = true;
                content.SetActive(true);
            }
        }
        */
            
    }
}
