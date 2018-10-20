using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class Enemy_3 : Enemy
{

	[Header("Set in Inspector")]

	public float lifeTime = 5;

	[Header("Set Dynamically")]
	public Vector3[] points;
	public float birthTime;


	// Use this for initialization

	void Start()
	{

		points = new Vector3[3];

		points[0] = pos;

		float xMin = -bndCheck.camWidth + bndCheck.radius;
		float xMax = bndCheck.camWidth - bndCheck.radius;

		Vector3 v;

		v = Vector3.zero;
		v.x = Random.Range(xMin, xMax);
		v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
		v.z = -40;
		points[1] = v;

		v = Vector3.zero;
		v.y = pos.y;
		v.x = Random.Range(xMin, xMax);
		v.z = -40;
		points[2] = v;

		birthTime = Time.time;
	}


	
// Update is called once per frame

	public override void Move()
	{

		float u = (Time.time - birthTime) / lifeTime;

		if(u > 1)
		{
			Destroy(this.gameObject);
			return;
		}

		Vector3 p01;
		Vector3 p12;
		u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);

		p01 = (1-u) * points[0] + u * points[1];
		p12 = (1-u) * points[1] + u * points[2];
		pos = (1-u) * p01 + u * p12;
 	}

}
