using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;

    [Header("Set in Inspector")]
    public float speed = 30;
    public float speedFocus = 10;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float radius = 1f;

    [Header("Set Dynamically")]
    public bool isFocusMode = false;
    public float shieldLevel = 1;
    public float camWidth;
    public float camHeight;

    // Use this for initialization
    void Awake()
    {
		if(S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S");
        }

        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }
	
	// Update is called once per frame
	void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        if (Input.GetKey("right shift") == true || Input.GetKey("left shift") == true)
        {
            isFocusMode = true;
        }
        else
        {
            isFocusMode = false;
        }

        Vector3 pos = transform.position;
        if(isFocusMode == true)
        {
            pos.x += xAxis * speedFocus * Time.deltaTime;
            pos.y += yAxis * speedFocus * Time.deltaTime;
        }
        else
        {
            pos.x += xAxis * speed * Time.deltaTime;
            pos.y += yAxis * speed * Time.deltaTime;
        }

        if (pos.x > camWidth + radius)
        {
            pos.x = -camWidth - radius + 1;
        }
        if (pos.x < -camWidth - radius)
        {
            pos.x = camWidth + radius - 1;
        }
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
        }
        if (pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
        }

        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
	}
}
