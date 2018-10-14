﻿using System.Collections;

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
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;



    [Header("Set Dynamically")]

    public bool isFocusMode = false;

    public int graze = 0;
    public float camWidth;

    public float camHeight;

/*    [SerializeField]
    public float _shieldLevel = 1;

    private GameObject lastTriggerGo = null;
*/

/*    public float shieldLevel
    {
	get
	{
	    return _shieldLevel;
	}
	set
	{
	    _shieldLevel = Mathf.Min(value, 4);

	    if(value < 0)
	    {
		Destroy(this.gameObject);
	    }
	}
    }
*/
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


	if(Input.GetKeyDown(KeyCode.Space))
	{
	    TempFire();
	}
    }

    void OnTriggerEnter(Collider other)
    {
	Transform rootT = other.gameObject.transform.root;
	GameObject go = rootT.gameObject;

	if(go.tag == "Enemy" || go.tag == "ProjectileEnemy")
	{
	    graze += 1;
	}
    }
    void TempFire()
    {
	GameObject projGO = Instantiate<GameObject>(projectilePrefab);
	projGO.transform.position = transform.position;
	Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
	rigidB.velocity = Vector3.up * projectileSpeed;
    }
}