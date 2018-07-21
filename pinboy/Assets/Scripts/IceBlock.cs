using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour {
    public int hp;
    public Sprite[] sprites;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool hurt()
    {

        hp--;
        if(hp == 2)
            GetComponent<SpriteRenderer>().sprite = sprites[0];
        else if(hp == 1)
            GetComponent<SpriteRenderer>().sprite = sprites[1];
        return hp <= 0;
    }
}
