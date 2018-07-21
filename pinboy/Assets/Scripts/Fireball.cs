using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {
    public GameObject ball;
    public float v;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float x = transform.position.x - ball.transform.position.x;
        float y = transform.position.y - ball.transform.position.y;
        x = x * v / Mathf.Sqrt(x * x + y * y);
        y = y * v / Mathf.Sqrt(x * x + y * y);
        GetComponent<Rigidbody2D>().velocity = - new Vector3(x, y, 0);
    }
}
