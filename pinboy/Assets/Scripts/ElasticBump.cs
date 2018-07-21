using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElasticBump : MonoBehaviour {
    private Vector3 originScale;
    private bool flag;
    public float bumpSpeed = 0.06f;
    private float deltaTime;
    public float insistTime = 0.1f;
    public Vector3 addScale;

    // Use this for initialization
    void Start () {
        originScale = transform.localScale;
        deltaTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (flag) {
            transform.localScale = Vector3.Lerp(transform.localScale, originScale + addScale, bumpSpeed);
            deltaTime += Time.deltaTime;
            if (deltaTime >= insistTime) {
                deltaTime = 0;
                flag = false;
            }
        }
        else
            transform.localScale = Vector3.Lerp(transform.localScale, originScale, bumpSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.collider.name == "Ball")
        {
            flag = true;
            deltaTime = 0;
        }
    }
}
